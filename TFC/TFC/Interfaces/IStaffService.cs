using TFC.Models;
using System.ComponentModel.DataAnnotations;

namespace TFC.Interfaces
{
    public interface IStaffService
    {
        Task<ServiceResult<IEnumerable<OrderDto>>> GetAllOrdersAsync();
        Task<ServiceResult<OrderDto>> GetOrderByIdAsync(decimal id);
        Task<ServiceResult<UpdateOrderStatusResult>> UpdateOrderStatusAsync(decimal id, string status);
        Task<ServiceResult<IEnumerable<OrderDto>>> GetOrdersByStatusAsync(string status);
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string? Error { get; set; }
        public int? Count { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "", int? count = null)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Count = count
            };
        }

        public static ServiceResult<T> ErrorResult(string message, string? error = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                Error = error
            };
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
        public DateTime? UpdatedAt { get; set; }
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

    public class UpdateOrderStatusResult
    {
        public decimal OrderId { get; set; }
        public string? OldStatus { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}