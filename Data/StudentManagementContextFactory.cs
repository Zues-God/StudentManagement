using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace StudentManagement.Data
{
    public class StudentManagementContextFactory : IDesignTimeDbContextFactory<StudentManagementContext>
    {
        public StudentManagementContext CreateDbContext(string[] args)
        {
            // Try multiple paths to find appsettings.json
            var paths = new[]
            {
                Directory.GetCurrentDirectory(),
                Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."),
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ""
            };

            string? basePath = null;
            foreach (var path in paths)
            {
                if (File.Exists(Path.Combine(path, "appsettings.json")))
                {
                    basePath = path;
                    break;
                }
            }

            // If not found, use current directory
            basePath ??= Directory.GetCurrentDirectory();

            // Build configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=StudentManagement;User Id=sa;Password=123456;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<StudentManagementContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new StudentManagementContext(optionsBuilder.Options);
        }
    }
}

