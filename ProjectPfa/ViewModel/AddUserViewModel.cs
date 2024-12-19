using ProjectPfa.Data;
using ProjectPfa.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProjectPfa.ViewModel
{
    public class AddUserViewModel : BaseViewModel
    {
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _mobileNumber;
        private string _profilePicturePath;

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

        public ICommand CreateAccountCommand { get; }
        public ICommand UploadProfilePictureCommand { get; }

        public AddUserViewModel()
        {
            CreateAccountCommand = new Command(async () => await CreateAccountAsync());
            UploadProfilePictureCommand = new Command(async () => await UploadProfilePictureAsync());
        }

        private async Task CreateAccountAsync()
        {
            if (IsInputValid())
            {
                try
                {
                    var newUser = new User
                    {
                        Username = Username,
                        Email = Email,
                        Password = Password, // Hash password in DatabaseService
                        MobileNumber = MobileNumber,
                        ProfilePicturePath = ProfilePicturePath
                    };

                    // Add user to the database
                    var result = await DatabaseService.AddUserAsync(newUser);

                    if (result > 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
                        await Application.Current.MainPage.Navigation.PopAsync(); // Navigate back
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to create account. Please try again.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields correctly. Make sure passwords match.", "OK");
            }
        }

        private async Task UploadProfilePictureAsync()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a Profile Picture"
            });

            if (result != null)
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                using (var stream = await result.OpenReadAsync())
                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                ProfilePicturePath = filePath;
                OnPropertyChanged(nameof(ProfilePicturePath));
            }
        }

        private bool IsInputValid()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password == ConfirmPassword;
        }
    }
}
