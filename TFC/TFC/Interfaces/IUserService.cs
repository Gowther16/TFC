using TFC.Models;

namespace TFC.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(decimal id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(User user);
    }
}
