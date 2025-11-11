using Microsoft.EntityFrameworkCore;
using StudentManagement.Data.Entities;

namespace StudentManagement.Data
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext(DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseParticipant> CourseParticipants { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20).IsRequired();
                entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("NVARCHAR(500)");
                entity.Property(e => e.Credits).HasColumnName("credits").IsRequired();
                entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Instructor)
                    .WithMany()
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure CourseParticipant entity
            modelBuilder.Entity<CourseParticipant>(entity =>
            {
                entity.ToTable("course_participants", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.CourseId).HasColumnName("course_id").IsRequired();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.EnrolledAt).HasColumnName("enrolled_at").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Course)
                    .WithMany()
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.CourseId, e.UserId }).IsUnique();
            });

            // Configure Grade entity
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("grades", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ParticipantId).HasColumnName("participant_id").IsRequired();
                entity.Property(e => e.GradeType).HasColumnName("grade_type").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Value).HasColumnName("value").HasColumnType("DECIMAL(4,2)");
                entity.Property(e => e.LetterGrade).HasColumnName("letter_grade").HasMaxLength(5);
                entity.Property(e => e.Notes).HasColumnName("notes").HasColumnType("NVARCHAR(500)");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Participant)
                    .WithMany()
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasCheckConstraint("CK_GradeType", "[grade_type] IN ('project', 'midterm', 'final', 'total')");
            });

            // Configure Slot entity
            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("slot", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.StartTime).HasColumnName("start_time").HasColumnType("TIME").IsRequired();
                entity.Property(e => e.EndTime).HasColumnName("end_time").HasColumnType("TIME").IsRequired();
            });

            // Configure Schedule entity
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedules", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.CourseId).HasColumnName("course_id").IsRequired();
                entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week").HasMaxLength(20).IsRequired();
                entity.Property(e => e.SlotId).HasColumnName("slot_id").IsRequired();
                entity.Property(e => e.Room).HasColumnName("room").HasMaxLength(50);
                entity.Property(e => e.Notes).HasColumnName("notes").HasColumnType("NVARCHAR(500)");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Course)
                    .WithMany()
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Slot)
                    .WithMany()
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasCheckConstraint("CK_DayOfWeek", "[day_of_week] IN ('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday')");
            });
        }
    }
}

