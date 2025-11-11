using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class AdminDashboardView : Window
    {
        public AdminDashboardView()
        {
            InitializeComponent();
            var dbService = new DatabaseService();
            var courseService = new CourseService(dbService);
            var enrollmentService = new EnrollmentService(dbService);
            var scheduleService = new ScheduleService(dbService);
            var userService = new UserService(dbService);
            var slotService = new SlotService(dbService);
            var navigationService = new NavigationService();
            
            DataContext = new AdminDashboardViewModel(
                courseService, enrollmentService, scheduleService, 
                userService, slotService, navigationService);
            
            Loaded += (s, e) => WindowState = WindowState.Maximized;
            
            if (DataContext is AdminDashboardViewModel vm)
            {
                Loaded += async (s, e) => 
                {
                    await vm.LoadDataCommand.ExecuteAsync(null);
                    PopulateInstructorFilter();
                };
                vm.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(vm.SelectedUser) && vm.SelectedUser != null)
                    {
                        // Set ComboBox selection based on user role
                        foreach (ComboBoxItem item in RoleComboBox.Items)
                        {
                            if (item.Content.ToString() == vm.SelectedUser.Role)
                            {
                                RoleComboBox.SelectedItem = item;
                                break;
                            }
                        }
                    }
                    if (e.PropertyName == nameof(vm.Lecturers))
                    {
                        PopulateInstructorFilter();
                    }
                    if (e.PropertyName == nameof(vm.StatusMessage) && !string.IsNullOrEmpty(vm.StatusMessage))
                    {
                        StatusSnackbar.MessageQueue?.Enqueue(vm.StatusMessage);
                    }
                };
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel vm && 
                vm.SelectedUser != null && 
                RoleComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                vm.SelectedUser.Role = selectedItem.Content.ToString()!;
            }
        }

        private void InstructorFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AdminDashboardViewModel vm && 
                sender is ComboBox comboBox &&
                comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();
                vm.SelectedInstructorFilter = tag;
            }
        }

        private void QuickAssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && 
                button.CommandParameter is Course course &&
                DataContext is AdminDashboardViewModel vm)
            {
                vm.SelectedCourse = course;
                vm.AssignInstructorCommand.ExecuteAsync(null);
            }
        }

        private void PopulateInstructorFilter()
        {
            if (DataContext is AdminDashboardViewModel vm)
            {
                InstructorFilterComboBox.Items.Clear();
                InstructorFilterComboBox.Items.Add(new ComboBoxItem { Content = "All Instructors", Tag = "" });
                
                foreach (var lecturer in vm.Lecturers)
                {
                    InstructorFilterComboBox.Items.Add(new ComboBoxItem 
                    { 
                        Content = lecturer.FullName ?? lecturer.Username, 
                        Tag = lecturer.Id.ToString() 
                    });
                }
                
                if (InstructorFilterComboBox.Items.Count > 0)
                {
                    InstructorFilterComboBox.SelectedIndex = 0;
                }
            }
        }
    }
}
