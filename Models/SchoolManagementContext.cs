using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models;

public partial class SchoolManagementContext : DbContext
{
    public SchoolManagementContext()
    {
    }

    public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseParticipant> CourseParticipants { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=school_management;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__courses__3213E83F11C68D7B");

            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MaxParticipants)
                .HasDefaultValue(100)
                .HasColumnName("max_participants");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Semester)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("semester");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Subject).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_courses_subject");
        });

        modelBuilder.Entity<CourseParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__course_p__3213E83F8AD39516");

            entity.ToTable("course_participants");

            entity.HasIndex(e => new { e.CourseId, e.UserId }, "UQ_course_participant").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("joined_at");
            entity.Property(e => e.RoleInCourse)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role_in_course");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseParticipants)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_course_participant_course");

            entity.HasOne(d => d.User).WithMany(p => p.CourseParticipants)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_course_participant_user");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83F05DFF286");

            entity.ToTable("grades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GradeType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("grade_type");
            entity.Property(e => e.LetterGrade)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("letter_grade");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Value)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("value");

            entity.HasOne(d => d.Participant).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ParticipantId)
                .HasConstraintName("FK_grade_participant");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_grade_updated_by");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__schedule__3213E83FA17E42B7");

            entity.ToTable("schedules");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("day_of_week");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.Room)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("room");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Course).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_schedule_course");

            entity.HasOne(d => d.Slot).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK_schedule_slot");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__slot__3213E83F2A90307C");

            entity.ToTable("slot");

            entity.HasIndex(e => e.SlotNumber, "UQ__slot__ED51435B81DE8A36").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.SlotNumber).HasColumnName("slot_number");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subjects__3213E83F8D6063B0");

            entity.ToTable("subjects");

            entity.HasIndex(e => e.Code, "UQ__subjects__357D4CF93B229285").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Credits)
                .HasDefaultValue(3)
                .HasColumnName("credits");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FB0E47D26");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164E3D898B9").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572335A339D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
