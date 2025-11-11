using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class ScheduleService
    {
        private readonly DatabaseService _dbService;

        public ScheduleService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<Schedule>> GetAllSchedulesAsync()
        {
            using var context = _dbService.CreateContext();
            
            var schedules = await context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Slot)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.Slot!.StartTime)
                .ToListAsync();
            
            return schedules.Select(s => new Schedule
            {
                Id = s.Id,
                CourseId = s.CourseId,
                CourseName = s.Course?.Name,
                DayOfWeek = s.DayOfWeek,
                SlotId = s.SlotId,
                SlotName = s.Slot?.Name,
                Room = s.Room,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();
        }

        public async Task<List<Schedule>> GetSchedulesByUserAsync(int userId, string role)
        {
            using var context = _dbService.CreateContext();
            
            IQueryable<Data.Entities.Schedule> query;
            
            if (role == "lecture")
            {
                query = context.Schedules
                    .Include(s => s.Course)
                    .Include(s => s.Slot)
                    .Where(s => s.Course!.InstructorId == userId);
            }
            else // student
            {
                query = context.Schedules
                    .Include(s => s.Course!)
                        .ThenInclude(c => c.CourseParticipants)
                    .Include(s => s.Slot)
                    .Where(s => s.Course!.CourseParticipants!.Any(cp => cp.UserId == userId));
            }
            
            var schedules = await query
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.Slot!.StartTime)
                .ToListAsync();
            
            return schedules.Select(s => new Schedule
            {
                Id = s.Id,
                CourseId = s.CourseId,
                CourseName = s.Course?.Name,
                DayOfWeek = s.DayOfWeek,
                SlotId = s.SlotId,
                SlotName = s.Slot?.Name,
                Room = s.Room,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();
        }

        public async Task<bool> CreateScheduleAsync(Schedule schedule)
        {
            using var context = _dbService.CreateContext();
            
            var entity = new Data.Entities.Schedule
            {
                CourseId = schedule.CourseId,
                DayOfWeek = schedule.DayOfWeek,
                SlotId = schedule.SlotId,
                Room = schedule.Room,
                Notes = schedule.Notes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.Schedules.Add(entity);
            
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

        public async Task<bool> UpdateScheduleAsync(Schedule schedule)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Schedules.FindAsync(schedule.Id);
            if (entity == null) return false;

            entity.CourseId = schedule.CourseId;
            entity.DayOfWeek = schedule.DayOfWeek;
            entity.SlotId = schedule.SlotId;
            entity.Room = schedule.Room;
            entity.Notes = schedule.Notes;
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

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Schedules.FindAsync(id);
            if (entity == null) return false;

            context.Schedules.Remove(entity);
            
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
