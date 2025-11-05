using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Models;

namespace StudentManagement.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<CourseParticipant> CourseParticipants { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Slot> Slots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Chuỗi kết nối tới SQL Server
            optionsBuilder.UseSqlServer(@"Server=ADMIN\SQL2022;Database=school_management;Trusted_Connection=True;TrustServerCertificate=True;uid=sa;password=123456;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map cột trong SQL với thuộc tính C#
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
        }
    }
}
