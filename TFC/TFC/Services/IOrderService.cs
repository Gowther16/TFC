using TFC.DTOs;

namespace TFC.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResult> CreateOrderAsync(CreateOrderRequest request);
    }

    public class CreateOrderResult
    {
        public bool Success { get; set; }
        public string? OrderCode { get; set; }
        public string? Message { get; set; }
    }
}
