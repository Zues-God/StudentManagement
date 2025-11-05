using System.Windows;
using System.Windows.Input;
using StudentManagement.ViewModels;

namespace StudentManagement.Views
{
    public partial class Register : Window
    {

        public Register()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel();
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

        private void LoginLabel_Click(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
