using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Views
{
    public partial class AssignInstructorDialog : Window
    {
        public int CourseId { get; private set; }
        public int? InstructorId { get; private set; }

        public AssignInstructorDialog(Course course)
        {
            InitializeComponent();
            CourseId = course.Id;
            CourseInfoTextBlock.Text = $"Course: {course.Code} - {course.Name}";
            Loaded += AssignInstructorDialog_Loaded;
        }

        private async void AssignInstructorDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var dbService = new DatabaseService();
            var userService = new UserService(dbService);
            var courseService = new CourseService(dbService);

            var lecturers = await userService.GetUsersByRoleAsync("lecture");
            InstructorComboBox.ItemsSource = lecturers;

            // Load current course to get current instructor
            var courses = await courseService.GetAllCoursesAsync();
            var currentCourse = courses.FirstOrDefault(c => c.Id == CourseId);
            
            if (currentCourse?.InstructorId.HasValue == true)
            {
                InstructorComboBox.SelectedItem = lecturers.FirstOrDefault(l => l.Id == currentCourse.InstructorId.Value);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (InstructorComboBox.SelectedItem is User selectedInstructor)
            {
                InstructorId = selectedInstructor.Id;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select an instructor.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

