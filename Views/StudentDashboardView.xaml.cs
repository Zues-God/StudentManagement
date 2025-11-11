using System.Windows;
using System.Windows.Controls;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class StudentDashboardView : Window
    {
        public StudentDashboardView()
        {
            InitializeComponent();
            var dbService = new DatabaseService();
            var gradeService = new GradeService(dbService);
            var enrollmentService = new EnrollmentService(dbService);
            var scheduleService = new ScheduleService(dbService);
            var courseService = new CourseService(dbService);
            var navigationService = new NavigationService();
            
            DataContext = new StudentDashboardViewModel(
                gradeService, enrollmentService, scheduleService, courseService, navigationService);
            
            Loaded += (s, e) => WindowState = WindowState.Maximized;
            
            if (DataContext is StudentDashboardViewModel vm)
            {
                Loaded += async (s, e) => await vm.LoadDataCommand.ExecuteAsync(null);
            }
        }

        private void CoursesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Grades will be loaded automatically via OnSelectedCourseChanged
        }

        private void EnrollButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Course course &&
                DataContext is StudentDashboardViewModel vm)
            {
                vm.SelectedAvailableCourse = course;
                vm.RegisterForCourseCommand.ExecuteAsync(null);
            }
        }
    }
}

