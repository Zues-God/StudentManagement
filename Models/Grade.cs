using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int ParticipantId { get; set; }

    public string GradeType { get; set; } = null!;

    public decimal? Value { get; set; }

    public string? LetterGrade { get; set; }

    public string? Notes { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CourseParticipant Participant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
