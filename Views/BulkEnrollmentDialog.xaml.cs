using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Views
{
    public class StudentSelectionItem
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    public partial class BulkEnrollmentDialog : Window
    {
        public int CourseId { get; private set; }
        public List<int> StudentIds { get; private set; } = new();

        public BulkEnrollmentDialog()
        {
            InitializeComponent();
            Loaded += BulkEnrollmentDialog_Loaded;
        }

        private async void BulkEnrollmentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var dbService = new DatabaseService();
            var courseService = new CourseService(dbService);
            var userService = new UserService(dbService);

            var courses = await courseService.GetAllCoursesAsync();
            var students = await userService.GetUsersByRoleAsync("student");

            CourseComboBox.ItemsSource = courses;
            StudentsDataGrid.ItemsSource = students.Select(s => new StudentSelectionItem
            {
                Id = s.Id,
                FullName = s.FullName ?? s.Username,
                Email = s.Email,
                Username = s.Username,
                IsSelected = false
            }).ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseComboBox.SelectedItem is not Course selectedCourse)
            {
                MessageBox.Show("Please select a course.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedStudents = StudentsDataGrid.ItemsSource?
                .Cast<StudentSelectionItem>()
                .Where(s => s.IsSelected)
                .ToList();

            if (selectedStudents == null || selectedStudents.Count == 0)
            {
                MessageBox.Show("Please select at least one student.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CourseId = selectedCourse.Id;
            StudentIds = selectedStudents.Select(s => s.Id).ToList();

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

