using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class CourseService
    {
        private readonly DatabaseService _dbService;

        public CourseService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            using var context = _dbService.CreateContext();
            
            var courses = await context.Courses
                .Include(c => c.Instructor)
                .OrderBy(c => c.Code)
                .ToListAsync();
            
            return courses.Select(c => new Course
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                Credits = c.Credits,
                InstructorId = c.InstructorId,
                InstructorName = c.Instructor?.FullName,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            using var context = _dbService.CreateContext();
            
            var entity = new Data.Entities.Course
            {
                Code = course.Code,
                Name = course.Name,
                Description = course.Description,
                Credits = course.Credits,
                InstructorId = course.InstructorId,
                CreatedAt = DateTime.Now
            };

            context.Courses.Add(entity);
            
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

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Courses.FindAsync(course.Id);
            if (entity == null) return false;

            entity.Code = course.Code;
            entity.Name = course.Name;
            entity.Description = course.Description;
            entity.Credits = course.Credits;
            entity.InstructorId = course.InstructorId;
            entity.UpdatedAt = DateTime.Now;

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

        public async Task<bool> DeleteCourseAsync(int id)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Courses.FindAsync(id);
            if (entity == null) return false;

            context.Courses.Remove(entity);

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

        public async Task<bool> AssignInstructorAsync(int courseId, int? instructorId)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Courses.FindAsync(courseId);
            if (entity == null) return false;

            entity.InstructorId = instructorId;
            entity.UpdatedAt = DateTime.Now;

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
