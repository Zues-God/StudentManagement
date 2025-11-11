using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using UserModel = StudentManagement.Models.User;

namespace StudentManagement.Services
{
    public class UserService
    {
        private readonly DatabaseService _dbService;

        public UserService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            using var context = _dbService.CreateContext();
            
            var users = await context.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Username)
                .ToListAsync();
            
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                FullName = u.FullName,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }

        public async Task<List<UserModel>> GetUsersByRoleAsync(string role)
        {
            using var context = _dbService.CreateContext();
            
            var users = await context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Username)
                .ToListAsync();
            
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                FullName = u.FullName,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string role)
        {
            using var context = _dbService.CreateContext();
            
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Role = role;
            user.UpdatedAt = DateTime.Now;

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            using var context = _dbService.CreateContext();
            
            var user = await context.Users.FindAsync(id);
            if (user == null) return false;

            context.Users.Remove(user);
            
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
