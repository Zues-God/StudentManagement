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
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StudentManagement.Views.Lecturer
{
    public partial class LecturerSchedule : Window
    {
        private readonly AppDbContext _context = new AppDbContext();
        private int _lecturerId = 3; // 🧠 tạm test với ID 3
        private int _selectedCourseId;

        public LecturerSchedule()
        {
            InitializeComponent();
            LoadLecturerCourses();
        }

        // 🧾 Load danh sách lớp mà giảng viên dạy
        private void LoadLecturerCourses()
        {
            var courses = from cp in _context.CourseParticipants
                          join c in _context.Courses on cp.CourseId equals c.Id
                          where cp.UserId == _lecturerId && cp.RoleInCourse == "lecturer"
                          select c;

            dgCourses.ItemsSource = courses.ToList();
        }

        // Khi chọn 1 lớp → load danh sách sinh viên
        private void dgCourses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected = dgCourses.SelectedItem as Course;
            if (selected != null)
            {
                _selectedCourseId = selected.Id;
                LoadStudentsInCourse(_selectedCourseId);
            }
        }

        // 🧠 Load danh sách sinh viên trong lớp
        private void LoadStudentsInCourse(int courseId)
        {
            var students = from cp in _context.CourseParticipants
                           join u in _context.Users on cp.UserId equals u.Id
                           where cp.CourseId == courseId && cp.RoleInCourse == "student"
                           select new StudentAttendanceVM
                           {
                               UserId = u.Id,
                               FullName = u.FullName,
                               IsPresent = false // mặc định false
                           };

            dgStudents.ItemsSource = students.ToList();
        }

        // 💾 Lưu điểm danh (hiện tại chỉ mô phỏng)
        private void BtnSaveAttendance_Click(object sender, RoutedEventArgs e)
        {
            var data = dgStudents.ItemsSource as List<StudentAttendanceVM>;
            if (data == null || _selectedCourseId == 0)
            {
                MessageBox.Show("Please select a course first.");
                return;
            }

            // Tạm demo hiển thị
            int presentCount = data.Count(x => x.IsPresent);
            MessageBox.Show($"Attendance saved!\nPresent: {presentCount}/{data.Count}", "Success");
        }
    }

    // 👇 ViewModel phụ để hiển thị danh sách sinh viên điểm danh
    public class StudentAttendanceVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public bool IsPresent { get; set; }
    }
}
