using StudentManagement.Helper;
using StudentManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class LecturerScheduleViewModel : BaseViewModel
    {
        private readonly AppDbContext _context = new AppDbContext();
        public int LecturerId { get; }

        // Schedules that this lecturer teaches
        private ObservableCollection<ScheduleItem> _schedules;
        public ObservableCollection<ScheduleItem> Schedules
        {
            get => _schedules;
            set { _schedules = value; OnPropertyChanged(nameof(Schedules)); }
        }

        private ScheduleItem _selectedSchedule;
        public ScheduleItem SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged(nameof(SelectedSchedule));
                if (_selectedSchedule != null) LoadStudentsForSelectedSchedule();
                else Students = new ObservableCollection<StudentItem>();
            }
        }

        // Students for selected schedule
        private ObservableCollection<StudentItem> _students = new ObservableCollection<StudentItem>();
        public ObservableCollection<StudentItem> Students
        {
            get => _students;
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }

        public ICommand LoadStudentsCommand { get; }
        public ICommand SaveAttendanceCommand { get; }
        public ICommand ReloadSchedulesCommand { get; }

        public LecturerScheduleViewModel(int lecturerId)
        {
            LecturerId = lecturerId;
            LoadStudentsCommand = new RelayCommand(o => LoadStudentsForSelectedSchedule(), o => SelectedSchedule != null);
            SaveAttendanceCommand = new RelayCommand(o => SaveAttendance(), o => SelectedSchedule != null && Students != null && Students.Any());
            ReloadSchedulesCommand = new RelayCommand(o => LoadSchedules());

            LoadSchedules();
        }

        // Load schedules taught by this lecturer (using course_participants table)
        private void LoadSchedules()
        {
            var q = from cp in _context.CourseParticipants
                    join c in _context.Courses on cp.CourseId equals c.Id
                    join s in _context.Schedules on c.Id equals s.CourseId
                    join sl in _context.Slots on s.SlotId equals sl.Id
                    where cp.UserId == LecturerId && cp.RoleInCourse == "lecturer"
                    select new ScheduleItem
                    {
                        ScheduleId = s.Id,
                        CourseId = c.Id,
                        CourseName = c.Name,
                        DayOfWeek = s.DayOfWeek,
                        Slot = $"Slot {sl.SlotNumber}",
                        Room = s.Room,
                        Notes = s.Notes
                    };

            Schedules = new ObservableCollection<ScheduleItem>(q.Distinct().ToList());
        }

        private void LoadStudentsForSelectedSchedule()
        {
            if (SelectedSchedule == null) return;

            // students enrolled in the course (role_in_course = 'student')
            var students = from cp in _context.CourseParticipants
                           join u in _context.Users on cp.UserId equals u.Id
                           where cp.CourseId == SelectedSchedule.CourseId && cp.RoleInCourse == "student"
                           select new StudentItem
                           {
                               Id = u.Id,
                               FullName = u.FullName,
                               IsPresent = false // default - lecturer will mark
                           };

            Students = new ObservableCollection<StudentItem>(students.ToList());
        }

        private void SaveAttendance()
        {
            if (SelectedSchedule == null)
            {
                MessageBox.Show("Please select a schedule first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // **Placeholder**: implement actual attendance persistence if you have an Attendance table.
            var present = Students.Where(s => s.IsPresent).Select(s => s.FullName).ToList();
            string message = present.Count == 0 ? "No student marked present." :
                $"Marked present: {string.Join(", ", present)}";

            MessageBox.Show(message, "Attendance Saved (temporary)", MessageBoxButton.OK, MessageBoxImage.Information);

            // If you have an attendance table, insert/update here using _context and SaveChanges()
        }

        // Simple DTOs for binding
        public class ScheduleItem
        {
            public int ScheduleId { get; set; }
            public int CourseId { get; set; }
            public string CourseName { get; set; }
            public string DayOfWeek { get; set; }
            public string Slot { get; set; }
            public string Room { get; set; }
            public string Notes { get; set; }
        }

        public class StudentItem
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public bool IsPresent { get; set; }
        }
    }
}
