using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public int SlotId { get; set; }

    public string? Room { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
