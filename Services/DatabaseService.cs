using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagement.Data;

namespace StudentManagement.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            // Try to load from appsettings.json, fallback to hardcoded value
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Server=localhost,1433;Database=StudentManagement;User Id=sa;Password=Admin12345#;TrustServerCertificate=True;";
        }

        public StudentManagementContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentManagementContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new StudentManagementContext(optionsBuilder.Options);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var context = CreateContext();
                await context.Database.CanConnectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
