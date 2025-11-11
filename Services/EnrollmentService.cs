using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class EnrollmentService
    {
        private readonly DatabaseService _dbService;

        public EnrollmentService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<CourseParticipant>> GetAllEnrollmentsAsync()
        {
            using var context = _dbService.CreateContext();
            
            var enrollments = await context.CourseParticipants
                .Include(cp => cp.Course)
                .Include(cp => cp.User)
                .OrderBy(cp => cp.Course!.Name)
                .ThenBy(cp => cp.User!.FullName)
                .ToListAsync();
            
            return enrollments.Select(cp => new CourseParticipant
            {
                Id = cp.Id,
                CourseId = cp.CourseId,
                CourseName = cp.Course?.Name,
                UserId = cp.UserId,
                StudentName = cp.User?.FullName,
                StudentEmail = cp.User?.Email,
                EnrolledAt = cp.EnrolledAt,
                UpdatedAt = cp.UpdatedAt
            }).ToList();
        }

        public async Task<bool> CreateEnrollmentAsync(int courseId, int userId)
        {
            using var context = _dbService.CreateContext();
            
            // Check if enrollment already exists
            var exists = await context.CourseParticipants
                .AnyAsync(cp => cp.CourseId == courseId && cp.UserId == userId);
            
            if (exists) return false;

            var enrollment = new Data.Entities.CourseParticipant
            {
                CourseId = courseId,
                UserId = userId,
                EnrolledAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.CourseParticipants.Add(enrollment);
            
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

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.CourseParticipants.FindAsync(id);
            if (entity == null) return false;

            context.CourseParticipants.Remove(entity);
            
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
