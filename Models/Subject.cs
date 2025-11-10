using System;
using System.Collections.Generic;

namespace StudentManagement.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Credits { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
