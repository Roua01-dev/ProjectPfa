using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectPfa.Data;
using ProjectPfa.Models;
using ProjectPfa.View;

namespace ProjectPfa.ViewModel
{
    public class UserManagementViewModel : ObservableObject
    {
        // Observable collection for users
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        private ObservableCollection<User> _allUsers = new ObservableCollection<User>();



        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchUsers();
            }
        }

        private bool _isSelectAllChecked;
        public bool IsSelectAllChecked
        {
            get => _isSelectAllChecked;
            set
            {
                if (SetProperty(ref _isSelectAllChecked, value))
                {
                    SelectAllUsers(value);
                }
            }
        }

        // Commands
        public ICommand RefreshCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SearchCommand { get; }

        public UserManagementViewModel()
        {
            // Initialize commands
            RefreshCommand = new AsyncRelayCommand(RefreshUsersAsync);
            AddUserCommand = new RelayCommand(OnAddUser);
            // UpdateUserCommand = new RelayCommand<User>(OnUpdateUser);
            DeleteUserCommand = new Command<User>(async (user) => await DeleteUser(user));
            SearchCommand = new RelayCommand<string>(query => SearchText = query);

            // Load users initially
            Task.Run(() => LoadUsersAsync());
        }
        public async Task DeleteUser(User user)
        {
            if (user == null)
            {
                return;
            }

            await DatabaseService.DeleteUserAsync(user.Id);
            _allUsers.Remove(user);
            Users.Remove(user);
        }

        private async void OnUpdateUser(User user)
        {
            if (user != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UpdateUserPage(user));
            }
        }

        // Load users from database
        public async Task LoadUsersAsync()
        {
            IsBusy = true;
            try
            {
                var users = await DatabaseService.GetAllUsersAsync();
                Users.Clear();
                _allUsers.Clear();

                foreach (var user in users)
                {
                    _allUsers.Add(user);
                    Users.Add(user);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Refresh the user list
        public async Task RefreshUsersAsync()
        {
            await LoadUsersAsync();
        }

    

        // Navigate to Add User Page
        private void OnAddUser()
        {
            // Example: Implement navigation logic to AddUserPage here
        }

        // Navigate to Update User Page
      

        // Search users
        private void SearchUsers()
        {
            if (_allUsers == null)
                return;

            if (string.IsNullOrEmpty(SearchText))
            {
                Users.Clear();
                foreach (var user in _allUsers)
                {
                    Users.Add(user);
                }
            }
            else
            {
                var filteredUsers = _allUsers
                    .Where(u => u.Username.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                                u.Email.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Users.Clear();
                foreach (var user in filteredUsers)
                {
                    Users.Add(user);
                }
            }
        }

        private async Task OnDeleteUser(User user)
        {
            if (user == null)
                return;

            await DatabaseService.DeleteUserAsync(user.Id);
            _allUsers.Remove(user);
            Users.Remove(user);
        }

        // Select or deselect all users
        public void SelectAllUsers(bool isSelected)
        {
            // Placeholder for select all logic if needed
            // Example: Iterate through Users and set a property like `IsSelected` if implemented
        }
    }
}
