using ProjectPfa.ViewModel;
using ProjectPfa.Models;
using ProjectPfa.Data;
using System;
using System.Collections.ObjectModel;
 
namespace ProjectPfa.View
{
    public partial class UserManagementPage : ContentPage
    {
        private readonly UserManagementViewModel _viewModel;
        private DatabaseService _database;

        public UserManagementPage()
        {
            InitializeComponent();
            _database = new DatabaseService();

            _viewModel = new UserManagementViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Load users when the page appears
            await _viewModel.LoadUsersAsync();
        }

       

        private async void UpdateUserCommand(object sender, EventArgs e)
        {
            // Get the user associated with the clicked button
            if (sender is Button button && button.BindingContext is User user)
            {
                // Navigate to Update User Page, passing the selected user
                await Navigation.PushAsync(new UpdateUserPage(user));
            }
        }

        //private async void DeleteUserCommand(object sender, EventArgs e)
        //{
        //    if (sender is Button button && button.BindingContext is User user)
        //    {
        //        // Confirm deletion
        //        bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {user.Username}?", "Yes", "No");
        //        if (confirmDelete)
        //        {
        //            await _viewModel.DeleteUserAsync(user);
        //        }
        //    }
        //}

        //private void OnSelectAllCheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    // Update the Select All state in the ViewModel
        //    _viewModel.SelectAllUsers(e.Value);
        //}



        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var user = button?.BindingContext as User;
            if (user != null)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {user.Username}?", "Yes", "No");
                if (confirmDelete)
                {
                    await _viewModel.DeleteUser(user);
                }
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            // Navigate to AddUserProperty page
            await Navigation.PushAsync(new AddUserPage());
        }



        private async void OnUpdateUserClicked(object sender, EventArgs e)
        {
            // Get the user associated with the clicked button
            var button = sender as Button;
            var user = button?.BindingContext as User;

            if (user != null)
            {
                // Navigate to the UpdateUserPage, passing the selected user as a parameter
                await Navigation.PushAsync(new UpdateUserPage(user));
            }
        }


    }
}
