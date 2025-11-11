using Microsoft.EntityFrameworkCore;
using StudentManagement.Helper;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Helper;
using StudentManagement.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        public ICommand ToRegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            ToRegisterCommand = new RelayCommand((o) =>
            {
                var registerWindow = new Views.Register();
                registerWindow.Show();

                // Đóng cửa sổ Login hiện tại
                foreach (var window in Application.Current.Windows)
                {
                    if (window is Views.Login)
                    {
                        (window as Views.Login).Close();
                        break;
                    }
                }
            });
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => _userName = value?.Trim();
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }

        private void Login(object obj)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                try
                {
                    var user = db.Users
                        .FirstOrDefault(u =>
                            u.Username.Trim().Equals(UserName, StringComparison.OrdinalIgnoreCase) &&
                            u.Password.Trim().Equals(Password));

                    if (user == null)
                    {
                        MessageBox.Show("Invalid username or password.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (user.IsActive == false)
                    {
                        MessageBox.Show("This account is currently inactive.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    UserSession.Instance.CurrentUser = user;

                    // ✅ Điều hướng theo Role
                    Window targetWindow = null;
                    switch (user.Role)
                    {
                        case "admin":
                            targetWindow = new Views.Admin.ScheduleManagement();
                            break;

                        case "student":
                            targetWindow = new Views.Student.StudentSchedule(user.Id);
                            break;

                        case "lecturer":
                            targetWindow = new Views.Lecturer.LecturerSchedule(user.Id);
                            break;

                        default:
                            MessageBox.Show("Unknown user role.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    targetWindow.Show();

                    // 🔒 Đóng cửa sổ Login sau khi mở giao diện chính
                    foreach (var window in Application.Current.Windows)
                    {
                        if (window is Views.Login)
                        {
                            (window as Views.Login).Close();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error logging in: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
