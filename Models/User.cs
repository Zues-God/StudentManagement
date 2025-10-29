using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class User
    {
        public User()
        {
        }
        public User(string username, string password, string email, string role, string fullName,
                    DateTime? dateOfBirth = null, string? phone = null, string? address = null, bool? isActive = true)
        {
            Username = username;
            Password = password;
            Email = email;
            Role = role;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Phone = phone;
            Address = address;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }                      // Primary Key
        public string Username { get; set; }             // Tên đăng nhập
        public string Password { get; set; }             // Mật khẩu (đã mã hóa)
        public string Email { get; set; }                // Email người dùng
        public string Role { get; set; }                 // Vai trò (admin, user,...)
        public string FullName { get; set; }             // Họ và tên
        [AllowNull]
        public DateTime? DateOfBirth { get; set; }       // Ngày sinh (có thể null)
        [AllowNull]
        public string Phone { get; set; }                // Số điện thoại (có thể null)
        [AllowNull]
        public string Address { get; set; }              // Địa chỉ (có thể null)
        public bool? IsActive { get; set; }              // Trạng thái hoạt động (true/false)
        public DateTime? CreatedAt { get; set; }         // Thời gian tạo
        public DateTime? UpdatedAt { get; set; }         // Thời gian cập nhật
    }
}
