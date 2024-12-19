using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectPfa.Data;
using ProjectPfa.Models;
using ProjectPfa.View;

namespace ProjectPfa.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _mobileNumber;
        private string _username;
        private string _profilePicturePath;

        public SignUpViewModel()
        {
            SignUpCommand = new Command(async () => await OnSignUp());
            SignInCommand = new Command(OnSignIn);
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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string MobileNumber
        {
            get => _mobileNumber;
            set
            {
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand SignUpCommand { get; }
        public ICommand SignInCommand { get; }
        public ICommand UploadProfilePictureCommand => new Command(async () => await UploadProfilePicture());



        private async Task UploadProfilePicture()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {

                ProfilePicturePath = result.FullPath;
                Debug.WriteLine(" ProfilePicturePath", ProfilePicturePath);

            }
            else
            {
                Debug.WriteLine("No image was selected.");
            }
        }

        private async Task OnSignUp()
        {
            // Validate input
            if (!IsValidEmail(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            if (!IsValidPhoneNumber(MobileNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid phone number.", "OK");
                return;
            }

            //if (!IsStrongPassword(Password))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long and contain a mix of letters, numbers, and special characters.", "OK");
            //    return;
            //}


            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "password.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(ProfilePicturePath))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must upload a profile picture.", "OK");
                return;
            }

            // Create a new user object
            var user = new User
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                MobileNumber = MobileNumber,
                Username = Username,
                ProfilePicturePath = ProfilePicturePath
            };

            // Add the user to the database
            int result = await DatabaseService.AddUserAsync(user);

            if (result > 0)
            {
                Debug.WriteLine($"User {Email} created successfully!");

                // Navigate to SignIn page
                App.Current.MainPage = new SignIn();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to create account. Please try again.", "OK");
            }
        }

        private void OnSignIn()
        {
            // Navigate to the SignIn page
            App.Current.MainPage = new SignIn();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^[0-9]{8}$");
        }

        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"[a-z]") && Regex.IsMatch(password, @"[0-9]") &&
                   Regex.IsMatch(password, @"[\W]");
        }
    }
}
