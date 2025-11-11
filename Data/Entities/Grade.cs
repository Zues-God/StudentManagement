using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Data.Entities
{
    [Table("grades", Schema = "dbo")]
    public class Grade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("participant_id")]
        public int ParticipantId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("grade_type")]
        public string GradeType { get; set; } = string.Empty;

        [Column("value", TypeName = "DECIMAL(4,2)")]
        public decimal? Value { get; set; }

        [MaxLength(5)]
        [Column("letter_grade")]
        public string? LetterGrade { get; set; }

        [Column("notes", TypeName = "NVARCHAR(500)")]
        public string? Notes { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("ParticipantId")]
        public virtual CourseParticipant? Participant { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User? UpdatedByUser { get; set; }
    }
}

