using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TFC.Models;
using TFC.Services;
using System.ComponentModel.DataAnnotations;
using TFC.Interfaces;

namespace TFC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext _context;
        private readonly IUserService _userService;

        public AccountController(ModelContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // ViewModel for Login
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByUsernameAsync(model.Username);
                if (!user.Active.Equals(1))
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị vô hiệu hóa, hãy liên hệ admin để có thể kích hoạt.");

                }
                else if (user != null && user.Active.Equals(1) && VerifyPassword(model.Password, user.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim("Username", user.Username),
                        new Claim(ClaimTypes.Role, user.Role ?? "user")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(24)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["InfoMessage"] = "Bạn đã đăng xuất thành công.";
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var userId = decimal.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var model = new ProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Role = user.Role
            };

            return View(model);
        }

        // POST: Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                var userId = decimal.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound();

                user.Name = model.Name;

                if (await _userService.UpdateUserAsync(user))
                {
                    // Update claims
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim("Username", user.Username),
                        new Claim(ClaimTypes.Role, user.Role ?? "user")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể cập nhật thông tin. Vui lòng thử lại.");
                }
            }

            return View(model);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                var userId = decimal.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound();

                if (!VerifyPassword(model.CurrentPassword, user.PasswordHash))
                {
                    ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                    return View(model);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                if (await _userService.UpdateUserAsync(user))
                {
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể đổi mật khẩu. Vui lòng thử lại.");
                }
            }

            return View(model);
        }
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }

    // ViewModel cho Profile
    public class ProfileViewModel
    {
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Vai trò")]
        public string? Role { get; set; }
    }

    // ViewModel cho Change Password
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}