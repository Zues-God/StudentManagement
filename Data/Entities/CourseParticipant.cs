using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Data.Entities
{
    [Table("course_participants", Schema = "dbo")]
    public class CourseParticipant
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("course_id")]
        public int CourseId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("enrolled_at")]
        public DateTime? EnrolledAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}

