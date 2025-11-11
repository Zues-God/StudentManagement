using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class GradeService
    {
        private readonly DatabaseService _dbService;

        public GradeService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<Grade>> GetGradesByParticipantAsync(int participantId)
        {
            using var context = _dbService.CreateContext();
            
            var grades = await context.Grades
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.User)
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.Course)
                .Include(g => g.UpdatedByUser)
                .Where(g => g.ParticipantId == participantId)
                .OrderBy(g => g.GradeType)
                .ToListAsync();
            
            return grades.Select(g => new Grade
            {
                Id = g.Id,
                ParticipantId = g.ParticipantId,
                GradeType = g.GradeType,
                Value = g.Value,
                LetterGrade = g.LetterGrade,
                Notes = g.Notes,
                UpdatedBy = g.UpdatedBy,
                UpdatedByName = g.UpdatedByUser?.FullName,
                UpdatedAt = g.UpdatedAt,
                StudentName = g.Participant?.User?.FullName,
                CourseName = g.Participant?.Course?.Name
            }).ToList();
        }

        public async Task<List<Grade>> GetAllGradesAsync()
        {
            using var context = _dbService.CreateContext();
            
            var grades = await context.Grades
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.User)
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.Course)
                .Include(g => g.UpdatedByUser)
                .OrderBy(g => g.Participant!.Course!.Name)
                .ThenBy(g => g.Participant!.User!.FullName)
                .ThenBy(g => g.GradeType)
                .ToListAsync();
            
            return grades.Select(g => new Grade
            {
                Id = g.Id,
                ParticipantId = g.ParticipantId,
                GradeType = g.GradeType,
                Value = g.Value,
                LetterGrade = g.LetterGrade,
                Notes = g.Notes,
                UpdatedBy = g.UpdatedBy,
                UpdatedByName = g.UpdatedByUser?.FullName,
                UpdatedAt = g.UpdatedAt,
                StudentName = g.Participant?.User?.FullName,
                CourseName = g.Participant?.Course?.Name
            }).ToList();
        }

        public async Task<List<Grade>> GetGradesByUserIdAsync(int userId)
        {
            using var context = _dbService.CreateContext();
            
            var grades = await context.Grades
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.User)
                .Include(g => g.Participant!)
                    .ThenInclude(p => p!.Course)
                .Include(g => g.UpdatedByUser)
                .Where(g => g.Participant!.UserId == userId)
                .OrderBy(g => g.Participant!.Course!.Name)
                .ThenBy(g => g.GradeType)
                .ToListAsync();
            
            return grades.Select(g => new Grade
            {
                Id = g.Id,
                ParticipantId = g.ParticipantId,
                GradeType = g.GradeType,
                Value = g.Value,
                LetterGrade = g.LetterGrade,
                Notes = g.Notes,
                UpdatedBy = g.UpdatedBy,
                UpdatedByName = g.UpdatedByUser?.FullName,
                UpdatedAt = g.UpdatedAt,
                StudentName = g.Participant?.User?.FullName,
                CourseName = g.Participant?.Course?.Name
            }).ToList();
        }

        public async Task<bool> UpsertGradeAsync(Grade grade, int updatedBy)
        {
            using var context = _dbService.CreateContext();
            
            var existing = await context.Grades
                .FirstOrDefaultAsync(g => g.ParticipantId == grade.ParticipantId && g.GradeType == grade.GradeType);
            
            if (existing != null)
            {
                // Update existing
                existing.Value = grade.Value;
                existing.LetterGrade = grade.LetterGrade;
                existing.Notes = grade.Notes;
                existing.UpdatedBy = updatedBy;
                existing.UpdatedAt = DateTime.Now;
            }
            else
            {
                // Create new
                var entity = new Data.Entities.Grade
                {
                    ParticipantId = grade.ParticipantId,
                    GradeType = grade.GradeType,
                    Value = grade.Value,
                    LetterGrade = grade.LetterGrade,
                    Notes = grade.Notes,
                    UpdatedBy = updatedBy,
                    UpdatedAt = DateTime.Now
                };
                context.Grades.Add(entity);
            }
            
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
