using Microsoft.EntityFrameworkCore;
using TFC.Interfaces;
using TFC.Models;

namespace TFC.Services
{
    public class UserService : IUserService
    {
        private readonly ModelContext _context;

        public UserService(ModelContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(decimal id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null) return false;
            return user.PasswordHash == password;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Where(u => u.Active.Equals(1)).ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}