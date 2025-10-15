using Microsoft.IdentityModel.Tokens;
using schoolManagerUser1.DAO;
using schoolManagerUser1.Models;
using schoolManagerUser1.Utils;
using System;
using System.Formats.Asn1;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace schoolManagerUser1.Login_Register
{
    public partial class Register : Window
    {
        UserDAO userDAO = new UserDAO();

        public Register()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

            string username = usernameTextBox.Text.Trim();
            string email = emailTextBox.Text.Trim();
            string password = passwordBox.Password.Trim();
            string fullName = fullNameTextBox.Text.Trim();
            string dob = dateOfBirthTextBox.Text.Trim();
            string role = ((ComboBoxItem)roleComboBox.SelectedItem).Content.ToString();
            string phone = phoneTextBox.Text.Trim();
            string address = addressTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
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
            if (!dob.IsNullOrEmpty()&&!UtilClass.IsValidDateOfBirth(dob)) { 
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

            User user = new User(username, password, email, role, fullName, dateOfBirth, phone, address);

            try
            {

                if (userDAO.AddUser(user))
                {
                    MessageBox.Show("Registered successfully!", "successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow login = new MainWindow();
                    login.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginLabel_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }
    }
}
