namespace StudentManagement.Models
{
    public class CourseParticipant
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public int UserId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public DateTime? EnrolledAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

