using Microsoft.AspNetCore.Mvc;
using TFC.DTOs;
using TFC.Services;

namespace TFC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
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
    }
}
