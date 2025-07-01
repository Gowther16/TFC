using Microsoft.AspNetCore.Mvc.Rendering;

namespace TFC.DTOs
{
    public class AdminDTO
    {
        // ViewModels cho Admin Controller

        // Dashboard ViewModels
        public class DashboardViewModel
        {
            public int TotalUsers { get; set; }
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }

            public int TodayOrders { get; set; }
            public decimal TodayRevenue { get; set; }

            public int MonthlyOrders { get; set; }
            public decimal MonthlyRevenue { get; set; }

            public List<RecentOrderViewModel> RecentOrders { get; set; } = new();
            public List<NewUserViewModel> NewUsers { get; set; } = new();
            public List<ChartDataViewModel> ChartData { get; set; } = new();
        }

        public class RecentOrderViewModel
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalAmount { get; set; }
            public OrderStatus Status { get; set; }
            public DateTime CreatedDate { get; set; }

            public string StatusDisplay => Status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Completed => "Hoàn thành",
                _ => "Không xác định"
            };

            public string StatusClass => Status switch
            {
                OrderStatus.Pending => "warning",
                OrderStatus.Confirmed => "info",
                OrderStatus.Completed => "success",
                _ => "secondary"
            };
        }
        public class ProductDto
        {
            public decimal Id { get; set; }
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public decimal? Inventory { get; set; }
            public decimal? CategoryId { get; set; }
            public string? CategoryName { get; set; }
        }
        public class NewUserViewModel
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class ChartDataViewModel
        {
            public DateTime Date { get; set; }
            public decimal Revenue { get; set; }
            public string DateLabel => Date.ToString("dd/MM");
        }

        // User Management ViewModels
        public class UsersListViewModel
        {
            public List<UserManagementViewModel> Users { get; set; } = new();
            public string SearchTerm { get; set; } = "";
            public int CurrentPage { get; set; } = 1;
            public int TotalPages { get; set; }
            public int TotalUsers { get; set; }

            public bool HasPreviousPage => CurrentPage > 1;
            public bool HasNextPage => CurrentPage < TotalPages;
        }

        public class UserManagementViewModel
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastLoginDate { get; set; }

            public string StatusDisplay => IsActive ? "Hoạt động" : "Bị khóa";
            public string StatusClass => IsActive ? "success" : "danger";
        }

        public class UserDetailsViewModel
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastLoginDate { get; set; }
            public List<string> Roles { get; set; } = new();
            public int TotalOrders { get; set; }
            public decimal TotalSpent { get; set; }

            public string RolesDisplay => string.Join(", ", Roles);
        }

        // Order Management ViewModels
        public class OrdersListViewModel
        {
            public List<OrderManagementViewModel> Orders { get; set; } = new();
            public string SearchTerm { get; set; } = "";
            public OrderStatus? SelectedStatus { get; set; }
            public int CurrentPage { get; set; } = 1;
            public int TotalPages { get; set; }
            public int TotalOrders { get; set; }

            public bool HasPreviousPage => CurrentPage > 1;
            public bool HasNextPage => CurrentPage < TotalPages;

            public List<SelectListItem> StatusOptions => new()
            {
                new SelectListItem { Value = "", Text = "Tất cả trạng thái" },
                new SelectListItem { Value = OrderStatus.Pending.ToString(), Text = "Chờ xử lý" },
                new SelectListItem { Value = OrderStatus.Confirmed.ToString(), Text = "Đã xác nhận" },
                new SelectListItem { Value = OrderStatus.Completed.ToString(), Text = "Hoàn thành" },
            };
        }

        public class OrderManagementViewModel
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public decimal TotalAmount { get; set; }
            public OrderStatus Status { get; set; }
            public int ItemCount { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }

            public string StatusDisplay => Status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Completed => "Hoàn thành",
                _ => "Không xác định"
            };

            public string StatusClass => Status switch
            {
                OrderStatus.Pending => "warning",
                OrderStatus.Confirmed => "info",
                OrderStatus.Completed => "success",
                _ => "secondary"
            };
        }

        public class OrderDetailsViewModel
        {
            public int Id { get; set; }
            public CustomerInfoViewModel Customer { get; set; }
            public List<OrderItemViewModel> Items { get; set; } = new();
            public OrderStatus Status { get; set; }
            public string PaymentMethod { get; set; }
            public string ShippingAddress { get; set; }
            public string Notes { get; set; }
            public decimal SubTotal { get; set; }
            public decimal ShippingFee { get; set; }
            public decimal TotalAmount { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }

            public string StatusDisplay => Status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Completed => "Hoàn thành",
                _ => "Không xác định"
            };

            public List<SelectListItem> StatusOptions => new()
            {
                new SelectListItem { Value = OrderStatus.Pending.ToString(), Text = "Chờ xử lý" },
                new SelectListItem { Value = OrderStatus.Pending.ToString(), Text = "Đã xác nhận" },
                new SelectListItem { Value = OrderStatus.Completed.ToString(), Text = "Hoàn thành" },
            };
        }

        public class CustomerInfoViewModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
        }

        public class OrderItemViewModel
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }

        // Reports ViewModels
        public class ReportsViewModel
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
            public int CompletedOrders { get; set; }
            public int CancelledOrders { get; set; }
            public int NewCustomers { get; set; }

            public List<TopProductViewModel> TopProducts { get; set; } = new();
            public List<DailyRevenueViewModel> DailyRevenue { get; set; } = new();

            public decimal CompletionRate => TotalOrders > 0 ? (decimal)CompletedOrders / TotalOrders * 100 : 0;
            public decimal CancellationRate => TotalOrders > 0 ? (decimal)CancelledOrders / TotalOrders * 100 : 0;
            public decimal AverageOrderValue => CompletedOrders > 0 ? TotalRevenue / CompletedOrders : 0;
        }

        public class TopProductViewModel
        {
            public string ProductName { get; set; }
            public int TotalSold { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        public class DailyRevenueViewModel
        {
            public DateTime Date { get; set; }
            public decimal Revenue { get; set; }
            public int OrderCount { get; set; }

            public string DateLabel => Date.ToString("dd/MM");
        }

        public enum OrderStatus
        {
            Pending = 0,       // Chờ xử lý
            Confirmed = 1,    // Đã xác nhận 
            Completed = 2    // Hoàn thành
        }
    }
}
