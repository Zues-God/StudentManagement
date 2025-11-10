using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class CourseParticipant
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    public string RoleInCourse { get; set; } = null!;

    public string? Status { get; set; }

    public DateOnly? JoinedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual User User { get; set; } = null!;
}
