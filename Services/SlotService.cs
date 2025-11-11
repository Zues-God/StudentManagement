using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class SlotService
    {
        private readonly DatabaseService _dbService;

        public SlotService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<Slot>> GetAllSlotsAsync()
        {
            using var context = _dbService.CreateContext();
            
            var slots = await context.Slots
                .OrderBy(s => s.StartTime)
                .ToListAsync();
            
            return slots.Select(s => new Slot
            {
                Id = s.Id,
                Name = s.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList();
        }

        public async Task<bool> CreateSlotAsync(Slot slot)
        {
            using var context = _dbService.CreateContext();
            
            var entity = new Data.Entities.Slot
            {
                Name = slot.Name,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime
            };

            context.Slots.Add(entity);

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

        public async Task<bool> UpdateSlotAsync(Slot slot)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Slots.FindAsync(slot.Id);
            if (entity == null) return false;

            entity.Name = slot.Name;
            entity.StartTime = slot.StartTime;
            entity.EndTime = slot.EndTime;

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

        public async Task<bool> DeleteSlotAsync(int id)
        {
            using var context = _dbService.CreateContext();
            
            var entity = await context.Slots.FindAsync(id);
            if (entity == null) return false;

            context.Slots.Remove(entity);

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

        public async Task InitializeDefaultSlotsAsync()
        {
            using var context = _dbService.CreateContext();
            
            var existingSlots = await context.Slots.CountAsync();
            if (existingSlots >= 5) return; // Already have 5 slots

            var defaultSlots = new[]
            {
                new Data.Entities.Slot { Name = "Slot 1", StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(9, 0, 0) },
                new Data.Entities.Slot { Name = "Slot 2", StartTime = new TimeSpan(9, 10, 0), EndTime = new TimeSpan(10, 40, 0) },
                new Data.Entities.Slot { Name = "Slot 3", StartTime = new TimeSpan(10, 50, 0), EndTime = new TimeSpan(12, 20, 0) },
                new Data.Entities.Slot { Name = "Slot 4", StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(14, 30, 0) },
                new Data.Entities.Slot { Name = "Slot 5", StartTime = new TimeSpan(14, 40, 0), EndTime = new TimeSpan(16, 10, 0) }
            };

            foreach (var slot in defaultSlots)
            {
                var exists = await context.Slots
                    .AnyAsync(s => s.Name == slot.Name);
                if (!exists)
                {
                    context.Slots.Add(slot);
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                // Ignore errors
            }
        }
    }
}
