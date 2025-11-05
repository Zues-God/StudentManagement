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
    [Table("course_participants")]
    public class CourseParticipant
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("role_in_course")]
        public string RoleInCourse { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("joined_at")]
        public DateTime? JoinedAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
