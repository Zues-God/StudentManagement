using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StudentManagement.Models;
using System.Linq;
using System.Windows;

namespace StudentManagement.Views.Student
{
    public partial class StudentSchedule : Window
    {
        private readonly AppDbContext _context = new AppDbContext();

        public StudentSchedule()
        {
            InitializeComponent();
            LoadStudentSchedule(1); //student_id = 1 để test
        }

        private void LoadStudentSchedule(int studentId)
        {
            // Lấy danh sách schedule mà sinh viên đã đăng ký học
            var query = from cp in _context.CourseParticipants
                        join c in _context.Courses on cp.CourseId equals c.Id
                        join s in _context.Schedules on c.Id equals s.CourseId
                        where cp.UserId == studentId
                        select new
                        {
                            CourseName = c.Name,
                            s.DayOfWeek,
                            s.SlotId,
                            s.Room,
                            s.Notes
                        };

            dgStudentSchedule.ItemsSource = query.ToList();
        }
    }
}
