using Microsoft.Data.SqlClient; // Hoặc System.Data.SqlClient nếu có
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using StudentManagement.db;

namespace StudentManagement.DAO
{
    public class UserDAO
    {
        private readonly DBContext _db;

        public UserDAO()
        {
            _db = new DBContext();
        }

        // Lấy tất cả người dùng
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM Users";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["id"],
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            Role = reader["role"].ToString(),
                            FullName = reader["full_name"].ToString(),
                            DateOfBirth = reader["date_of_birth"] == DBNull.Value ? null : (DateTime?)reader["date_of_birth"],
                            Phone = reader["phone"].ToString(),
                            Address = reader["address"].ToString(),
                            IsActive = reader["is_active"] == DBNull.Value ? null : (bool?)reader["is_active"],
                            CreatedAt = reader["created_at"] == DBNull.Value ? null : (DateTime?)reader["created_at"],
                            UpdatedAt = reader["updated_at"] == DBNull.Value ? null : (DateTime?)reader["updated_at"]
                        });
                    }
                }
            }
            return users;
        }

        // Lấy người dùng theo id
        public User GetUserById(int id)
        {
            User user = null;
            string query = "SELECT * FROM Users WHERE id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = (int)reader["id"],
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            Role = reader["role"].ToString(),
                            FullName = reader["full_name"].ToString(),
                            DateOfBirth = reader["date_of_birth"] == DBNull.Value ? null : (DateTime?)reader["date_of_birth"],
                            Phone = reader["phone"].ToString(),
                            Address = reader["address"].ToString(),
                            IsActive = reader["is_active"] == DBNull.Value ? null : (bool?)reader["is_active"],
                            CreatedAt = reader["created_at"] == DBNull.Value ? null : (DateTime?)reader["created_at"],
                            UpdatedAt = reader["updated_at"] == DBNull.Value ? null : (DateTime?)reader["updated_at"]
                        };
                    }
                }
            }

            return user;
        }

        // Thêm người dùng mới
        public bool AddUser(User user)
        {
            string query = @"INSERT INTO Users (username, password, email, role, full_name, date_of_birth, phone, address, is_active, created_at, updated_at)
                             VALUES (@Username, @Password, @Email, @Role, @FullName, @DateOfBirth, @Phone, @Address, @IsActive, @CreatedAt, @UpdatedAt)";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@DateOfBirth", (object?)user.DateOfBirth ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object?)user.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object?)user.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", (object?)user.IsActive ?? true);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                int rows = cmd.ExecuteNonQuery();
        
                return rows > 0;
            }
        }

        // Cập nhật người dùng
        public bool UpdateUser(User user)
        {
            string query = @"UPDATE Users SET username=@Username, password=@Password, email=@Email, role=@Role, full_name=@FullName,
                             date_of_birth=@DateOfBirth, phone=@Phone, address=@Address, is_active=@IsActive, updated_at=@UpdatedAt
                             WHERE id=@Id";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@DateOfBirth", (object?)user.DateOfBirth ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object?)user.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object?)user.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", (object?)user.IsActive ?? true);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                int rows = cmd.ExecuteNonQuery();
        
                return rows > 0;
            }
        }
        // Xóa người dùng
        public bool DeleteUser(int id)
        {
            string query = "DELETE FROM Users WHERE id=@Id";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                int rows = cmd.ExecuteNonQuery();
 
                return rows > 0;
            }
        }
        public User Login(string username, string password)
        {
            User user = null;
            string query = "SELECT * FROM Users WHERE username = @Username AND password = @Password";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Nếu bạn dùng hash thì truyền hash

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = (int)reader["id"],
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            Role = reader["role"].ToString(),
                            FullName = reader["full_Name"].ToString(),
                            DateOfBirth = reader["date_of_birth"] == DBNull.Value ? null : (DateTime?)reader["date_of_birth"],
                            Phone = reader["phone"].ToString(),
                            Address = reader["address"].ToString(),
                            IsActive = reader["is_active"] == DBNull.Value ? null : (bool?)reader["is_active"],
                            CreatedAt = reader["created_at"] == DBNull.Value ? null : (DateTime?)reader["created_at"],
                            UpdatedAt = reader["updated_at"] == DBNull.Value ? null : (DateTime?)reader["updated_at"]
                        };
                    }
                }
            }
     
            return user;
        }
        // Tìm kiếm người dùng theo từ khóa
        public List<User> SearchUsers(string keyword)
        {
            List<User> users = new List<User>();
            string query = @"SELECT * FROM Users 
                     WHERE username LIKE @Keyword 
                        OR email LIKE @Keyword 
                        OR full_name LIKE @Keyword";

            using (SqlCommand cmd = new SqlCommand(query, _db.Connection))
            {
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%"); // Tìm chứa từ khóa

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["id"],
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            Role = reader["role"].ToString(),
                            FullName = reader["full_name"].ToString(),
                            DateOfBirth = reader["date_of_birth"] == DBNull.Value ? null : (DateTime?)reader["date_of_birth"],
                            Phone = reader["phone"].ToString(),
                            Address = reader["address"].ToString(),
                            IsActive = reader["is_active"] == DBNull.Value ? null : (bool?)reader["is_active"],
                            CreatedAt = reader["created_at"] == DBNull.Value ? null : (DateTime?)reader["created_at"],
                            UpdatedAt = reader["updated_at"] == DBNull.Value ? null : (DateTime?)reader["updated_at"]
                        });
                    }
                }
            }

            return users;
        }


    }
}

