using TFC.Models;
using TFC.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static TFC.DTOs.AdminDTO;
namespace TFC.Services
{
    public class AdminService : IAdminService
    {
        private readonly ModelContext _context; 
        private readonly ILogger<AdminService> _logger;

        public AdminService(ModelContext context, ILogger<AdminService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region User Management

        public async Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy danh sách tất cả users");

                var users = await _context.Set<User>()
                    .OrderByDescending(u => u.Id)
                    .ToListAsync();

                _logger.LogInformation("Lấy danh sách users thành công, tổng: {Count}", users.Count);
                return ServiceResult<IEnumerable<User>>.SuccessResult(users, "Lấy danh sách users thành công", users.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách users");
                return ServiceResult<IEnumerable<User>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách users", ex.Message);
            }
        }

        public async Task<ServiceResult<User>> GetUserByIdAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<User>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu tìm user với ID: {Id}", id);

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("Không tìm thấy user với ID: {Id}", id);
                    return ServiceResult<User>.ErrorResult("Không tìm thấy user");
                }

                _logger.LogInformation("Tìm thấy user với ID: {Id}", id);
                return ServiceResult<User>.SuccessResult(user, "Tìm user thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm user với ID: {Id}", id);
                return ServiceResult<User>.ErrorResult("Có lỗi xảy ra khi tìm user", ex.Message);
            }
        }

        public async Task<ServiceResult<User>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Bắt đầu tạo user mới với tên: {Name}", request.Name);

                // Check if username already exists
                var existingUser = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Username == request.Name.ToLower());

                if (existingUser != null)
                {
                    return ServiceResult<User>.ErrorResult("Tên người dùng đã tồn tại");
                }

                // Create new user
                var user = new User
                {
                    Name = request.Name,
                    Username = request.UserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = request.Role,
                    Active = request.Status == "active" ? 1 : 0
                };

                _context.Set<User>().Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Tạo user mới thành công với ID: {Id}", user.Id);
                return ServiceResult<User>.SuccessResult(user, "Tạo user thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo user mới");
                return ServiceResult<User>.ErrorResult("Có lỗi xảy ra khi tạo user", ex.Message);
            }
        }

        public async Task<ServiceResult<User>> UpdateUserAsync(decimal id, UpdateUserRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<User>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu cập nhật user với ID: {Id}", id);

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return ServiceResult<User>.ErrorResult("Không tìm thấy user");
                }

                user.Name = request.Name;
                user.Role = request.Role;
                user.Active = request.Status == "active" ? 1 : 0;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Cập nhật user thành công với ID: {Id}", id);
                return ServiceResult<User>.SuccessResult(user, "Cập nhật user thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật user với ID: {Id}", id);
                return ServiceResult<User>.ErrorResult("Có lỗi xảy ra khi cập nhật user", ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<bool>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu xóa user với ID: {Id}", id);

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return ServiceResult<bool>.ErrorResult("Không tìm thấy user");
                }

                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Xóa user thành công với ID: {Id}", id);
                return ServiceResult<bool>.SuccessResult(true, "Xóa user thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa user với ID: {Id}", id);
                return ServiceResult<bool>.ErrorResult("Có lỗi xảy ra khi xóa user", ex.Message);
            }
        }

        #endregion

        #region Product Management

        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy danh sách tất cả products");

                var products = await _context.Set<Product>()
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Inventory = p.Inventory,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                }).ToList();


                _logger.LogInformation("Lấy danh sách products thành công, tổng: {Count}", productDtos.Count);
                return ServiceResult<IEnumerable<ProductDto>>.SuccessResult(productDtos, "Lấy danh sách products thành công", productDtos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách products");
                return ServiceResult<IEnumerable<ProductDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách products", ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy danh sách tất cả categories");

                var categories = await _context.Set<Category>()
                    .OrderByDescending(c => c.Id)
                    .ToListAsync();

                _logger.LogInformation("Lấy danh sách categories thành công, tổng: {Count}", categories.Count);
                return ServiceResult<IEnumerable<Category>>.SuccessResult(categories, "Lấy danh sách categories thành công", categories.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách categories");
                return ServiceResult<IEnumerable<Category>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách categories", ex.Message);
            }
        }

        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<ProductDto>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu tìm product với ID: {Id}", id);

                var product = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    _logger.LogWarning("Không tìm thấy product với ID: {Id}", id);
                    return ServiceResult<ProductDto>.ErrorResult("Không tìm thấy product");
                }

                string? categoryName = null;
                if (product.CategoryId.HasValue)
                {
                    categoryName = await _context.Set<Category>()
                        .Where(c => c.Id == product.CategoryId.Value)
                        .Select(c => c.Name)
                        .FirstOrDefaultAsync();
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Inventory = product.Inventory,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryName
                };

                _logger.LogInformation("Tìm thấy product với ID: {Id}", id);
                return ServiceResult<ProductDto>.SuccessResult(productDto, "Tìm product thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm product với ID: {Id}", id);
                return ServiceResult<ProductDto>.ErrorResult("Có lỗi xảy ra khi tìm product", ex.Message);
            }
        }

        public async Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductRequest request)
        {
            try
            {
                _logger.LogInformation("Bắt đầu tạo product mới với tên: {Name}", request.Name);

                var existingProduct = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower());

                if (existingProduct != null)
                {
                    return ServiceResult<ProductDto>.ErrorResult("Tên sản phẩm đã tồn tại");
                }

                decimal? categoryId = null;

                if (request.CategoryID != null)
                {
                    var category = await _context.Set<Category>().FindAsync(request.CategoryID);
                    if (category == null)
                        return ServiceResult<ProductDto>.ErrorResult("Danh mục không tồn tại");

                    categoryId = category.Id;
                }

                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    ImageUrl = request.ImageUrl,
                    Inventory = request.StockQuantity,
                    CategoryId = categoryId
                };

                _context.Set<Product>().Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Tạo product mới thành công với ID: {Id}", product.Id);

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Inventory = product.Inventory,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryId != null
                        ? (await _context.Set<Category>().Where(c => c.Id == categoryId).Select(c => c.Name).FirstOrDefaultAsync())
                        : null
                };

                return ServiceResult<ProductDto>.SuccessResult(productDto, "Tạo product thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo product mới");
                return ServiceResult<ProductDto>.ErrorResult("Có lỗi xảy ra khi tạo product", ex.Message);
            }
        }


        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(decimal id, UpdateProductRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<ProductDto>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu cập nhật product với ID: {Id}", id);

                var product = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return ServiceResult<ProductDto>.ErrorResult("Không tìm thấy product");
                }

                var existingProduct = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower() && p.Id != id);

                if (existingProduct != null)
                {
                    return ServiceResult<ProductDto>.ErrorResult("Tên sản phẩm đã tồn tại");
                }

                decimal? categoryID = request.CategoryId;

                if (categoryID != null)
                {
                    var count = await _context.Set<Category>()
                    .CountAsync(c => c.Id == categoryID);
                    if (count == 0)
                        return ServiceResult<ProductDto>.ErrorResult("Danh mục không tồn tại");

                }


                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.ImageUrl = request.ImageUrl;
                product.Inventory = request.StockQuantity;
                product.CategoryId = categoryID;
                _logger.LogInformation("Product entity trước khi lưu: {@product}", product);
                await _context.SaveChangesAsync();

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Inventory = product.Inventory,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryID != null
                        ? await _context.Set<Category>()
                            .Where(c => c.Id == categoryID)
                            .Select(c => c.Name)
                            .FirstOrDefaultAsync()
                        : null
                };

                _logger.LogInformation("Cập nhật product thành công với ID: {Id}", id);
                return ServiceResult<ProductDto>.SuccessResult(productDto, "Cập nhật product thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật product với ID: {Id}", id);
                return ServiceResult<ProductDto>.ErrorResult("Có lỗi xảy ra khi cập nhật product", ex.Message);
            }
        }


        public async Task<ServiceResult<bool>> DeleteProductAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<bool>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu xóa product với ID: {Id}", id);

                var product = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return ServiceResult<bool>.ErrorResult("Không tìm thấy product");
                }

                _context.Set<Product>().Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Xóa product thành công với ID: {Id}", id);
                return ServiceResult<bool>.SuccessResult(true,"Xóa product thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa product với ID: {Id}", id);
                return ServiceResult<bool>.ErrorResult("Có lỗi xảy ra khi xóa product", ex.Message);
            }
        }

        #endregion

        #region Combo Management

        public async Task<ServiceResult<IEnumerable<ComboDto>>> GetAllCombosAsync()
        {
            try
            {
                var combos = await _context.Set<Combo>()
                    .Include(c => c.Comboitems)
                        .ThenInclude(ci => ci.Product) 
                    .OrderByDescending(c => c.Id)
                    .ToListAsync();

                var comboDtos = combos.Select(c => new ComboDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl,
                    Available = true,
                    ProductCount = c.Comboitems?.Count ?? 0,
                    Products = c.Comboitems?.Select(ci => new ComboProductDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.Name,
                        Quantity = (int)ci.Quantity,
                        Price = ci.Product?.Price ?? 0
                    }).ToList() ?? new List<ComboProductDto>()
                }).ToList();

                return ServiceResult<IEnumerable<ComboDto>>.SuccessResult(comboDtos, "Lấy danh sách combo thành công", comboDtos.Count);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<ComboDto>>.ErrorResult("Có lỗi xảy ra", ex.Message);
            }
        }

        public async Task<ServiceResult<ComboDetailDto>> GetComboByIdAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<ComboDetailDto>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu tìm Combo với ID: {Id}", id);

                var combo = await _context.Set<Combo>()
                    .Include(c => c.Comboitems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (combo == null)
                {
                    _logger.LogWarning("Không tìm thấy Combo với ID: {Id}", id);
                    return ServiceResult<ComboDetailDto>.ErrorResult("Không tìm thấy Combo");
                }

                var comboDetail = new ComboDetailDto
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Description = combo.Description,
                    Price = combo.Price,
                    ImageUrl = combo.ImageUrl,
                    Available = true,
                    ProductCount = combo.Comboitems?.Count ?? 0,
                    Products = combo.Comboitems?.Select(ci => new ComboProductDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.Name,
                        Quantity = (int)ci.Quantity,
                        Price = ci.Product?.Price ?? 0,
                        CategoryName = ci.Product?.Category?.Name,
                        ProductImageUrl = ci.Product?.ImageUrl
                    }).ToList() ?? new List<ComboProductDto>()
                };

                _logger.LogInformation("Tìm thấy Combo với ID: {Id}", id);
                return ServiceResult<ComboDetailDto>.SuccessResult(comboDetail, "Tìm Combo thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm Combo với ID: {Id}", id);
                return ServiceResult<ComboDetailDto>.ErrorResult("Có lỗi xảy ra khi tìm Combo", ex.Message);
            }
        }


        public async Task<ServiceResult<ComboDto>> CreateComboAsync(CreateComboRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Bắt đầu tạo Combo mới với tên: {Name}", request.Name);

                var existingCombo = await _context.Set<Combo>()
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower());
                if (existingCombo != null)
                    return ServiceResult<ComboDto>.ErrorResult("Tên combo đã tồn tại");
                
                var combo = new Combo
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    ImageUrl = request.ImageUrl,
                    Available = request.Status == "active",
                };

                _context.Set<Combo>().Add(combo);
                await _context.SaveChangesAsync();

                var comboProducts = new List<Comboitem>();

                var productIds = request.Products.Select(p => p.ProductId).ToList();
                var products = await _context.Set<Product>()
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                var missingProductIds = productIds.Except(products.Select(p => p.Id)).ToList();
                if (missingProductIds.Any())
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<ComboDto>.ErrorResult($"Sản phẩm với ID {string.Join(", ", missingProductIds)} không tồn tại");
                }

                foreach (var productRequest in request.Products)
                {
                    comboProducts.Add(new Comboitem
                    {
                        ComboId = combo.Id,
                        ProductId = productRequest.ProductId,
                        Quantity = productRequest.Quantity
                    });
                }

                _context.Set<Comboitem>().AddRange(comboProducts);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Tạo Combo mới thành công với ID: {Id}", combo.Id);

                var comboDto = new ComboDto
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Description = combo.Description,
                    Price = combo.Price,
                    ImageUrl = combo.ImageUrl,
                    Available = combo.Available ?? false,
                    ProductCount = comboProducts.Count,
                    Products = comboProducts.Select(ci =>
                    {
                        var product = products.First(p => p.Id == ci.ProductId);
                        return new ComboProductDto
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Price = product.Price,
                            Quantity = (int)(ci.Quantity ?? 1),
                            CategoryName = product.Category?.Name,
                            ProductImageUrl = product.ImageUrl
                        };
                    }).ToList()
                };

                return ServiceResult<ComboDto>.SuccessResult(comboDto, "Tạo Combo thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi tạo Combo mới");
                return ServiceResult<ComboDto>.ErrorResult("Có lỗi xảy ra khi tạo Combo", ex.Message);
            }
        }


        public async Task<ServiceResult<ComboDto>> UpdateComboAsync(decimal id, UpdateComboRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (id <= 0)
                    return ServiceResult<ComboDto>.ErrorResult("ID không hợp lệ");

                _logger.LogInformation("Bắt đầu cập nhật Combo với ID: {Id}", id);

                var combo = await _context.Set<Combo>().FirstOrDefaultAsync(p => p.Id == id);
                if (combo == null)
                    return ServiceResult<ComboDto>.ErrorResult("Không tìm thấy Combo");

                var existingCombo = await _context.Set<Combo>()
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower() && p.Id != id);
                if (existingCombo != null)
                    return ServiceResult<ComboDto>.ErrorResult("Tên combo đã tồn tại");

                combo.Name = request.Name;
                combo.Description = request.Description;
                combo.Price = request.Price;
                combo.ImageUrl = request.ImageUrl;
                combo.Available = request.Status == "active";

                var existingItems = await _context.Set<Comboitem>().Where(ci => ci.ComboId == id).ToListAsync();
                _context.Set<Comboitem>().RemoveRange(existingItems);

                var productIds = request.Products.Select(p => p.ProductId).ToList();
                var products = await _context.Set<Product>()
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                var missingIds = productIds.Except(products.Select(p => p.Id)).ToList();
                if (missingIds.Any())
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<ComboDto>.ErrorResult($"Sản phẩm với ID {string.Join(", ", missingIds)} không tồn tại");
                }

                var newItems = request.Products.Select(p => new Comboitem
                {
                    ComboId = combo.Id,
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList();

                _context.Set<Comboitem>().AddRange(newItems);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var comboDto = new ComboDto
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Description = combo.Description,
                    Price = combo.Price,
                    ImageUrl = combo.ImageUrl,
                    Available = combo.Available ?? false,
                    ProductCount = newItems.Count,
                    Products = newItems.Select(ci =>
                    {
                        var product = products.First(p => p.Id == ci.ProductId);
                        return new ComboProductDto
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Price = product.Price,
                            Quantity = (int)(ci.Quantity ?? 1),
                            CategoryName = product.Category?.Name,
                            ProductImageUrl = product.ImageUrl
                        };
                    }).ToList()
                };

                return ServiceResult<ComboDto>.SuccessResult(comboDto, "Cập nhật Combo thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi cập nhật Combo với ID: {Id}", id);
                return ServiceResult<ComboDto>.ErrorResult("Có lỗi xảy ra khi cập nhật Combo", ex.Message);
            }
        }


        public async Task<ServiceResult<bool>> DeleteComboAsync(decimal id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<bool>.ErrorResult("ID không hợp lệ");
                }

                _logger.LogInformation("Bắt đầu xóa Combo với ID: {Id}", id);

                var combo = await _context.Set<Combo>()
                    .Include(c => c.Comboitems) 
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (combo == null)
                {
                    return ServiceResult<bool>.ErrorResult("Không tìm thấy Combo");
                }

                if (combo.Comboitems?.Any() == true)
                {
                    _context.Set<Comboitem>().RemoveRange(combo.Comboitems);
                }

                _context.Set<Combo>().Remove(combo);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Xóa Combo thành công với ID: {Id}", id);
                return ServiceResult<bool>.SuccessResult(true, "Xóa Combo thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa Combo với ID: {Id}", id);
                return ServiceResult<bool>.ErrorResult("Có lỗi xảy ra khi xóa Combo", ex.Message);
            }
        }

        #endregion
    }
}