using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TFC.Interfaces;
using System.ComponentModel.DataAnnotations;
using TFC.Models;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminApiController> _logger;

        public AdminApiController(IAdminService adminService, ILogger<AdminApiController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        #region User Management APIs
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var result = await _adminService.GetAllUsersAsync();

            if (!result.Success)
            {
                _logger.LogError("Lỗi khi lấy danh sách users: {Message}", result.Message);
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

        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(decimal id)
        {
            var result = await _adminService.GetUserByIdAsync(id);

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

        [HttpPost("users")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
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

            var result = await _adminService.CreateUserAsync(request);

            if (!result.Success)
            {
                if (result.Message.Contains("đã tồn tại") || result.Message.Contains("không hợp lệ"))
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

            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, new
            {
                success = result.Success,
                data = result.Data,
                message = result.Message
            });
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult> UpdateUser(decimal id, [FromBody] UpdateUserRequest request)
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

            var result = await _adminService.UpdateUserAsync(id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("không hợp lệ") || result.Message.Contains("đã tồn tại"))
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

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(decimal id)
        {
            var result = await _adminService.DeleteUserAsync(id);

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
                message = result.Message
            });
        }
        #endregion
        
        #region Product Management APIs
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var result = await _adminService.GetAllProductsAsync();

            if (!result.Success)
            {
                _logger.LogError("Lỗi khi lấy danh sách products: {Message}", result.Message);
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

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var result = await _adminService.GetAllCategoriesAsync();

            if (!result.Success)
            {
                _logger.LogError("Lỗi khi lấy danh sách Categories: {Message}", result.Message);
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


        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProductById(decimal id)
        {
            var result = await _adminService.GetProductByIdAsync(id);

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

        [HttpPost("products")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequest request)
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

            var result = await _adminService.CreateProductAsync(request);

            if (!result.Success)
            {
                if (result.Message.Contains("đã tồn tại") || result.Message.Contains("không hợp lệ"))
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

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, new
            {
                success = result.Success,
                data = result.Data,
                message = result.Message
            });
        }

        [HttpPut("products/{id}")]
        public async Task<ActionResult> UpdateProduct(decimal id, [FromBody] UpdateProductRequest request)
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

            var result = await _adminService.UpdateProductAsync(id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("không hợp lệ") || result.Message.Contains("đã tồn tại"))
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

        [HttpDelete("products/{id}")]
        public async Task<ActionResult> DeleteProduct(decimal id)
        {
            var result = await _adminService.DeleteProductAsync(id);

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
                message = result.Message
            });
        }

        #endregion

        #region Combo Management APIs
        
        [HttpGet("combos")]
        public async Task<ActionResult<IEnumerable<ComboDto>>> GetAllCombos()
        {
            var result = await _adminService.GetAllCombosAsync();

            if (!result.Success)
            {
                _logger.LogError("Lỗi khi lấy danh sách products: {Message}", result.Message);
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

        [HttpGet("combos/{id}")]
        public async Task<ActionResult<Combo>> GetComboById(decimal id)
        {
            var result = await _adminService.GetComboByIdAsync(id);

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

        [HttpPost("combos")]
        public async Task<ActionResult> CreateCombo([FromBody] CreateComboRequest request)
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

            var result = await _adminService.CreateComboAsync(request);

            if (!result.Success)
            {
                if (result.Message.Contains("đã tồn tại") || result.Message.Contains("không hợp lệ"))
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

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, new
            {
                success = result.Success,
                data = result.Data,
                message = result.Message
            });
        }

        [HttpPut("combos/{id}")]
        public async Task<ActionResult> UpdateCombo(decimal id, [FromBody] UpdateComboRequest request)
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

            var result = await _adminService.UpdateComboAsync(id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("không hợp lệ") || result.Message.Contains("đã tồn tại"))
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

        [HttpDelete("combos/{id}")]
        public async Task<ActionResult> DeleteCombo(decimal id)
        {
            var result = await _adminService.DeleteComboAsync(id);

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
                message = result.Message
            });
        }

        #endregion
    }
}