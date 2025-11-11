using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Views
{
    public partial class CourseDialog : Window
    {
        public Course? Course { get; private set; }

        public CourseDialog(Course? course = null)
        {
            InitializeComponent();
            Course = course ?? new Course();
            Loaded += CourseDialog_Loaded;
        }

        private async void CourseDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var dbService = new DatabaseService();
            var userService = new UserService(dbService);

            var lecturers = await userService.GetUsersByRoleAsync("lecture");
            InstructorComboBox.ItemsSource = lecturers;

            if (Course != null && Course.Id > 0)
            {
                CodeTextBox.Text = Course.Code;
                NameTextBox.Text = Course.Name;
                DescriptionTextBox.Text = Course.Description ?? "";
                CreditsTextBox.Text = Course.Credits.ToString();
                if (Course.InstructorId.HasValue)
                {
                    InstructorComboBox.SelectedItem = lecturers.FirstOrDefault(l => l.Id == Course.InstructorId.Value);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CodeTextBox.Text) || 
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                !int.TryParse(CreditsTextBox.Text, out int credits))
            {
                MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Course!.Code = CodeTextBox.Text;
            Course.Name = NameTextBox.Text;
            Course.Description = string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ? null : DescriptionTextBox.Text;
            Course.Credits = credits;
            Course.InstructorId = InstructorComboBox.SelectedItem is User selectedInstructor 
                ? selectedInstructor.Id 
                : null;
            
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

