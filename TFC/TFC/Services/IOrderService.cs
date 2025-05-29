using TFC.DTOs;
using static TFC.DTOs.FindOrderDTO;

namespace TFC.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResult> CreateOrderAsync(CreateOrderRequest request);
        Task<GetOrdersByPhoneResult> GetOrdersByPhoneAsync(string phoneNumber);
    }

    public class CreateOrderResult
    {
        public bool Success { get; set; }
        public string? OrderCode { get; set; }
        public string? Message { get; set; }
    }
}
