using StudentManagement.Models;
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

namespace StudentManagement.Views.Admin
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.AdminViewModel();
        }

        private void MenuItem_Home_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = null;
        }

        private void MenuItem_Profile_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new ProfilePage();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new UserManagement();
        }
    }
}
