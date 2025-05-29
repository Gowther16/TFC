using Microsoft.EntityFrameworkCore;
using TFC.DTOs;
using TFC.Models;

namespace TFC.Services
{
    public class OrderService : IOrderService
    {
        private readonly ModelContext _context; 

        public OrderService(ModelContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var customer = await FindOrCreateCustomerAsync(request.Customer);

                var orderCode = GenerateOrderCode();

                var order = new Order
                {
                    OrderCode = orderCode,
                    CustomerId = customer.Id,
                    TotalAmount = request.TotalAmount,
                    Status = "Pending", 
                    CreatedAt = DateTime.Now
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderItems = new List<Orderitem>();

                foreach (var item in request.Items)
                {
                    var orderItem = new Orderitem
                    {
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        ProductId = null,
                        ComboId = null
                    };

                    if (item.Type.ToLower() == "product")
                    {
                        var productId = decimal.Parse(item.Id);
                        var productCount = await _context.Products.CountAsync(p => p.Id == productId);

                        if (productCount == 0)
                        {
                            throw new Exception($"Sản phẩm với ID {item.Id} không tồn tại");
                        }

                        orderItem.ProductId = productId;
                        orderItem.ComboId = null; 
                    }
                    else if (item.Type.ToLower() == "combo")
                    {
                        var comboId = decimal.Parse(item.Id);
                        var comboCount = await _context.Combos.CountAsync(c => c.Id == comboId);

                        if (comboCount == 0)
                        {
                            throw new Exception($"Combo với ID {item.Id} không tồn tại");
                        }

                        orderItem.ComboId = comboId;
                        orderItem.ProductId = null; 
                    }
                    else
                    {
                        throw new Exception($"Loại item không hợp lệ: {item.Type}");
                    }

                    orderItems.Add(orderItem);
                }

                _context.Orderitems.AddRange(orderItems);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new CreateOrderResult
                {
                    Success = true,
                    OrderCode = orderCode,
                    Message = "Đặt món thành công"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new CreateOrderResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private async Task<Customer> FindOrCreateCustomerAsync(CustomerInfo customerInfo)
        {
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == customerInfo.Phone);

            if (existingCustomer != null)
            {
                existingCustomer.Name = customerInfo.Name;
                if (!string.IsNullOrEmpty(customerInfo.Email))
                {
                    existingCustomer.Email = customerInfo.Email;
                }

                await _context.SaveChangesAsync();
                return existingCustomer;
            }

            var newCustomer = new Customer
            {
                Name = customerInfo.Name,
                Phone = customerInfo.Phone,
                Email = customerInfo.Email,
                CreatedAt = DateTime.Now
            };
            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return newCustomer;
        }

        private string GenerateOrderCode()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random().Next(100, 999);
            return $"ORD{timestamp}{random}";
        }
    }
}