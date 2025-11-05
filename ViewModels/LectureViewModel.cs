using StudentManagement.Models;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    internal class LectureViewModel : BaseViewModel
    {
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(nameof(User)); }
        }
        public ICommand LogoutCommand { get; }
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public LectureViewModel()
        {
            User = UserSession.Instance.CurrentUser;
            LogoutCommand = new Helper.RelayCommand(Logout);
            text = "Welcome back, " + User.FullName + "!";
        }
        private void Logout(object obj)
        {
            UserSession.Instance.Clear();
            var loginWindow = new Views.Login();
            loginWindow.Show();
            foreach (var window in System.Windows.Application.Current.Windows)
            {
                if (window is Views.Home)
                {
                    (window as Views.Home).Close();
                }
            }
        }
    }
}
