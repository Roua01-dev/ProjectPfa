using ProjectPfa.Models;
using ProjectPfa.View;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ProjectPfa.Data;

namespace ProjectPfa.ViewModel
{
    public class SignInViewModel : BaseViewModel
    {
        private Database _database;
        private string _email;
        private string _password;
        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public SignInViewModel()
        {
            _database = new Database();
            SignInCommand = new Command(async () => await SignInAsync());
            ForgotPasswordCommand = new Command(async () => await ForgotPasswordAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }



        private async Task SignInAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password.", "OK");
                    return;
                }

                var user = _database.GetUserByEmailAsync(Email).Result;
                if (user == null)
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Email not found. Please check your email.", "OK");
                    return;
                }

                if (user.Password != Password)
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Incorrect password. Please try again.", "OK");
                    return;
                }

                //App.Current.MainPage = new LandingPage(user.Username, user.ProfilePicturePath);
                App.Current.MainPage = new ProfilePage();
                //Application.Current.MainPage.DisplayAlert("Error", "tres bien.", "OK");

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            // Use Shell for navigation
            // await Shell.Current.GoToAsync("landing");
        }



        private async Task ForgotPasswordAsync()
        {
            // Navigate to ForgetPage
            // await Application.Current.MainPage.Navigation.PushAsync(new ForgetPage());
        }
    }
}
