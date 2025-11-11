using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Data.Entities
{
    [Table("schedules", Schema = "dbo")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("course_id")]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("day_of_week")]
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        [Column("slot_id")]
        public int SlotId { get; set; }

        [MaxLength(50)]
        [Column("room")]
        public string? Room { get; set; }

        [Column("notes", TypeName = "NVARCHAR(500)")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        [ForeignKey("SlotId")]
        public virtual Slot? Slot { get; set; }
    }
}

