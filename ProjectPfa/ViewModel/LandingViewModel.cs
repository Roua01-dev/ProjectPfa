using ProjectPfa.Models;
using ProjectPfa.Data;
using static ProjectPfa.Data.DatabaseService;
using System.Diagnostics;
using System.Windows.Input;
using ProjectPfa.View;

namespace ProjectPfa.ViewModel
{
    public class LandingViewModel : BaseViewModel
    {
        private string _username;
        private string _profilePicturePath;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set
            {
                _profilePicturePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }

        public LandingViewModel()
        {
            Username = "Roua youneb";
            ProfilePicturePath = "default_profile.png";
            LoadCurrentUser();
            LogoutCommand = new Command(Logout);
        }

        private async Task LoadCurrentUser()
        {
            if (UserSession.CurrentUserId == null)
            {
                Username = "Roua youneb";
                ProfilePicturePath = "default_profile.png";
                return;
            }

            var currentUser = await DatabaseService.GetUserByIdAsync(UserSession.CurrentUserId.Value);

            if (currentUser != null)
            {
                Username = currentUser.Username;
                ProfilePicturePath = string.IsNullOrEmpty(currentUser.ProfilePicturePath)
                    ? "default_profile.png"
                    : currentUser.ProfilePicturePath;
            }
            else
            {
                Username = "Roua youneb";
                ProfilePicturePath = "default_profile.png";
            }
        }

        private void Logout()
        {
            // Clear session
            DatabaseService.UserSession.LogoutUser();

            // Navigate to the login page
            Application.Current.MainPage = new NavigationPage(new SignIn());
        }
    }
}
