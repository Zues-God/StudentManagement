using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace StudentManagement.ViewModels
{
    public partial class AdminDashboardViewModel : BaseViewModel
    {
        private readonly CourseService _courseService;
        private readonly EnrollmentService _enrollmentService;
        private readonly ScheduleService _scheduleService;
        private readonly UserService _userService;
        private readonly SlotService _slotService;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Course> courses = new();

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> enrollments = new();

        [ObservableProperty]
        private ObservableCollection<Schedule> schedules = new();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private Course? selectedCourse;

        [ObservableProperty]
        private CourseParticipant? selectedEnrollment;

        [ObservableProperty]
        private Schedule? selectedSchedule;

        [ObservableProperty]
        private User? selectedUser;

        [ObservableProperty]
        private string currentTab = "Courses";

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Course> filteredCourses = new();

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> filteredEnrollments = new();

        [ObservableProperty]
        private ObservableCollection<Schedule> filteredSchedules = new();

        [ObservableProperty]
        private ObservableCollection<User> filteredUsers = new();

        [ObservableProperty]
        private ObservableCollection<Slot> slots = new();

        [ObservableProperty]
        private Slot? selectedSlot;

        [ObservableProperty]
        private ObservableCollection<Course> filteredCoursesForAssignment = new();

        [ObservableProperty]
        private ObservableCollection<User> lecturers = new();

        [ObservableProperty]
        private string? selectedInstructorFilter;

        [ObservableProperty]
        private int totalCourses;

        [ObservableProperty]
        private int totalStudents;

        [ObservableProperty]
        private int totalLecturers;

        [ObservableProperty]
        private int totalEnrollments;

        [ObservableProperty]
        private int totalSchedules;

        public AdminDashboardViewModel(
            CourseService courseService,
            EnrollmentService enrollmentService,
            ScheduleService scheduleService,
            UserService userService,
            SlotService slotService,
            NavigationService navigationService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _scheduleService = scheduleService;
            _userService = userService;
            _slotService = slotService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoadData()
        {
            await _slotService.InitializeDefaultSlotsAsync();
            await LoadCourses();
            await LoadEnrollments();
            await LoadSchedules();
            await LoadUsers();
            await LoadSlots();
            await LoadLecturers();
            UpdateStatistics();
        }

        [RelayCommand]
        private async Task LoadLecturers()
        {
            var lecturersList = await _userService.GetUsersByRoleAsync("lecture");
            Lecturers.Clear();
            foreach (var lecturer in lecturersList)
            {
                Lecturers.Add(lecturer);
            }
            FilterCoursesForAssignment();
        }

        [RelayCommand]
        private async Task LoadSlots()
        {
            var slotsList = await _slotService.GetAllSlotsAsync();
            Slots.Clear();
            foreach (var slot in slotsList)
            {
                Slots.Add(slot);
            }
        }

        private void UpdateStatistics()
        {
            TotalCourses = Courses.Count;
            TotalEnrollments = Enrollments.Count;
            TotalSchedules = Schedules.Count;
            TotalStudents = Users.Count(u => u.Role == "student");
            TotalLecturers = Users.Count(u => u.Role == "lecture");
        }

        [RelayCommand]
        private async Task LoadCourses()
        {
            var coursesList = await _courseService.GetAllCoursesAsync();
            Courses.Clear();
            foreach (var course in coursesList)
            {
                Courses.Add(course);
            }
            FilterCourses();
            FilterCoursesForAssignment();
            UpdateStatistics();
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterCourses();
            FilterEnrollments();
            FilterSchedules();
            FilterUsers();
        }

        private void FilterCourses()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredCourses.Clear();
                foreach (var course in Courses)
                {
                    FilteredCourses.Add(course);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredCourses.Clear();
                foreach (var course in Courses.Where(c => 
                    c.Code.ToLower().Contains(searchLower) ||
                    c.Name.ToLower().Contains(searchLower) ||
                    (c.InstructorName?.ToLower().Contains(searchLower) ?? false)))
                {
                    FilteredCourses.Add(course);
                }
            }
        }

        private void FilterEnrollments()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredEnrollments.Clear();
                foreach (var enrollment in Enrollments)
                {
                    FilteredEnrollments.Add(enrollment);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredEnrollments.Clear();
                foreach (var enrollment in Enrollments.Where(e =>
                    (e.CourseName?.ToLower().Contains(searchLower) ?? false) ||
                    (e.StudentName?.ToLower().Contains(searchLower) ?? false) ||
                    (e.StudentEmail?.ToLower().Contains(searchLower) ?? false)))
                {
                    FilteredEnrollments.Add(enrollment);
                }
            }
        }

        private void FilterSchedules()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredSchedules.Clear();
                foreach (var schedule in Schedules)
                {
                    FilteredSchedules.Add(schedule);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredSchedules.Clear();
                foreach (var schedule in Schedules.Where(s =>
                    (s.CourseName?.ToLower().Contains(searchLower) ?? false) ||
                    s.DayOfWeek.ToLower().Contains(searchLower) ||
                    (s.Room?.ToLower().Contains(searchLower) ?? false)))
                {
                    FilteredSchedules.Add(schedule);
                }
            }
        }

        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredUsers.Clear();
                foreach (var user in Users)
                {
                    FilteredUsers.Add(user);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredUsers.Clear();
                foreach (var user in Users.Where(u =>
                    u.Username.ToLower().Contains(searchLower) ||
                    u.Email.ToLower().Contains(searchLower) ||
                    (u.FullName?.ToLower().Contains(searchLower) ?? false) ||
                    u.Role.ToLower().Contains(searchLower)))
                {
                    FilteredUsers.Add(user);
                }
            }
        }

        [RelayCommand]
        private async Task LoadEnrollments()
        {
            var enrollmentsList = await _enrollmentService.GetAllEnrollmentsAsync();
            Enrollments.Clear();
            foreach (var enrollment in enrollmentsList)
            {
                Enrollments.Add(enrollment);
            }
            FilterEnrollments();
            UpdateStatistics();
        }

        [RelayCommand]
        private async Task LoadSchedules()
        {
            var schedulesList = await _scheduleService.GetAllSchedulesAsync();
            Schedules.Clear();
            foreach (var schedule in schedulesList)
            {
                Schedules.Add(schedule);
            }
            FilterSchedules();
        }

        [RelayCommand]
        private async Task LoadUsers()
        {
            var usersList = await _userService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in usersList)
            {
                Users.Add(user);
            }
            FilterUsers();
            UpdateStatistics();
        }

        [RelayCommand]
        private async Task ShowCourseDialog()
        {
            var dialog = new Views.CourseDialog(SelectedCourse);
            if (dialog.ShowDialog() == true && dialog.Course != null)
            {
                bool success;
                if (dialog.Course.Id == 0)
                {
                    success = await _courseService.CreateCourseAsync(dialog.Course);
                    StatusMessage = success ? "Course created successfully" : "Failed to create course";
                }
                else
                {
                    success = await _courseService.UpdateCourseAsync(dialog.Course);
                    StatusMessage = success ? "Course updated successfully" : "Failed to update course";
                }

                if (success)
                {
                    await LoadCourses();
                }
            }
        }

        [RelayCommand]
        private async Task SaveCourse()
        {
            if (SelectedCourse == null) return;
            await ShowCourseDialog();
        }

        [RelayCommand]
        private async Task AssignInstructor()
        {
            if (SelectedCourse == null) return;

            var dialog = new Views.AssignInstructorDialog(SelectedCourse);
            if (dialog.ShowDialog() == true)
            {
                var success = await _courseService.AssignInstructorAsync(
                    dialog.CourseId, 
                    dialog.InstructorId);
                
                StatusMessage = success 
                    ? "Instructor assigned successfully" 
                    : "Failed to assign instructor";

                if (success)
                {
                    await LoadCourses();
                    FilterCoursesForAssignment();
                }
            }
        }

        [RelayCommand]
        private async Task RemoveInstructor()
        {
            if (SelectedCourse == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove the instructor from course '{SelectedCourse.Name}'?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var success = await _courseService.AssignInstructorAsync(SelectedCourse.Id, null);
                StatusMessage = success 
                    ? "Instructor removed successfully" 
                    : "Failed to remove instructor";

                if (success)
                {
                    await LoadCourses();
                    FilterCoursesForAssignment();
                }
            }
        }

        partial void OnSelectedInstructorFilterChanged(string? value)
        {
            FilterCoursesForAssignment();
        }

        private void FilterCoursesForAssignment()
        {
            FilteredCoursesForAssignment.Clear();
            
            IEnumerable<Course> coursesToShow = Courses;

            if (!string.IsNullOrWhiteSpace(SelectedInstructorFilter))
            {
                if (int.TryParse(SelectedInstructorFilter, out int instructorId))
                {
                    coursesToShow = coursesToShow.Where(c => c.InstructorId == instructorId);
                }
            }

            foreach (var course in coursesToShow.OrderBy(c => c.Code))
            {
                FilteredCoursesForAssignment.Add(course);
            }
        }

        [RelayCommand]
        private async Task DeleteCourse()
        {
            if (SelectedCourse == null) return;

            var success = await _courseService.DeleteCourseAsync(SelectedCourse.Id);
            StatusMessage = success ? "Course deleted successfully" : "Failed to delete course";

            if (success)
            {
                await LoadCourses();
                SelectedCourse = null;
            }
        }

        [RelayCommand]
        private async Task ShowEnrollmentDialog()
        {
            var dialog = new Views.EnrollmentDialog();
            if (dialog.ShowDialog() == true)
            {
                var success = await _enrollmentService.CreateEnrollmentAsync(
                    dialog.CourseId, 
                    dialog.UserId);
                
                StatusMessage = success ? "Enrollment created successfully" : "Failed to create enrollment";

                if (success)
                {
                    await LoadEnrollments();
                }
            }
        }

        [RelayCommand]
        private async Task SaveEnrollment()
        {
            await ShowEnrollmentDialog();
        }

        [RelayCommand]
        private async Task DeleteEnrollment()
        {
            if (SelectedEnrollment == null) return;

            var success = await _enrollmentService.DeleteEnrollmentAsync(SelectedEnrollment.Id);
            StatusMessage = success ? "Enrollment deleted successfully" : "Failed to delete enrollment";

            if (success)
            {
                await LoadEnrollments();
                SelectedEnrollment = null;
            }
        }

        [RelayCommand]
        private async Task ShowScheduleDialog()
        {
            var dialog = new Views.ScheduleDialog(SelectedSchedule);
            if (dialog.ShowDialog() == true && dialog.Schedule != null)
            {
                bool success;
                if (dialog.Schedule.Id == 0)
                {
                    success = await _scheduleService.CreateScheduleAsync(dialog.Schedule);
                    StatusMessage = success ? "Schedule created successfully" : "Failed to create schedule";
                }
                else
                {
                    success = await _scheduleService.UpdateScheduleAsync(dialog.Schedule);
                    StatusMessage = success ? "Schedule updated successfully" : "Failed to update schedule";
                }

                if (success)
                {
                    await LoadSchedules();
                }
            }
        }

        [RelayCommand]
        private async Task SaveSchedule()
        {
            if (SelectedSchedule == null) return;
            await ShowScheduleDialog();
        }

        [RelayCommand]
        private async Task DeleteSchedule()
        {
            if (SelectedSchedule == null) return;

            var success = await _scheduleService.DeleteScheduleAsync(SelectedSchedule.Id);
            StatusMessage = success ? "Schedule deleted successfully" : "Failed to delete schedule";

            if (success)
            {
                await LoadSchedules();
                SelectedSchedule = null;
            }
        }

        [RelayCommand]
        private async Task UpdateUserRole()
        {
            if (SelectedUser == null) return;

            var success = await _userService.UpdateUserRoleAsync(SelectedUser.Id, SelectedUser.Role);
            StatusMessage = success ? "User role updated successfully" : "Failed to update user role";

            if (success)
            {
                await LoadUsers();
            }
        }

        [RelayCommand]
        private async Task DeleteUser()
        {
            if (SelectedUser == null) return;

            var success = await _userService.DeleteUserAsync(SelectedUser.Id);
            StatusMessage = success ? "User deleted successfully" : "Failed to delete user";

            if (success)
            {
                await LoadUsers();
                SelectedUser = null;
            }
        }

        [RelayCommand]
        private async Task ShowSlotDialog()
        {
            var dialog = new Views.SlotDialog(SelectedSlot);
            if (dialog.ShowDialog() == true && dialog.Slot != null)
            {
                bool success;
                if (dialog.Slot.Id == 0)
                {
                    success = await _slotService.CreateSlotAsync(dialog.Slot);
                    StatusMessage = success ? "Slot created successfully" : "Failed to create slot";
                }
                else
                {
                    success = await _slotService.UpdateSlotAsync(dialog.Slot);
                    StatusMessage = success ? "Slot updated successfully" : "Failed to update slot";
                }

                if (success)
                {
                    await LoadSlots();
                }
            }
        }

        [RelayCommand]
        private async Task DeleteSlot()
        {
            if (SelectedSlot == null) return;

            var success = await _slotService.DeleteSlotAsync(SelectedSlot.Id);
            StatusMessage = success ? "Slot deleted successfully" : "Failed to delete slot";

            if (success)
            {
                await LoadSlots();
                SelectedSlot = null;
            }
        }

        [RelayCommand]
        private async Task BulkEnrollStudents()
        {
            var dialog = new Views.BulkEnrollmentDialog();
            if (dialog.ShowDialog() == true && dialog.CourseId > 0 && dialog.StudentIds.Count > 0)
            {
                int successCount = 0;
                int failCount = 0;

                foreach (var studentId in dialog.StudentIds)
                {
                    var success = await _enrollmentService.CreateEnrollmentAsync(dialog.CourseId, studentId);
                    if (success) successCount++;
                    else failCount++;
                }

                StatusMessage = $"Bulk enrollment completed: {successCount} succeeded, {failCount} failed";
                await LoadEnrollments();
            }
        }

        [RelayCommand]
        private void Logout()
        {
            CurrentUser.Instance.Clear();
            _navigationService.NavigateToLogin();
        }
    }
}

