using schoolManagerUser1.DAO;
using schoolManagerUser1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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

namespace schoolManagerUser1.View.AdminFeature
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>

    public partial class UserManagement : Page
    {
        private UserDAO userDAO = new UserDAO();

        public ObservableCollection<User> Users { get; set; }
        private ICollectionView UsersView;

        public UserManagement(User currentUser)
        {
            InitializeComponent();

            Users = new ObservableCollection<User>(userDAO.GetAllUsers());
            this.DataContext = this;

            // Tạo CollectionView để filter
            UsersView = CollectionViewSource.GetDefaultView(Users);
        }

        // Khi gõ vào textbox
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Khi nhấn Enter
        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            string searchText = SearchBox.Text.ToLower();

            UsersView.Filter = userObj =>
            {
                var user = userObj as User;
                return user != null && user.Username.ToLower().Contains(searchText);
            };

            UsersView.Refresh();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thêm user mới!");
            // Ở đây bạn có thể mở AddUserWindow
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button)?.DataContext as User;
            if (user == null) return;

            if (MessageBox.Show($"Bạn có chắc muốn xóa user '{user.Username}' không?",
                                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool result = userDAO.DeleteUser(user.Id);
                if (result)
                {
                    Users.Remove(user);
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
                else
                    MessageBox.Show("Xóa thất bại!", "Lỗi");
            }
        }
    }
}
