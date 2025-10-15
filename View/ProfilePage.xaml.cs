using Microsoft.IdentityModel.Tokens;
using schoolManagerUser1.DAO;
using schoolManagerUser1.Models;
using schoolManagerUser1.Utils;
using System.Data;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace schoolManagerUser1.View
{
    public partial class ProfilePage : Page
    {
        private readonly UserDAO _userDAO = new UserDAO();
        private User currentUser;

        public ProfilePage(User user)
        {
            InitializeComponent();
            LoadProfile(user);
        }

        private void LoadProfile(User user)
        {
            currentUser = user;
            if (currentUser != null)
            {
                usernameTextBox.Text = currentUser.Username;
                emailTextBox.Text = currentUser.Email;
                fullNameTextBox.Text = currentUser.FullName;
                DOBTextBox.Text = currentUser.DateOfBirth?.ToString("yyyy-MM-dd") ?? "";
                phoneTextBox.Text = currentUser.Phone?.ToString() ?? "";
                addressTextBox.Text = currentUser.Address?.ToString() ?? "";

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (!oldPassTextBox.Text.ToString().Trim().Equals(currentUser.Password.ToString().Trim()))
            {
                MessageBox.Show("Current password is incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newPassTextBox.ToString().Equals(oldPassTextBox.ToString()))
            {
                MessageBox.Show("New password and confirm new password do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (currentUser != null)
            {
                string username = usernameTextBox.Text.Trim();
                string email = emailTextBox.Text.Trim();
                string password;
                if (!string.IsNullOrEmpty(newPassTextBox.Text.Trim()))
                {
                    password = newPassTextBox.Text.Trim();
                }
                else { 
                    password = currentUser.Password;
                }
                string fullName = fullNameTextBox.Text.Trim();
                string dob = DOBTextBox.Text.Trim();
                string role = currentUser.Role;
                string phone = phoneTextBox.Text.Trim();
                string address = addressTextBox.Text.Trim();

                if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Please fill in all required information!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!UtilClass.IsValidUsername(username))
                {
                    MessageBox.Show("Username must start with a letter. The following 2 to 15 characters can be letters, numbers, or underscores!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidPassword(password))
                {
                    MessageBox.Show("Password Must be at least 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidEmail(email))
                {
                    MessageBox.Show("Invalid email!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!UtilClass.IsValidFullName(fullName))
                {
                    MessageBox.Show("Full name: only letters and spaces allowed, minimum 2 words!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!dob.IsNullOrEmpty() && !UtilClass.IsValidDateOfBirth(dob))
                {
                    MessageBox.Show("Date of Birth: format yyyy-MM-dd and valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!phone.IsNullOrEmpty() && !UtilClass.IsValidPhone(phone))
                {
                    MessageBox.Show("Phone number: starts with 0, has 10 or 11 digits!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!address.IsNullOrEmpty() && !UtilClass.IsValidAddress(address))
                {
                    MessageBox.Show("Address: allows letters, numbers, commas, periods, spaces, minimum 5 characters!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                dynamic dateOfBirth = dob.IsNullOrEmpty() ? null : DateTime.Parse(dob);

                currentUser = new User(username, password, email, role, fullName, dateOfBirth, phone, address);
                Home home = new Home(currentUser);
                home.Show();
                Window.GetWindow(this).Close();

                bool success = _userDAO.UpdateUser(currentUser);
                if (success)
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Không có thay đổi nào được lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
