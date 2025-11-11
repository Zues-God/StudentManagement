using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Views
{
    public partial class EnrollmentDialog : Window
    {
        public int CourseId { get; private set; }
        public int UserId { get; private set; }

        public EnrollmentDialog()
        {
            InitializeComponent();
            Loaded += EnrollmentDialog_Loaded;
        }

        private async void EnrollmentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var dbService = new DatabaseService();
            var courseService = new CourseService(dbService);
            var userService = new UserService(dbService);

            var courses = await courseService.GetAllCoursesAsync();
            var students = await userService.GetUsersByRoleAsync("student");

            CourseComboBox.ItemsSource = courses;
            UserComboBox.ItemsSource = students;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseComboBox.SelectedItem is Course selectedCourse &&
                UserComboBox.SelectedItem is User selectedUser)
            {
                CourseId = selectedCourse.Id;
                UserId = selectedUser.Id;
                
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select both a course and a student.", "Validation Error", 
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

