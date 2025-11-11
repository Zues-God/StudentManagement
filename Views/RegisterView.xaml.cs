using System.Windows;
using System.Windows.Controls;
using StudentManagement.Services;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            var dbService = new DatabaseService();
            var authService = new AuthService(dbService);
            var navigationService = new NavigationService();
            DataContext = new RegisterViewModel(authService, navigationService);
            
            Loaded += (s, e) => WindowState = WindowState.Maximized;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.ConfirmPassword = passwordBox.Password;
            }
        }
    }
}

