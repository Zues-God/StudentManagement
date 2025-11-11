using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "slot",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    start_time = table.Column<TimeSpan>(type: "TIME", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "TIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slot", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    credits = table.Column<int>(type: "int", nullable: false),
                    instructor_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_courses_users_instructor_id",
                        column: x => x.instructor_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_participants",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CourseId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_participants_courses_CourseId1",
                        column: x => x.CourseId1,
                        principalSchema: "dbo",
                        principalTable: "courses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_course_participants_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "dbo",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_participants_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    day_of_week = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    slot_id = table.Column<int>(type: "int", nullable: false),
                    room = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.id);
                    table.CheckConstraint("CK_DayOfWeek", "[day_of_week] IN ('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday')");
                    table.ForeignKey(
                        name: "FK_schedules_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "dbo",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedules_slot_slot_id",
                        column: x => x.slot_id,
                        principalSchema: "dbo",
                        principalTable: "slot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    participant_id = table.Column<int>(type: "int", nullable: false),
                    grade_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    value = table.Column<decimal>(type: "DECIMAL(4,2)", nullable: true),
                    letter_grade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    notes = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.id);
                    table.CheckConstraint("CK_GradeType", "[grade_type] IN ('project', 'midterm', 'final', 'total')");
                    table.ForeignKey(
                        name: "FK_grades_course_participants_participant_id",
                        column: x => x.participant_id,
                        principalSchema: "dbo",
                        principalTable: "course_participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grades_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_participants_course_id_user_id",
                schema: "dbo",
                table: "course_participants",
                columns: new[] { "course_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_participants_CourseId1",
                schema: "dbo",
                table: "course_participants",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_course_participants_user_id",
                schema: "dbo",
                table: "course_participants",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_instructor_id",
                schema: "dbo",
                table: "courses",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_grades_participant_id",
                schema: "dbo",
                table: "grades",
                column: "participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_grades_updated_by",
                schema: "dbo",
                table: "grades",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_course_id",
                schema: "dbo",
                table: "schedules",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_slot_id",
                schema: "dbo",
                table: "schedules",
                column: "slot_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grades",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "schedules",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "course_participants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "slot",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}
