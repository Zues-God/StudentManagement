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
using StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using System.Windows;
using StudentManagement.ViewModels;

namespace StudentManagement.Views.Lecturer
{
    public partial class LecturerSchedule : Window
    {
        public LecturerSchedule(int lecturerId)
        {
            InitializeComponent();
            DataContext = new LecturerScheduleViewModel(lecturerId);
        }
    }
}
