using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Windows;

namespace StudentManagement.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public LoginViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password";
                return;
            }

            ErrorMessage = string.Empty;
            var user = await _authService.LoginAsync(Username, Password);

            if (user != null)
            {
                CurrentUser.Instance.SetUser(user);
                _navigationService.NavigateToDashboard(user.Role);
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateToRegister();
        }
    }
}

