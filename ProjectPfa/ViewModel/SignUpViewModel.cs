using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectPfa.Data;
using ProjectPfa.Models;
using ProjectPfa.Services;
using ProjectPfa.View;

namespace ProjectPfa.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private Database _database;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _mobileNumber;
        private string _username;
        private string _profilePicturePath;
        private ObservableCollection<string> _countries;

        public SignUpViewModel()
        {
            _database = new Database();
            SignUpCommand = new Command(async () => await OnSignUp());
            SignInCommand = new Command(OnSignIn);

            // Load countries
            _countries = new ObservableCollection<string> { "TN" }; // Simplified for example
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

        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignUpCommand { get; }
        public ICommand SignInCommand { get; }
        public ICommand UploadProfilePictureCommand => new Command(async () => await UploadProfilePicture());


        private async Task UploadProfilePicture()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var filePath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                using (var fileStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                ProfilePicturePath = filePath;
            }
        }

        private async Task OnSignUp()
        {
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

            if (!IsStrongPassword(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long and contain a mix of letters, numbers, and special characters.", "OK");
                return;
            }

            if (await IsEmailInUse(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email is already in use.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(ProfilePicturePath))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must upload a profile picture.", "OK");
                return;
            }

            var user = new User
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                MobileNumber = MobileNumber,
                Username = Username,
                ProfilePicturePath = ProfilePicturePath
            };

            await _database.SaveUserAsync(user);
            Debug.WriteLine($"User {Email} created successfully!");

            // Navigate to SignIn page
            App.Current.MainPage = new SignIn();
           // await Application.Current.MainPage.Navigation.PushAsync(new SignIn());
        }

        private void OnSignIn()
        {
            // Logic for navigating to the SignIn page
             App.Current.MainPage = new SignIn();
            // Application.Current.MainPage.Navigation.PushAsync(new SignIn());
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

        private async Task<bool> IsEmailInUse(string email)
        {
            var user = await _database.GetUserByEmailAsync(email);
            return user != null;
        }
    }
}
