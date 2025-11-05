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
    [Table("courses")]
    public class Course
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("semester")]
        public string Semester { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("max_participants")]
        public int? MaxParticipants { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
