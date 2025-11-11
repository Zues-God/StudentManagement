using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Data.Entities;
using StudentManagement.Models;
using System.Security.Cryptography;
using System.Text;
using UserEntity = StudentManagement.Data.Entities.User;
using UserModel = StudentManagement.Models.User;

namespace StudentManagement.Services
{
    public class AuthService
    {
        private readonly DatabaseService _dbService;

        public AuthService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<UserModel?> LoginAsync(string username, string password)
        {
            var passwordHash = HashPassword(password);
            
            using var context = _dbService.CreateContext();
            
            var user = await context.Users
                .Where(u => u.Username == username && u.PasswordHash == passwordHash)
                .FirstOrDefaultAsync();
            
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string role, string? fullName = null)
        {
            var passwordHash = HashPassword(password);
            
            using var context = _dbService.CreateContext();
            
            // Check if username or email already exists
            var exists = await context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
            
            if (exists) return false;

            var user = new UserEntity
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                FullName = fullName,
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            
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

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
