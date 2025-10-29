using StudentManagement.Helper;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    internal class ProfilePageViewModel : BaseViewModel
    {
        private readonly User curUser = UserSession.Instance.CurrentUser;
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value; OnPropertyChanged(nameof(User));
            }
        }
        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value.Trim();
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string _curPassword;
        public string CurPassword
        {
            get => _curPassword;
            set
            {
                _curPassword = value.Trim();
                OnPropertyChanged(nameof(CurPassword));
            }
        }
        private DateTime? _dob;
        public DateTime? DateOfBirth
        {
            get => _dob;
            set
            {
                _dob = value; OnPropertyChanged(nameof(DateOfBirth));
            }
        }
        public ICommand UpdateProfileCommand { get; }
        public ProfilePageViewModel()
        {
            User = curUser;
            UpdateProfileCommand = new RelayCommand(UpdateProfile);
            DateOfBirth = curUser.DateOfBirth;
        }

        private void UpdateProfile(object obj)
        {
            try
            {
                if (_curPassword != curUser.Password)
                {
                    MessageBox.Show("Current password is incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                string paswd;
                if (_newPassword != null)
                {
                    if (!UtilClass.IsValidPassword(_newPassword))
                    {
                        MessageBox.Show("New Password Must be at least 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    paswd = _newPassword;
                }
                else
                {
                    paswd = curUser.Password;
                }


                if (string.IsNullOrEmpty(_user.Username) ||
                string.IsNullOrEmpty(_user.Email) || string.IsNullOrEmpty(_user.FullName))
                {
                    MessageBox.Show("Please fill in all required information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!UtilClass.IsValidUsername(_user.Username))
                {
                    MessageBox.Show("Username must start with a letter. The following 2 to 15 characters can be letters, numbers, or underscores!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidEmail(_user.Email))
                {
                    MessageBox.Show("Invalid email!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidFullName(_user.FullName))
                {
                    MessageBox.Show("Full name: only letters and spaces allowed, minimum 2 words!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(_user.Phone) && !UtilClass.IsValidPhone(_user.Phone))
                {
                    MessageBox.Show("Phone number: starts with 0, has 10 or 11 digits!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(_user.Address) && !UtilClass.IsValidAddress(_user.Address))
                {
                    MessageBox.Show("Address: allows letters, numbers, commas, periods, spaces, minimum 5 characters!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new AppDbContext())
                {
                    if (User.Username != curUser.Username && db.Users.Any(u => u.Username == User.Username))
                    {
                        MessageBox.Show("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (User.Email != curUser.Email && db.Users.Any(u => u.Email == User.Email))
                    {
                        MessageBox.Show("Email already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var userInDb = db.Users.FirstOrDefault(u => u.Id == curUser.Id);
                    if (userInDb != null)
                    {
                        userInDb.Username = _user.Username ?? "";
                        userInDb.FullName = _user.FullName ?? "";
                        userInDb.Email = _user.Email ?? "";
                        userInDb.DateOfBirth = DateOfBirth;
                        userInDb.Phone = User.Phone ?? "";
                        userInDb.Address = User.Address ?? "";
                        userInDb.Password = paswd;
                        userInDb.UpdatedAt = DateTime.Now;
                        db.SaveChanges();
                        MessageBox.Show("Profile updated successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Update the current user session
                        UserSession.Instance.CurrentUser = null;
                        UserSession.Instance.CurrentUser = (User)userInDb;
                        CurPassword = "";
                        OnPropertyChanged(nameof(CurPassword));
                        OnPropertyChanged(nameof(User));
                        var refreshPage = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                        if (refreshPage is Views.Home home)
                        {
                            if (UserSession.Instance.CurrentUser.Role == "student")
                            {
                                home.HomeFrame.Navigate(new Views.student.Student());
                            }
                            else if (UserSession.Instance.CurrentUser.Role == "admin")
                            {
                                home.HomeFrame.Navigate(new Views.Admin.Admin());
                            }
                            home.HomeFrame.NavigationService.RemoveBackEntry();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                System.Windows.MessageBox.Show("Error updating profile: " + ex.Message);
            }
        }
    }
}
