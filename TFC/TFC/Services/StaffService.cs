using Microsoft.EntityFrameworkCore;
using TFC.Interfaces;
using TFC.Models;

namespace TFC.Services
{
    public class StaffService : IStaffService
    {
        private readonly ModelContext _context;
        private readonly ILogger<StaffService> _logger;

        public StaffService(ModelContext context, ILogger<StaffService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy tất cả đơn hàng");

                var orders = await _context.Orders
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
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

                _logger.LogInformation("Lấy thành công {Count} đơn hàng", orders.Count);

                return ServiceResult<IEnumerable<OrderDto>>.SuccessResult(
                    orders,
                    "Lấy tất cả đơn hàng thành công",
                    orders.Count
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả đơn hàng");
                return ServiceResult<IEnumerable<OrderDto>>.ErrorResult(
                    "Có lỗi xảy ra khi lấy danh sách đơn hàng",
                    ex.Message
                );
            }
        }

        public async Task<ServiceResult<OrderDto>> GetOrderByIdAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<OrderDto>.ErrorResult("ID đơn hàng không hợp lệ");
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
                    return ServiceResult<OrderDto>.ErrorResult("Không tìm thấy đơn hàng");
                }

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderCode ?? string.Empty,
                    CustomerName = order.Customer != null ? order.Customer.Name ?? "Khách hàng không xác định" : "Khách hàng không xác định",
                    Status = order.Status ?? "Unknown",
                    TotalAmount = order.TotalAmount ?? 0,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
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

                return ServiceResult<OrderDto>.SuccessResult(orderDto, "Lấy chi tiết đơn hàng thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết đơn hàng với ID: {OrderId}", id);
                return ServiceResult<OrderDto>.ErrorResult(
                    "Có lỗi xảy ra khi lấy chi tiết đơn hàng",
                    ex.Message
                );
            }
        }

        public async Task<ServiceResult<UpdateOrderStatusResult>> UpdateOrderStatusAsync(decimal id, string status)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<UpdateOrderStatusResult>.ErrorResult("ID đơn hàng không hợp lệ");
                }

                if (string.IsNullOrWhiteSpace(status))
                {
                    return ServiceResult<UpdateOrderStatusResult>.ErrorResult("Trạng thái đơn hàng không được để trống");
                }

              
                var allowedStatuses = new[] { "pending", "confirmed", "completed"};
                if (!allowedStatuses.Contains(status.ToLower()))
                {
                    return ServiceResult<UpdateOrderStatusResult>.ErrorResult(
                        $"Trạng thái không hợp lệ. Các trạng thái được phép: {string.Join(", ", allowedStatuses)}"
                    );
                }

                _logger.LogInformation("Cập nhật trạng thái đơn hàng ID: {OrderId} thành {Status}", id, status);

                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng với ID: {OrderId}", id);
                    return ServiceResult<UpdateOrderStatusResult>.ErrorResult("Không tìm thấy đơn hàng");
                }

                var oldStatus = order.Status;
                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                var result = new UpdateOrderStatusResult
                {
                    OrderId = id,
                    OldStatus = oldStatus,
                    NewStatus = status,
                    UpdatedAt = DateTime.Now
                };

                _logger.LogInformation("Cập nhật trạng thái đơn hàng thành công. ID: {OrderId}, Trạng thái cũ: {OldStatus}, Trạng thái mới: {NewStatus}",
                    id, oldStatus, status);

                return ServiceResult<UpdateOrderStatusResult>.SuccessResult(result, "Cập nhật trạng thái đơn hàng thành công");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Lỗi database khi cập nhật trạng thái đơn hàng với ID: {OrderId}", id);
                return ServiceResult<UpdateOrderStatusResult>.ErrorResult(
                    "Có lỗi xảy ra khi cập nhật database",
                    "Database update failed"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái đơn hàng với ID: {OrderId}", id);
                return ServiceResult<UpdateOrderStatusResult>.ErrorResult(
                    "Có lỗi xảy ra khi cập nhật trạng thái đơn hàng",
                    ex.Message
                );
            }
        }

        public async Task<ServiceResult<IEnumerable<OrderDto>>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return ServiceResult<IEnumerable<OrderDto>>.ErrorResult("Trạng thái không được để trống");
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

                return ServiceResult<IEnumerable<OrderDto>>.SuccessResult(
                    orders,
                    $"Lấy danh sách đơn hàng với trạng thái '{status}' thành công",
                    orders.Count
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn hàng với trạng thái: {Status}", status);
                return ServiceResult<IEnumerable<OrderDto>>.ErrorResult(
                    "Có lỗi xảy ra khi lấy danh sách đơn hàng",
                    ex.Message
                );
            }
        }
    }
}