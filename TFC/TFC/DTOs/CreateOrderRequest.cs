using System.ComponentModel.DataAnnotations;

namespace TFC.DTOs
{
    public class CreateOrderRequest
    {
        [Required]
        public CustomerInfo Customer { get; set; } = null!;

        [Required]
        public List<OrderItemInfo> Items { get; set; } = new List<OrderItemInfo>();

        [Required]
        public decimal TotalAmount { get; set; }
    }

    public class CustomerInfo
    {
        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
    }

    public class OrderItemInfo
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!; 

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}
