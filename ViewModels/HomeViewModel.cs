using StudentManagement.Models;
using StudentManagement.Views.Admin;
using StudentManagement.Views.Lecture;
using StudentManagement.Views.student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    internal class HomeViewModel : BaseViewModel
    {
   
        private object _contentFrame;
        public object ContentFrame
        {
            get { return _contentFrame; }
            set { _contentFrame = value; OnPropertyChanged(nameof(ContentFrame)); }
        }
        private User user = UserSession.Instance.CurrentUser;

        public HomeViewModel() {
            if (user.Role == "admin")
                ContentFrame = new Admin();
            if (user.Role == "student")
                ContentFrame = new Student();
            if (user.Role == "lecturer")
                ContentFrame = new Lecture();
        }
    }
}
