using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace StudentManagement.ViewModels
{
    public partial class LecturerDashboardViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private readonly EnrollmentService _enrollmentService;
        private readonly ScheduleService _scheduleService;
        private readonly CourseService _courseService;
        private readonly ExcelExportService _excelService;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> participants = new();

        [ObservableProperty]
        private ObservableCollection<Grade> grades = new();

        [ObservableProperty]
        private ObservableCollection<Schedule> schedules = new();

        [ObservableProperty]
        private CourseParticipant? selectedParticipant;

        [ObservableProperty]
        private Grade? selectedGrade;

        [ObservableProperty]
        private string currentTab = "Grades";

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> filteredParticipants = new();

        [ObservableProperty]
        private ObservableCollection<Course> myCourses = new();

        [ObservableProperty]
        private Course? selectedMyCourse;

        [ObservableProperty]
        private ObservableCollection<CourseParticipant> courseStudents = new();

        private int _currentUserId;

        public LecturerDashboardViewModel(
            GradeService gradeService,
            EnrollmentService enrollmentService,
            ScheduleService scheduleService,
            CourseService courseService,
            ExcelExportService excelService,
            NavigationService navigationService)
        {
            _gradeService = gradeService;
            _enrollmentService = enrollmentService;
            _scheduleService = scheduleService;
            _courseService = courseService;
            _excelService = excelService;
            _navigationService = navigationService;
            
            _currentUserId = CurrentUser.Instance.User?.Id ?? 0;
        }

        [RelayCommand]
        private async Task LoadData()
        {
            await LoadParticipants();
            await LoadSchedules();
            await LoadMyCourses();
        }

        [RelayCommand]
        private async Task LoadMyCourses()
        {
            if (_currentUserId == 0) return;

            var allCourses = await _courseService.GetAllCoursesAsync();
            MyCourses.Clear();
            foreach (var course in allCourses.Where(c => c.InstructorId == _currentUserId))
            {
                MyCourses.Add(course);
            }
        }

        [RelayCommand]
        private async Task LoadCourseStudents()
        {
            if (SelectedMyCourse == null) return;

            var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            CourseStudents.Clear();
            foreach (var enrollment in allEnrollments.Where(e => e.CourseId == SelectedMyCourse.Id))
            {
                CourseStudents.Add(enrollment);
            }
        }

        partial void OnSelectedMyCourseChanged(Course? value)
        {
            if (value != null)
            {
                LoadCourseStudentsCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task LoadParticipants()
        {
            var enrollmentsList = await _enrollmentService.GetAllEnrollmentsAsync();
            Participants.Clear();
            foreach (var enrollment in enrollmentsList)
            {
                Participants.Add(enrollment);
            }
            FilterParticipants();
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterParticipants();
        }

        private void FilterParticipants()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredParticipants.Clear();
                foreach (var participant in Participants)
                {
                    FilteredParticipants.Add(participant);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                FilteredParticipants.Clear();
                foreach (var participant in Participants.Where(p =>
                    (p.StudentName?.ToLower().Contains(searchLower) ?? false) ||
                    (p.CourseName?.ToLower().Contains(searchLower) ?? false)))
                {
                    FilteredParticipants.Add(participant);
                }
            }
        }

        [RelayCommand]
        private async Task LoadGrades()
        {
            if (SelectedParticipant == null) return;

            var gradesList = await _gradeService.GetGradesByParticipantAsync(SelectedParticipant.Id);
            Grades.Clear();
            foreach (var grade in gradesList)
            {
                Grades.Add(grade);
            }
        }

        [RelayCommand]
        private async Task LoadSchedules()
        {
            if (_currentUserId == 0) return;

            var schedulesList = await _scheduleService.GetSchedulesByUserAsync(_currentUserId, "lecture");
            Schedules.Clear();
            foreach (var schedule in schedulesList)
            {
                Schedules.Add(schedule);
            }
        }

        [RelayCommand]
        private void ShowGradeDialog(string? gradeType)
        {
            if (SelectedParticipant == null || string.IsNullOrEmpty(gradeType)) return;

            SelectedGrade = new Grade
            {
                ParticipantId = SelectedParticipant.Id,
                GradeType = gradeType
            };
        }

        [ObservableProperty]
        private string gradeValueText = string.Empty;

        [ObservableProperty]
        private string letterGradeText = string.Empty;

        [ObservableProperty]
        private string notesText = string.Empty;

        partial void OnSelectedGradeChanged(Grade? value)
        {
            if (value != null)
            {
                GradeValueText = value.Value?.ToString() ?? string.Empty;
                LetterGradeText = value.LetterGrade ?? string.Empty;
                NotesText = value.Notes ?? string.Empty;
            }
            else
            {
                GradeValueText = string.Empty;
                LetterGradeText = string.Empty;
                NotesText = string.Empty;
            }
        }

        [RelayCommand]
        private async Task SaveGrade()
        {
            if (SelectedGrade == null) return;

            // Update SelectedGrade with text values
            if (decimal.TryParse(GradeValueText, out decimal value))
            {
                SelectedGrade.Value = value;
            }
            else if (string.IsNullOrWhiteSpace(GradeValueText))
            {
                SelectedGrade.Value = null;
            }

            SelectedGrade.LetterGrade = string.IsNullOrWhiteSpace(LetterGradeText) ? null : LetterGradeText;
            SelectedGrade.Notes = string.IsNullOrWhiteSpace(NotesText) ? null : NotesText;

            var success = await _gradeService.UpsertGradeAsync(SelectedGrade, _currentUserId);
            StatusMessage = success ? "Grade saved successfully" : "Failed to save grade";

            if (success)
            {
                await LoadGrades();
                SelectedGrade = null;
            }
        }

        [RelayCommand]
        private async Task ExportGradesToExcel()
        {
            var allGrades = await _gradeService.GetAllGradesAsync();
            
            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = $"Grades_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var success = await _excelService.ExportGradesToExcelAsync(allGrades, saveDialog.FileName);
                StatusMessage = success ? "Grades exported successfully" : "Failed to export grades";
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

