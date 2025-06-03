using Microsoft.AspNetCore.Mvc;
using TFC.DTOs;
using TFC.Services;

namespace TFC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                var result = await _orderService.CreateOrderAsync(request);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        orderCode = result.OrderCode,
                        message = "Đặt món thành công"
                    });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra khi xử lý đơn hàng" });
            }
        }
        [HttpPost("GetOrdersByOrderCode")]
        public async Task<IActionResult> GetOrdersByPhone([FromBody] GetOrdersByOrderCodeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.OrderCode))
                {
                    return BadRequest(new { success = false, message = "Order Code không được để trống" });
                }

                var result = await _orderService.GetOrdersByOrderCodeAsync(request.OrderCode);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        customerName = result.CustomerName,
                        orders = result.Orders,
                        message = $"Tìm thấy {result.Orders.Count} đơn hàng"
                    });
                }
                else
                {
                    return NotFound(new { success = false, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra khi tìm kiếm đơn hàng" });
            }
        }
    }

    public class GetOrdersByOrderCodeRequest
    {
        public string OrderCode { get; set; }
    }
}
