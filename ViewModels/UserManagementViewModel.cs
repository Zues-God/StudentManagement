using StudentManagement.Helper;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    internal class UserManagementViewModel : BaseViewModel
    {
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(nameof(User)); }
        }
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(nameof(Users)); }
        }
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {

                    Username = _selectedUser.Username;
                    Password = _selectedUser.Password;
                    Email = _selectedUser.Email;
                    FullName = _selectedUser.FullName;
                    DateOfBirth = _selectedUser.DateOfBirth;
                    Phone = _selectedUser.Phone;
                    Address = _selectedUser.Address;
                    Role = _selectedUser.Role ?? "student";
                    IsActive = _selectedUser.IsActive ?? true;
                }
                
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        private bool _isActive = true;
        public bool IsActive { get => _isActive;set { _isActive = value; OnPropertyChanged(nameof(IsActive)); } }
        private ObservableCollection<bool> _isActiveList = new ObservableCollection<bool> { true, false };
        public ObservableCollection<bool> IsActiveList
        {
            get => _isActiveList;
            set
            {
                _isActiveList = value;
                OnPropertyChanged(nameof(IsActiveList));
            }
        }
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
        private ObservableCollection<string> roles = new ObservableCollection<string> { "student", "lecturer" };

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));

            }
        }
        public ICommand SearchCommand => new RelayCommand(o => SearchUsers());
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public UserManagementViewModel()
        {
            LoadUsers();
            
            AddUserCommand = new RelayCommand(o => AddUser());
            EditUserCommand = new RelayCommand(o => EditUser(), o => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(o => DeleteUser(), o => SelectedUser != null);

        }

        private void LoadUsers()
        {
            //if (EditedUser == null) {
            //    EditedUser = new User();
            //    IsActive = true;
            //    Role = "student";
            //    OnPropertyChanged(nameof(EditedUser));
            //}
            
            using (var context = new AppDbContext())
            {
                var userList = context.Users.ToList();
                Users = new ObservableCollection<User>(userList);
                OnPropertyChanged(nameof(Users));
            }
          
        }

        private void AddUser()
        {


            if (Role is not "student" and not "lecturer")
            {
                MessageBox.Show("Role must be either 'student' or 'lecturer'!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
            User user = new User(Username, Password, Email, Role, FullName, DateOfBirth, Phone, Address);

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
                
                using (var context = new AppDbContext())
                {
                    
                    context.Users.Add(user);
                    context.SaveChanges();
                    LoadUsers();
                }
            }
        }

        private void EditUser()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select user to edit!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedUser.Role.ToLower().Equals("admin"))
            {
                MessageBox.Show("You cannot perform this action on the selected user!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Username) ||
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

                
                using (var context = new AppDbContext())
            {
                    if (SelectedUser.Username != Username && context.Users.Any(u => u.Username == Username))
                    {
                        MessageBox.Show("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (SelectedUser.Email != Email && context.Users.Any(u => u.Email == Email))
                    {
                        MessageBox.Show("Email already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var userInDb = context.Users.Find(SelectedUser.Id);
                if (userInDb != null)
                {
                    userInDb.Username = Username;
                    userInDb.Password = Password;
                    userInDb.Email = Email;
                    userInDb.FullName = FullName;
                    userInDb.DateOfBirth = DateOfBirth;
                    userInDb.Phone = Phone;
                    userInDb.Address = Address;
                    userInDb.Role = Role;
                    userInDb.IsActive = IsActive;
                    context.SaveChanges();
                    LoadUsers();
                }
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select user before delete!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedUser.Role.ToLower().Equals("admin"))
            {
                MessageBox.Show("You cannot perform this action on the selected user!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            using (var context = new AppDbContext())
            {
                var userInDb = context.Users.Find(SelectedUser.Id);
                if (userInDb != null)
                {
                    context.Users.Remove(userInDb);
                    context.SaveChanges();
                    LoadUsers();
                }
            }
        }


        private void SearchUsers()
        {
            using (var context = new AppDbContext())
            {
                var query = context.Users.AsQueryable();
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    query = query.Where(u => u.FullName.Contains(SearchText));
                }
                var filteredUsers = query.ToList();
                Users = new ObservableCollection<User>(filteredUsers);
                OnPropertyChanged(nameof(Users));
            }
        }
    }
}

