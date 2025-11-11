using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class LecturerDashboardView : Window
    {
        public LecturerDashboardView()
        {
            InitializeComponent();
            var dbService = new DatabaseService();
            var gradeService = new GradeService(dbService);
            var enrollmentService = new EnrollmentService(dbService);
            var scheduleService = new ScheduleService(dbService);
            var courseService = new CourseService(dbService);
            var excelService = new ExcelExportService();
            var navigationService = new NavigationService();
            
            DataContext = new LecturerDashboardViewModel(
                gradeService, enrollmentService, scheduleService, courseService,
                excelService, navigationService);
            
            Loaded += (s, e) => WindowState = WindowState.Maximized;
            
            if (DataContext is LecturerDashboardViewModel vm)
            {
                Loaded += async (s, e) => await vm.LoadDataCommand.ExecuteAsync(null);
                vm.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(vm.StatusMessage) && !string.IsNullOrEmpty(vm.StatusMessage))
                    {
                        StatusSnackbar.MessageQueue?.Enqueue(vm.StatusMessage);
                    }
                };
            }
        }

        private void ParticipantsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is LecturerDashboardViewModel vm && vm.SelectedParticipant != null)
            {
                vm.LoadGradesCommand.ExecuteAsync(null);
            }
        }
    }
}

