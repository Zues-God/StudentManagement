using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    private readonly UserDAO _userDAO;
    public MainWindow()
    {
        InitializeComponent();
        _userDAO = new UserDAO();
    }

    public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string username = usernameTextBox.Text.Trim();       // TextBox nhập username
        string password = passwordBox.Password.Trim();       // PasswordBox nhập password

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        User user = _userDAO.Login(username, password);

        if (user != null && user.IsActive == true)
        {

            // Chuyển đến trang Home trong Frame
            Home home = new Home(user);
            home.Show();
            this.Close(); // Đóng cửa sổ đăng nhập
        }
        else
        {
            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản chưa được kích hoạt.", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void EnterPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            LoginButton_Click(this, new RoutedEventArgs());
        }
    }

    private void RegisterLink_Click(object sender, RoutedEventArgs e)
    {
        Login_Register.Register registerPage = new Login_Register.Register();
        registerPage.Show();
        this.Close(); // Đóng cửa sổ đăng nhập
    }

}
}
