using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Data.Entities
{
    [Table("slot", Schema = "dbo")]
    public class Slot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("start_time", TypeName = "TIME")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time", TypeName = "TIME")]
        public TimeSpan EndTime { get; set; }
    }
}

