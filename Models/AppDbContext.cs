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
            // Bỏ toàn bộ .HasColumnName() => EF tự map đúng theo cột PascalCase
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<CourseParticipant>().ToTable("CourseParticipants");
            modelBuilder.Entity<Schedule>().ToTable("Schedules");
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<Slot>().ToTable("Slots");
        }
    }
}
