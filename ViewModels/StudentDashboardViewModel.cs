using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace StudentManagement.ViewModels
{
    public partial class StudentDashboardViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private readonly EnrollmentService _enrollmentService;
        private readonly ScheduleService _scheduleService;
        private readonly CourseService _courseService;

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> myCourses = new();

        [ObservableProperty]
        private ObservableCollection<Course> availableCourses = new();

        [ObservableProperty]
        private Course? selectedAvailableCourse;

        [ObservableProperty]
        private ObservableCollection<Grade> myGrades = new();

        [ObservableProperty]
        private ObservableCollection<Grade> currentCourseGrades = new();

        [ObservableProperty]
        private ObservableCollection<Schedule> mySchedules = new();

        [ObservableProperty]
        private CourseParticipant? selectedCourse;

        [ObservableProperty]
        private string currentTab = "Courses";

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> filteredCourses = new();

        private int _currentUserId;

        private readonly NavigationService _navigationService;

        public StudentDashboardViewModel(
            GradeService gradeService,
            EnrollmentService enrollmentService,
            ScheduleService scheduleService,
            CourseService courseService,
            NavigationService navigationService)
        {
            _gradeService = gradeService;
            _enrollmentService = enrollmentService;
            _scheduleService = scheduleService;
            _courseService = courseService;
            _navigationService = navigationService;
            
            _currentUserId = CurrentUser.Instance.User?.Id ?? 0;
            
            // Subscribe to collection changes to recalculate statistics
            MyGrades.CollectionChanged += (s, e) => CalculateStatistics();
        }

        [RelayCommand]
        private async Task LoadData()
        {
            await LoadMyCourses();
            await LoadMySchedules();
            await LoadAvailableCourses();
            await LoadAllMyGrades();
        }

        [RelayCommand]
        private async Task LoadAvailableCourses()
        {
            if (_currentUserId == 0) return;

            var allCourses = await _courseService.GetAllCoursesAsync();
            var myEnrolledCourseIds = MyCourses.Select(c => c.CourseId).ToHashSet();

            AvailableCourses.Clear();
            foreach (var course in allCourses.Where(c => !myEnrolledCourseIds.Contains(c.Id)))
            {
                AvailableCourses.Add(course);
            }
        }

        [RelayCommand]
        private async Task RegisterForCourse()
        {
            if (SelectedAvailableCourse == null || _currentUserId == 0) return;

            var success = await _enrollmentService.CreateEnrollmentAsync(SelectedAvailableCourse.Id, _currentUserId);
            StatusMessage = success ? "Successfully enrolled in course" : "Failed to enroll. You may already be enrolled.";

            if (success)
            {
                await LoadMyCourses();
                await LoadAvailableCourses();
                SelectedAvailableCourse = null;
            }
        }

        [ObservableProperty]
        private double averageGrade;

        [ObservableProperty]
        private int totalCredits;

        partial void OnMyGradesChanged(ObservableCollection<Grade> value)
        {
            CalculateStatistics();
        }

        private void CalculateStatistics()
        {
            if (MyGrades == null || MyGrades.Count == 0)
            {
                AverageGrade = 0;
                OnPropertyChanged(nameof(AverageGrade));
                return;
            }

            // Calculate average from all grades that have values
            // Prefer 'total' grade type for overall average, or average all if no total grades
            var totalGrades = MyGrades.Where(g => g.Value.HasValue && g.GradeType == "total").ToList();
            
            if (totalGrades.Count > 0)
            {
                // Use total grades if available (these are course final grades)
                var sum = totalGrades.Sum(g => (double)g.Value!.Value);
                AverageGrade = sum / totalGrades.Count;
            }
            else
            {
                // Fallback: average all grades with values (project, midterm, final)
                var allGradesWithValues = MyGrades.Where(g => g.Value.HasValue).ToList();
                if (allGradesWithValues.Count > 0)
                {
                    var sum = allGradesWithValues.Sum(g => (double)g.Value!.Value);
                    AverageGrade = sum / allGradesWithValues.Count;
                }
                else
                {
                    AverageGrade = 0;
                }
            }
            
            // Explicitly notify UI of the change
            OnPropertyChanged(nameof(AverageGrade));
        }

        [RelayCommand]
        private async Task LoadMyCourses()
        {
            if (_currentUserId == 0) return;

            var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            MyCourses.Clear();
            foreach (var enrollment in allEnrollments.Where(e => e.UserId == _currentUserId))
            {
                MyCourses.Add(enrollment);
            }
            FilterCourses();
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterCourses();
        }

        private void FilterCourses()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredCourses.Clear();
                foreach (var course in MyCourses)
                {
                    FilteredCourses.Add(course);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredCourses.Clear();
                foreach (var course in MyCourses.Where(c =>
                    (c.CourseName?.ToLower().Contains(searchLower) ?? false)))
                {
                    FilteredCourses.Add(course);
                }
            }
        }

        [RelayCommand]
        private async Task LoadAllMyGrades()
        {
            if (_currentUserId == 0) return;

            var allGrades = await _gradeService.GetGradesByUserIdAsync(_currentUserId);
            MyGrades.Clear();
            foreach (var grade in allGrades)
            {
                MyGrades.Add(grade);
            }
            // Trigger recalculation after loading
            CalculateStatistics();
            OnPropertyChanged(nameof(MyGrades)); // Ensure UI updates
        }

        [RelayCommand]
        private async Task LoadMyGrades()
        {
            if (SelectedCourse == null || _currentUserId == 0) return;

            var gradesList = await _gradeService.GetGradesByParticipantAsync(SelectedCourse.Id);
            CurrentCourseGrades.Clear();
            foreach (var grade in gradesList)
            {
                CurrentCourseGrades.Add(grade);
            }
        }

        partial void OnSelectedCourseChanged(CourseParticipant? value)
        {
            if (value != null)
            {
                LoadMyGradesCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task LoadMySchedules()
        {
            if (_currentUserId == 0) return;

            var schedulesList = await _scheduleService.GetSchedulesByUserAsync(_currentUserId, "student");
            MySchedules.Clear();
            foreach (var schedule in schedulesList)
            {
                MySchedules.Add(schedule);
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

