using TFC.Models;
using System.ComponentModel.DataAnnotations;
using static TFC.DTOs.AdminDTO;

namespace TFC.Interfaces
{
    public interface IAdminService
    {
        #region User Management
        Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync();
        Task<ServiceResult<User>> GetUserByIdAsync(decimal id);
        Task<ServiceResult<User>> CreateUserAsync(CreateUserRequest request);
        Task<ServiceResult<User>> UpdateUserAsync(decimal id, UpdateUserRequest request);
        Task<ServiceResult<bool>> DeleteUserAsync(decimal id);
        #endregion

        #region Product Management
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResult<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<ServiceResult<ProductDto>> GetProductByIdAsync(decimal id);
        Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductRequest request);
        Task<ServiceResult<ProductDto>> UpdateProductAsync(decimal id, UpdateProductRequest request);
        Task<ServiceResult<bool>> DeleteProductAsync(decimal id);
        #endregion

        #region Combo Management
        Task<ServiceResult<IEnumerable<ComboDto>>> GetAllCombosAsync();
        Task<ServiceResult<ComboDetailDto>> GetComboByIdAsync(decimal id);
        Task<ServiceResult<ComboDto>> CreateComboAsync(CreateComboRequest request);
        Task<ServiceResult<ComboDto>> UpdateComboAsync(decimal id, UpdateComboRequest request);
        Task<ServiceResult<bool>> DeleteComboAsync(decimal id);
        #endregion
    }

    #region User DTOs and Requests

    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 100 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 và không quá 100 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role không được để trống")]
        public string Role { get; set; } = "Staff";

        public string Status { get; set; } = "active";
    }

    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role không được để trống")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status không được để trống")]
        public string Status { get; set; } = string.Empty;
    }
    #endregion

    #region Product Requests


    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        public decimal CategoryID { get; set; }

        public string Status { get; set; } = "active";

        [StringLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; } = 0;
    }

    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        public decimal? CategoryId { get; set; }

        [StringLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; }
    }
    #endregion

    #region Combo Requests
    public class ComboDto
    {
        public decimal Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool Available { get; set; }
        public int ProductCount { get; set; }
        public List<ComboProductDto> Products { get; set; } = new List<ComboProductDto>();
    }

    public class ComboDetailDto : ComboDto
    {
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ComboProductDto
    {
        public decimal ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImageUrl { get; set; }
    }

    public class ComboProductRequest
    {
        [Required(ErrorMessage = "Product ID không được để trống")]
        public decimal ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }

    public class CreateComboRequest
    {
        [Required(ErrorMessage = "Tên combo không được để trống")]
        [StringLength(255, ErrorMessage = "Tên combo không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
        public string? ImageUrl { get; set; }

        public string Status { get; set; } = "active";

        [Required(ErrorMessage = "Phải chọn ít nhất một sản phẩm cho combo")]
        [MinLength(1, ErrorMessage = "Combo phải có ít nhất một sản phẩm")]
        public List<ComboProductRequest> Products { get; set; } = new List<ComboProductRequest>();
    }

    public class UpdateComboRequest
    {
        [Required(ErrorMessage = "Tên combo không được để trống")]
        [StringLength(255, ErrorMessage = "Tên combo không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
        public string? ImageUrl { get; set; }

        public string Status { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phải chọn ít nhất một sản phẩm cho combo")]
        [MinLength(1, ErrorMessage = "Combo phải có ít nhất một sản phẩm")]
        public List<ComboProductRequest> Products { get; set; } = new List<ComboProductRequest>();
    }
    #endregion
}