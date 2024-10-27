using ProjectPfa.Data;
using ProjectPfa.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProjectPfa.ViewModel
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly Database _database;
        private readonly Page _page;

        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _mobileNumber;
        private string _profilePicturePath;

        public Command CreateAccountClicked { get; }
        public Command UploadProfilePictureCommand { get; }

        public AddUserViewModel() // Passing Page instance for alerts
        {
            _database = new Database();
            CreateAccountClicked = new Command(async () => await CreateAccountClickedAsync());
            UploadProfilePictureCommand = new Command(async () => await UploadProfilePictureAsync());
        }

        public ImageSource ProfilePicture
        {
            get
            {
                if (string.IsNullOrEmpty(ProfilePicturePath))
                    return null;

                return ImageSource.FromFile(ProfilePicturePath);
            }
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string MobileNumber
        {
            get => _mobileNumber;
            set => SetProperty(ref _mobileNumber, value);
        }

        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set => SetProperty(ref _profilePicturePath, value);
        }

        private async Task UploadProfilePictureAsync()
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
                OnPropertyChanged(nameof(ProfilePicture)); // Notify the UI of the change
            }
        }

        private async Task CreateAccountClickedAsync()
        {
            if (IsInputValid())
            {
                // Check if email already exists
                var existingUser = await _database.GetUserByEmailAsync(Email);
                if (existingUser != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Email is already in use. Please choose a different email.", "OK");
                    return;
                }

                var newUser = new User
                {
                    Username = Username,
                    Email = Email,
                    Password = Password, // Ensure password is hashed if needed
                    ConfirmPassword = ConfirmPassword,
                    MobileNumber = MobileNumber,
                    ProfilePicturePath = ProfilePicturePath
                };

                await _database.SaveUserAsync(newUser);

                // Show success message
                await Application.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
            }
            else
            {
                // Handle validation errors
                await Application.Current.MainPage.DisplayAlert("Error", "Please check your input. Make sure all fields are filled correctly and passwords match.", "OK");
            }
        }

        private bool IsInputValid()
        {
            // Ensure all fields are filled
            if (string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) ||
                Password != ConfirmPassword)
            {
                return false;
            }

            // You can add more validations here if needed

            return true;
        }
    }
}
