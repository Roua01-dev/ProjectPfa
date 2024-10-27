using ProjectPfa.Models;
using ProjectPfa.Data;
using System.Collections.ObjectModel;
using System.Linq;
using ProjectPfa.ViewModel;
using ProjectPfa.Models;

namespace ProjectPfa.View
{
    public partial class UserManagementPage : ContentPage
    {
        private Database _database;
        private readonly UserManagementViewModel _viewModel;

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public UserManagementPage()
        {
            InitializeComponent();
            _database = new Database();
            LoadUsers();
            _viewModel= BindingContext as UserManagementViewModel;
        }

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

        private async void OnAddUserClicked (object sender ,EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage());
        }

        private async void LoadUsers()
        {
            var users = await _database.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
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

        private void OnSelectAllCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Access the ViewModel and update the "IsSelectAllChecked" property
            if (BindingContext is UserManagementViewModel viewModel)
            {
             //   viewModel.IsSelectAllChecked = e.Value;
            }
        }
    }
}
