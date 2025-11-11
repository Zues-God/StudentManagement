using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace StudentManagement.ViewModels
{
    public class StudentScheduleViewModel : BaseViewModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        private ObservableCollection<object> _studentSchedules;
        public ObservableCollection<object> StudentSchedules
        {
            get => _studentSchedules;
            set { _studentSchedules = value; OnPropertyChanged(nameof(StudentSchedules)); }
        }

        private int _studentId;
        public int StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                OnPropertyChanged(nameof(StudentId));
                LoadStudentSchedules(); // 🔄 Tự động load lại khi đổi student
            }
        }

        public StudentScheduleViewModel(int studentId)
        {
            StudentId = studentId;
        }

        private void LoadStudentSchedules()
        {
   
            var query = from cp in _context.CourseParticipants
                        join c in _context.Courses on cp.CourseId equals c.Id
                        join s in _context.Schedules on c.Id equals s.CourseId
                        join sl in _context.Slots on s.SlotId equals sl.Id
                        where cp.UserId == StudentId && cp.RoleInCourse == "student"
                        select new
                        {
                            CourseName = c.Name,
                            s.DayOfWeek,
                            Slot = $"Slot {sl.SlotNumber}",
                            s.Room,
                            s.Notes
                        };

            StudentSchedules = new ObservableCollection<object>(query.ToList());
        }

    }
}
