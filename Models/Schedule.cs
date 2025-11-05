using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    [Table("schedules")]
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        [Column("id")]
        public int Id { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("day_of_week")]
        public string DayOfWeek { get; set; }

        [Column("slot_id")]
        public int SlotId { get; set; }

        [Column("room")]
        public string Room { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
