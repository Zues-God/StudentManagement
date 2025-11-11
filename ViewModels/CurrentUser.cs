using StudentManagement.Models;

namespace StudentManagement.ViewModels
{
    public class CurrentUser
    {
        private static CurrentUser? _instance;
        private User? _user;

        private CurrentUser() { }

        public static CurrentUser Instance
        {
            get
            {
                _instance ??= new CurrentUser();
                return _instance;
            }
        }

        public User? User => _user;

        public void SetUser(User user)
        {
            _user = user;
        }

        public void Clear()
        {
            _user = null;
        }
    }
}

