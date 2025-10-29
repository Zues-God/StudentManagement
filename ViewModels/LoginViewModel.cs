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

namespace StudentManagement.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        public ICommand ToRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            ToRegisterCommand = new RelayCommand((o) =>
            {
                var registerWindow = new Views.Register();
                registerWindow.Show();
                foreach (var window in System.Windows.Application.Current.Windows)
                {
                    if (window is Views.Login)
                    {
                        (window as Views.Login).Close();
                    }
                }
            });
        }
        public ICommand LoginCommand { get; }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value.Trim();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value.Trim();
            }
        }
        private void Login(object obj)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.Username.Trim().ToString().Equals(UserName.Trim().ToString()) && u.Password.Trim().ToString().Equals(Password.Trim().ToString()));
                    UserSession.Instance.CurrentUser = (User) user;
                    if (UserSession.Instance.CurrentUser == null)
                    {
                        throw new Exception("Invalid username or password.");
                    return;
                    }
                if (UserSession.Instance.CurrentUser.IsActive == false) {
                        MessageBox.Show("This account is currently inactive.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }
                    var home = new Views.Home();
                    home.Show();

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
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
