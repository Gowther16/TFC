using Microsoft.EntityFrameworkCore;
using TFC.DTOs;
using TFC.Interfaces;
using TFC.Models;
using static TFC.DTOs.FindOrderDTO;

namespace TFC.Services
{
    public class OrderService : IOrderService
    {
        private readonly ModelContext _context;

        public OrderService(ModelContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var customer = await FindOrCreateCustomerAsync(request.Customer);

                var orderCode = GenerateOrderCode();
                var totalAmount = request.Items.Sum(item => item.Price * item.Quantity);

                var order = new Order
                {
                    OrderCode = orderCode,
                    CustomerId = customer.Id,
                    TotalAmount = totalAmount,
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
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                        if (product == null)
                            throw new Exception($"Sản phẩm với ID {item.Id} không tồn tại");

                        orderItem.ProductId = productId;

                        if (product.Inventory.HasValue)
                        {
                            product.Inventory -= item.Quantity;
                            if (product.Inventory < 0) product.Inventory = 0;
                        }
                    }
                    else if (item.Type.ToLower() == "combo")
                    {
                        var comboId = decimal.Parse(item.Id);
                        var combo = await _context.Combos
                            .Include(c => c.Comboitems)
                            .ThenInclude(ci => ci.Product)
                            .FirstOrDefaultAsync(c => c.Id == comboId);

                        if (combo == null)
                            throw new Exception($"Combo với ID {item.Id} không tồn tại");

                        orderItem.ComboId = comboId;

                        foreach (var comboItem in combo.Comboitems)
                        {
                            var product = comboItem.Product;
                            if (product != null && product.Inventory.HasValue)
                            {
                                product.Inventory -= comboItem.Quantity * item.Quantity;
                                if (product.Inventory < 0) product.Inventory = 0;
                            }
                        }
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

        public async Task<GetOrdersByOrderCodeResult> GetOrdersByOrderCodeAsync(string OrderCode)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.OrderCode == OrderCode)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

                if (!orders.Any())
                {
                    return new GetOrdersByOrderCodeResult
                    {
                        Success = false,
                        Message = "Không tìm thấy đơn hàng nào"
                    };
                }

                var customer = await _context.Customers
                    .Where(c => c.Id == orders.First().CustomerId)
                    .FirstOrDefaultAsync();

                var FindOrders = orders.Select(order => new FindOrder
                {
                    Id = order.Id,
                    OrderCode = order.OrderCode,
                    TotalAmount = (decimal)order.TotalAmount,
                    Status = order.Status,
                    CreatedAt = (DateTime)order.CreatedAt,
                    Items = order.Orderitems.Select(item => new OrderItemDTO
                    {
                        Quantity = (decimal)item.Quantity,
                        UnitPrice = (decimal)item.UnitPrice,
                        ProductName = item.Product?.Name,
                        ComboName = item.Combo?.Name
                    }).ToList()
                }).ToList();
                return new GetOrdersByOrderCodeResult
                {
                    Success = true,
                    Orders = FindOrders,
                    CustomerName = customer.Name
                };
            }
            catch (Exception ex)
            {
                return new GetOrdersByOrderCodeResult
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tìm kiếm đơn hàng"
                };
            }
        }

        private async Task<Customer> FindOrCreateCustomerAsync(CustomerInfo customerInfo)
        {
            var existingCustomer = await _context.Customers
                .Where(c => c.Name == customerInfo.Name && c.Email == customerInfo.Email)
                .FirstOrDefaultAsync();

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
                Email = customerInfo.Email,
                CreatedAt = DateTime.Now
            };
            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return newCustomer;
        }

        private string GenerateOrderCode()
        {
            var timestamp = DateTime.Now.ToString("dd");
            var random = new Random().Next(100, 999);
            return $"ORD-{timestamp}{random}";
        }
    }
}