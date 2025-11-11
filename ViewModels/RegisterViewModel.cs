using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentManagement.Services;
using System.Windows;

namespace StudentManagement.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private string fullName = string.Empty;

        [ObservableProperty]
        private string selectedRole = "student";

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string successMessage = string.Empty;

        public List<string> Roles { get; } = new() { "student", "lecture", "admin" };

        public RegisterViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task Register()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Please fill in all required fields";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters";
                return;
            }

            var success = await _authService.RegisterAsync(Username, Email, Password, SelectedRole, FullName);

            if (success)
            {
                SuccessMessage = "Registration successful! You can now login.";
                await Task.Delay(2000);
                _navigationService.NavigateToLogin();
            }
            else
            {
                ErrorMessage = "Registration failed. Username or email may already exist.";
            }
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationService.NavigateToLogin();
        }
    }
}

