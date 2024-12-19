using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Media;
using ProjectPfa.Models;
using ProjectPfa.Data;

namespace ProjectPfa.ViewModel
{
    public class UpdateUserViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _mobileNumber;
        public string MobileNumber
        {
            get => _mobileNumber;
            set => SetProperty(ref _mobileNumber, value);
        }

        private string _profilePicturePath;
        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set => SetProperty(ref _profilePicturePath, value);
        }

        private User _selectedUser;

        public Command UpdateAccountClicked { get; }
        public ICommand UploadProfilePictureCommand { get; }

        public UpdateUserViewModel(User user)
        {
            _selectedUser = user;

            // Initialize properties with user data
            Username = _selectedUser.Username;
            Email = _selectedUser.Email;
            MobileNumber = _selectedUser.MobileNumber;
            ProfilePicturePath = _selectedUser.ProfilePicturePath;

            // Initialize commands
            UpdateAccountClicked = new Command(async () => await OnUpdateAccountClicked());
            UploadProfilePictureCommand = new Command(async () => await UploadProfilePicture());
        }

        private async Task OnUpdateAccountClicked()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            // Check if the email has been changed and is already in use
            if (Email != _selectedUser.Email)
            {
                var existingUser = await DatabaseService.GetUserByEmailAsync(Email);
                if (existingUser != null && existingUser.Id != _selectedUser.Id)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This email address is already in use.", "OK");
                    return;
                }
            }

            // Update the user details
            _selectedUser.Username = Username;
            _selectedUser.Email = Email;
            _selectedUser.MobileNumber = MobileNumber;
            _selectedUser.ProfilePicturePath = ProfilePicturePath;

            // Update the user in the database
            int rowsAffected = await DatabaseService.UpdateUserAsync(_selectedUser);

            if (rowsAffected > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "User information has been updated successfully.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating user information.", "OK");
            }
        }

        private async Task UploadProfilePicture()
        {
            try
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
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Unable to pick photo: {ex.Message}", "OK");
            }
        }
    }
}
