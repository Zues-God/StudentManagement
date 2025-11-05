using System.Windows;

namespace StudentManagement
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // CHỌN MODULE CẦN TEST
            var win = new Views.Admin.ScheduleManagement();   // Admin test
            //var win = new Views.Student.StudentSchedule();     // Student test
            //var win = new Views.Lecturer.LecturerSchedule();     // Lecturer test

            win.Show();
        }
    }
}
