using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Slot
{
    public int Id { get; set; }

    public int SlotNumber { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
