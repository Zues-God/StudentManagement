namespace StudentManagement.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public int SlotId { get; set; }
        public string? SlotName { get; set; }
        public string? Room { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

