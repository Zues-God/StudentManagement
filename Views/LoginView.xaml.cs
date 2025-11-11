using System.Windows;
using System.Windows.Controls;
using StudentManagement.Services;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            var dbService = new DatabaseService();
            var authService = new AuthService(dbService);
            var navigationService = new NavigationService();
            DataContext = new LoginViewModel(authService, navigationService);
            
            Loaded += (s, e) => WindowState = WindowState.Maximized;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }
    }
}

