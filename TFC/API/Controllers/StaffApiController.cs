using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TFC.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffApiController> _logger;

        public StaffApiController(IStaffService staffService, ILogger<StaffApiController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var result = await _staffService.GetAllOrdersAsync();

            if (!result.Success)
            {
                if (result.Message.Contains("không được để trống"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    message = result.Message,
                    error = result.Error
                });
            }

            return Ok(new
            {
                success = result.Success,
                data = result.Data,
                count = result.Count,
                message = result.Message
            });
        }

        [HttpGet("orders/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(string status)
        {
            var result = await _staffService.GetOrdersByStatusAsync(status);

            if (!result.Success)
            {
                if (result.Message.Contains("không được để trống"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    message = result.Message,
                    error = result.Error
                });
            }

            return Ok(new
            {
                success = result.Success,
                data = result.Data,
                count = result.Count,
                message = result.Message
            });
        }

        [HttpGet("order/{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(decimal id)
        {
            var result = await _staffService.GetOrderByIdAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("không hợp lệ"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                if (result.Message.Contains("Không tìm thấy"))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    message = result.Message,
                    error = result.Error
                });
            }

            return Ok(new
            {
                success = result.Success,
                data = result.Data,
                message = result.Message
            });
        }

        [HttpPut("order/{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(decimal id, [FromBody] UpdateOrderStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Request body không được để trống"
                });
            }

            // Validate model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    success = false,
                    message = "Dữ liệu không hợp lệ",
                    errors = errors
                });
            }

            var result = await _staffService.UpdateOrderStatusAsync(id, request.Status);

            if (!result.Success)
            {
                if (result.Message.Contains("không hợp lệ") || result.Message.Contains("không được để trống"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                if (result.Message.Contains("Không tìm thấy"))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    message = result.Message,
                    error = result.Error
                });
            }

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            });
        }
    }
}