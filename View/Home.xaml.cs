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
using System.Windows.Shapes;

namespace StudentManagement.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public class Message
    {
        public string Text { get; set; }
    }
    public partial class Home : Window
    {
        public Home(User user)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            if (user.Role == "admin")
                HomeFrame.Content = new Admin(user);
            if (user.Role == "student")
                HomeFrame.Content = new Student(user);
            if (user.Role == "lecture")
                HomeFrame.Content = new Lecture(user);
        }
    }
}

