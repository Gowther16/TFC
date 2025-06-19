using TFC.DTOs;
using static TFC.DTOs.FindOrderDTO;

namespace TFC.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResult> CreateOrderAsync(CreateOrderRequest request);
        Task<GetOrdersByOrderCodeResult> GetOrdersByOrderCodeAsync(string OrderCode);
    }

    public class CreateOrderResult
    {
        public bool Success { get; set; }
        public string? OrderCode { get; set; }
        public string? Message { get; set; }
    }
}
