using ProjectPfa.Models;
using ProjectPfa.View;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ProjectPfa.Data;
using static ProjectPfa.Data.DatabaseService;

namespace ProjectPfa.ViewModel
{
    public class SignInViewModel : BaseViewModel
    {
        private string _email;
        private string _password;

        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public SignInViewModel()
        {
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
            // Vérifiez si les champs sont vides
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email cannot be empty.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password cannot be empty.", "OK");
                return;
            }

            // Vérifiez les informations d'identification de l'utilisateur
            var user = await DatabaseService.VerifyUserAsync(Email, Password);

            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password.", "OK");
                return;
            }
            // Enregistrer l'utilisateur connecté dans la session
            UserSession.LoginUser(user.Id);

            // Si tout est correct, naviguez vers la LandingPage
            App.Current.MainPage = new LandingPage();
        }



        private async Task ForgotPasswordAsync()
        {
            // Naviguer vers la page de récupération du mot de passe
           // await Application.Current.MainPage.Navigation.PushAsync(new ForgetPage());
        }


    }
}
