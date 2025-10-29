using StudentManagement.ViewModels;
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

namespace StudentManagement.Views.Lecture
{
    /// <summary>
    /// Interaction logic for Lecture.xaml
    /// </summary>
    public partial class Lecture : Page
    {
        public Lecture()
        {
            InitializeComponent();
            this.DataContext = new LectureViewModel();
        }

        private void ToHome(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = null;
        }

        private void ToProfile(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new ProfilePage();
        }
    }
}
