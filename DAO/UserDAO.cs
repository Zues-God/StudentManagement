using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Models;
using StudentManagement.db;

namespace StudentManagement.DAO
{
    public class UserDAO
    {
        private readonly AppDbContext _context;

        public UserDAO()
        {
            _context = new AppDbContext();
        }

        // Lấy tất cả người dùng
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Lấy người dùng theo ID
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Thêm người dùng
        public bool AddUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        // Cập nhật người dùng
        public bool UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null) return false;

            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.FullName = user.FullName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Phone = user.Phone;
            existingUser.Address = user.Address;
            existingUser.IsActive = user.IsActive;
            existingUser.UpdatedAt = DateTime.Now;

            return _context.SaveChanges() > 0;
        }

        // Xóa người dùng
        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }

        // Đăng nhập
        public User Login(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Tìm kiếm người dùng
        public List<User> SearchUsers(string keyword)
        {
            return _context.Users
                .Where(u => u.Username.Contains(keyword) ||
                            u.Email.Contains(keyword) ||
                            u.FullName.Contains(keyword))
                .ToList();
        }
    }
}
