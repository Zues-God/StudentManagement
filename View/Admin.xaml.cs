using schoolManagerUser1.Models;
using schoolManagerUser1.View.AdminFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace schoolManagerUser1.View
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private User _currentUser;
        public Admin(User user)
        {
            _currentUser = user;
            String[] name = user.FullName.Split(" ");
            Message msg = new Message
            {
                Text = $"Hello {name[name.Length - 1]}!"
            };
            this.DataContext = msg;
            InitializeComponent();
            
        }

        private void MenuItem_Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void MenuItem_Profile_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new ProfilePage(_currentUser);
        }

        private void MenuItem_Home_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = null;
        }

        private void MenuItem_UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new UserManagement(_currentUser);
        }
    }
}
