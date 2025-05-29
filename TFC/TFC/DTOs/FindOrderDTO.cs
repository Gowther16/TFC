namespace TFC.DTOs
{
    public class FindOrderDTO
    {
        public class GetOrdersByPhoneResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<FindOrder> Orders { get; set; } = new List<FindOrder>();
            public string CustomerName { get; set; }
        }

        public class FindOrder
        {
            public decimal Id { get; set; }
            public string OrderCode { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        }

        public class OrderItemDTO
        {
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public string ProductName { get; set; }
            public string ComboName { get; set; }
        }
    }
}
