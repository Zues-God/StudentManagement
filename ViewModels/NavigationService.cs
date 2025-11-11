using System.Windows;

namespace StudentManagement.ViewModels
{
    public class NavigationService
    {
        public void NavigateToLogin()
        {
            var loginWindow = new Views.LoginView();
            loginWindow.Show();
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = loginWindow;
        }

        public void NavigateToRegister()
        {
            var registerWindow = new Views.RegisterView();
            registerWindow.Show();
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = registerWindow;
        }

        public void NavigateToDashboard(string role)
        {
            Window? dashboard = role switch
            {
                "admin" => new Views.AdminDashboardView(),
                "lecture" => new Views.LecturerDashboardView(),
                "student" => new Views.StudentDashboardView(),
                _ => null
            };

            if (dashboard != null)
            {
                dashboard.Show();
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = dashboard;
            }
        }
    }
}

