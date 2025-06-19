using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TFC.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly ModelContext _context;
        private readonly ILogger<StaffApiController> _logger;

        public StaffApiController(ModelContext context, ILogger<StaffApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả đơn hàng có trạng thái "pending"
        /// </summary>
        /// <returns>Danh sách đơn hàng pending</returns>
        [HttpGet("pending-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPendingOrders()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy danh sách đơn hàng pending");

                var pendingOrders = await _context.Orders
                    .Where(o => o.Status != null && o.Status.ToLower() == "pending")
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        // Sử dụng OrderCode thay vì OrderNumber
                        OrderNumber = o.OrderCode ?? string.Empty,
                        CustomerName = o.Customer != null ? o.Customer.Name ?? "Khách hàng không xác định" : "Khách hàng không xác định",
                        Status = o.Status ?? "Unknown",
                        TotalAmount = o.TotalAmount ?? 0,
                        CreatedAt = o.CreatedAt,
                        OrderItems = o.Orderitems != null ? o.Orderitems.Select(oi => new OrderItemDto
                        {
                            Id = oi.Id,
                            ProductName = oi.Product != null ? oi.Product.Name : null,
                            ComboName = oi.Combo != null ? oi.Combo.Name : null,
                            Quantity = oi.Quantity ?? 0,
                            UnitPrice = oi.UnitPrice ?? 0,
                            SubTotal = (oi.Quantity ?? 0) * (oi.UnitPrice ?? 0)
                        }).ToList() : new List<OrderItemDto>()
                    })
                    .ToListAsync();

                _logger.LogInformation("Lấy thành công {Count} đơn hàng pending", pendingOrders.Count);

                return Ok(new
                {
                    success = true,
                    data = pendingOrders,
                    count = pendingOrders.Count,
                    message = "Lấy danh sách đơn hàng pending thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn hàng pending");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy danh sách đơn hàng",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy chi tiết một đơn hàng theo ID
        /// </summary>
        /// <param name="id">ID của đơn hàng</param>
        /// <returns>Chi tiết đơn hàng</returns>
        [HttpGet("order/{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "ID đơn hàng không hợp lệ"
                    });
                }

                _logger.LogInformation("Lấy chi tiết đơn hàng với ID: {OrderId}", id);

                var order = await _context.Orders
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng với ID: {OrderId}", id);
                    return NotFound(new
                    {
                        success = false,
                        message = "Không tìm thấy đơn hàng"
                    });
                }

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    // Sử dụng OrderCode thay vì OrderNumber
                    OrderNumber = order.OrderCode ?? string.Empty,
                    CustomerName = order.Customer != null ? order.Customer.Name ?? "Khách hàng không xác định" : "Khách hàng không xác định",
                    Status = order.Status ?? "Unknown",
                    TotalAmount = order.TotalAmount ?? 0,
                    CreatedAt = order.CreatedAt,
                    OrderItems = order.Orderitems != null ? order.Orderitems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductName = oi.Product != null ? oi.Product.Name : null,
                        ComboName = oi.Combo != null ? oi.Combo.Name : null,
                        Quantity = oi.Quantity ?? 0,
                        UnitPrice = oi.UnitPrice ?? 0,
                        SubTotal = (oi.Quantity ?? 0) * (oi.UnitPrice ?? 0)
                    }).ToList() : new List<OrderItemDto>()
                };

                _logger.LogInformation("Lấy chi tiết đơn hàng thành công với ID: {OrderId}", id);

                return Ok(new
                {
                    success = true,
                    data = orderDto,
                    message = "Lấy chi tiết đơn hàng thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết đơn hàng với ID: {OrderId}", id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy chi tiết đơn hàng",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        /// <param name="id">ID của đơn hàng</param>
        /// <param name="request">Trạng thái mới</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut("order/{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(decimal id, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "ID đơn hàng không hợp lệ"
                    });
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Status))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Trạng thái đơn hàng không được để trống"
                    });
                }

                // Validate status values
                var allowedStatuses = new[] { "pending", "confirmed", "preparing", "ready", "completed", "cancelled" };
                if (!allowedStatuses.Contains(request.Status.ToLower()))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Trạng thái không hợp lệ. Các trạng thái được phép: {string.Join(", ", allowedStatuses)}"
                    });
                }

                _logger.LogInformation("Cập nhật trạng thái đơn hàng ID: {OrderId} thành {Status}", id, request.Status);

                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng với ID: {OrderId}", id);
                    return NotFound(new
                    {
                        success = false,
                        message = "Không tìm thấy đơn hàng"
                    });
                }

                var oldStatus = order.Status;
                order.Status = request.Status;
                // Không cập nhật CreatedAt mà nên cập nhật UpdatedAt nếu có
                // order.CreatedAt = DateTime.UtcNow; // Remove this line

                await _context.SaveChangesAsync();

                _logger.LogInformation("Cập nhật trạng thái đơn hàng thành công. ID: {OrderId}, Trạng thái cũ: {OldStatus}, Trạng thái mới: {NewStatus}",
                    id, oldStatus, request.Status);

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật trạng thái đơn hàng thành công",
                    data = new
                    {
                        orderId = id,
                        oldStatus = oldStatus,
                        newStatus = request.Status,
                        updatedAt = DateTime.UtcNow
                    }
                });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Lỗi database khi cập nhật trạng thái đơn hàng với ID: {OrderId}", id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi cập nhật database",
                    error = "Database update failed"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái đơn hàng với ID: {OrderId}", id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi cập nhật trạng thái đơn hàng",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng theo trạng thái
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng</param>
        /// <returns>Danh sách đơn hàng theo trạng thái</returns>
        [HttpGet("orders/status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Trạng thái không được để trống"
                    });
                }

                _logger.LogInformation("Lấy danh sách đơn hàng với trạng thái: {Status}", status);

                var orders = await _context.Orders
                    .Where(o => o.Status != null && o.Status.ToLower() == status.ToLower())
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        // Sử dụng OrderCode thay vì OrderNumber
                        OrderNumber = o.OrderCode ?? string.Empty,
                        CustomerName = o.Customer != null ? o.Customer.Name ?? "Khách hàng không xác định" : "Khách hàng không xác định",
                        Status = o.Status ?? "Unknown",
                        TotalAmount = o.TotalAmount ?? 0,
                        CreatedAt = o.CreatedAt,
                        OrderItems = o.Orderitems != null ? o.Orderitems.Select(oi => new OrderItemDto
                        {
                            Id = oi.Id,
                            ProductName = oi.Product != null ? oi.Product.Name : null,
                            ComboName = oi.Combo != null ? oi.Combo.Name : null,
                            Quantity = oi.Quantity ?? 0,
                            UnitPrice = oi.UnitPrice ?? 0,
                            SubTotal = (oi.Quantity ?? 0) * (oi.UnitPrice ?? 0)
                        }).ToList() : new List<OrderItemDto>()
                    })
                    .ToListAsync();

                _logger.LogInformation("Lấy thành công {Count} đơn hàng với trạng thái {Status}", orders.Count, status);

                return Ok(new
                {
                    success = true,
                    data = orders,
                    count = orders.Count,
                    message = $"Lấy danh sách đơn hàng với trạng thái '{status}' thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn hàng với trạng thái: {Status}", status);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy danh sách đơn hàng",
                    error = ex.Message
                });
            }
        }
    }

    // DTO Classes
    public class OrderDto
    {
        public decimal Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public decimal Id { get; set; }
        public string? ProductName { get; set; }
        public string? ComboName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string Status { get; set; } = string.Empty;
    }
}
