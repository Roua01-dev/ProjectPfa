using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPfa.Models;
using ProjectPfa.Data;
using System.Windows.Input;

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
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }
        private Database _database;

        public Command UpdateAccountClicked { get; }
        public ICommand UploadProfilePictureCommand => new Command(async () => await UploadProfilePicture()); public UpdateUserViewModel()
        {
            // Constructeur par défaut requis pour l'instanciation dans le XAML
        }
        public UpdateUserViewModel(User user):this()
        {
            SelectedUser = user;

            // Initialiser les propriétés avec les données de l'utilisateur
            Username = SelectedUser.Username;
            Email = SelectedUser.Email;
            MobileNumber = SelectedUser.MobileNumber;
            ProfilePicturePath = SelectedUser.ProfilePicturePath;

            // Initialiser les commandes
            UpdateAccountClicked = new Command(OnUpdateAccountClicked);
          //  UploadProfilePictureCommand = new Command(OnUploadProfilePicture);

            // Initialiser l'instance de la base de données
            _database = new Database();
        }

        private async void OnUpdateAccountClicked()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            // Initialize the database instance
            var database = new Database();

            // Check if the email has been changed
            if (Email != SelectedUser.Email)
            {
                // Check if the new email is already in use by another user
                var existingUser = await database.GetUserByEmailAsync(Email);
                if (existingUser != null && existingUser.Id != SelectedUser.Id)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This email address is already in use.", "OK");
                    return;
                }
            }

            // Update the selected user
            SelectedUser.Username = Username;
            SelectedUser.Email = Email;
            SelectedUser.MobileNumber = MobileNumber;
            SelectedUser.ProfilePicturePath = ProfilePicturePath;

            // Update the user in the database
            int rowsAffected = await database.UpdateUserAsync(SelectedUser);

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




    }

}
