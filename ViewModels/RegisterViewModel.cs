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
    internal class RegisterViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }
        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        private string _role = "student";
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public List<string> Roles { get; } = new List<string> { "student", "lecturer" };
        public ICommand RegisterCommand { get; }
        public RegisterViewModel()
        {
           RegisterCommand = new RelayCommand(Register);
        }
        private void Register(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(FullName))
                {
                    MessageBox.Show("Please fill in all required information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!UtilClass.IsValidUsername(Username))
                {
                    MessageBox.Show("Username must start with a letter. The following 2 to 15 characters can be letters, numbers, or underscores!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidPassword(Password))
                {
                    MessageBox.Show("Password Must be at least 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Password and Confirm Password do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidEmail(Email))
                {
                    MessageBox.Show("Invalid email!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidFullName(FullName))
                {
                    MessageBox.Show("Full name: only letters and spaces allowed, minimum 2 words!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(Phone) && !UtilClass.IsValidPhone(Phone))
                {
                    MessageBox.Show("Phone number: starts with 0, has 10 or 11 digits!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(Address) && !UtilClass.IsValidAddress(Address))
                {
                    MessageBox.Show("Address: allows letters, numbers, commas, periods, spaces, minimum 5 characters!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Username = Username ?? "";
                Password = Password ?? "";
                Email = Email ?? "";
                FullName = FullName ?? "";
                Phone = Phone ?? "";
                Address = Address ?? "";
                User user = new User
                {
                    Username = Username,
                    Password = Password,
                    Email = Email,
                    Role = Role,
                    FullName = FullName,
                    DateOfBirth = DateOfBirth,
                    Phone = Phone,
                    Address = Address,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };


                using (var db = new AppDbContext())
                {
                    if (db.Users.Any(u => u.Username == Username))
                    {
                        MessageBox.Show("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (db.Users.Any(u => u.Email == Email))
                    {
                        MessageBox.Show("Email already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                    MessageBox.Show("Registration successful!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                var loginWindow = new Views.Login();
                loginWindow.Show();
                var registerWindow = Application.Current.Windows.OfType<Views.Register>().FirstOrDefault();
                registerWindow?.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed! Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
