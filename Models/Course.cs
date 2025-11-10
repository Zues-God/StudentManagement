using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Course
{
    public int Id { get; set; }

    public int SubjectId { get; set; }

    public string Name { get; set; } = null!;

    public string? Semester { get; set; }

    public int? Year { get; set; }

    public int? MaxParticipants { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CourseParticipant> CourseParticipants { get; set; } = new List<CourseParticipant>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Subject Subject { get; set; } = null!;
}
