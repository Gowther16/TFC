using Microsoft.EntityFrameworkCore;
using TFC.DTOs;
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

        // Thêm phương thức mới để tìm đơn hàng theo số điện thoại
        public async Task<GetOrdersByPhoneResult> GetOrdersByPhoneAsync(string phoneNumber)
        {
            try
            {
                // Tìm customer theo số điện thoại
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Phone == phoneNumber);

                if (customer == null)
                {
                    return new GetOrdersByPhoneResult
                    {
                        Success = false,
                        Message = "Không tìm thấy khách hàng với số điện thoại này"
                    };
                }

                // Lấy danh sách đơn hàng của customer
                var orders = await _context.Orders
                    .Where(o => o.CustomerId == customer.Id)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.Orderitems)
                        .ThenInclude(oi => oi.Combo)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

                if (!orders.Any())
                {
                    return new GetOrdersByPhoneResult
                    {
                        Success = false,
                        Message = "Không tìm thấy đơn hàng nào"
                    };
                }

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
                        UnitPrice = item.UnitPrice,
                        ProductName = item.Product?.Name,
                        ComboName = item.Combo?.Name
                    }).ToList()
                }).ToList();

                return new GetOrdersByPhoneResult
                {
                    Success = true,
                    Orders = FindOrders,
                    CustomerName = customer.Name
                };
            }
            catch (Exception ex)
            {
                return new GetOrdersByPhoneResult
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tìm kiếm đơn hàng"
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