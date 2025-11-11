namespace StudentManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public string GradeType { get; set; } = string.Empty; // 'project', 'midterm', 'final', 'total'
        public decimal? Value { get; set; }
        public string? LetterGrade { get; set; }
        public string? Notes { get; set; }
        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
    }
}

