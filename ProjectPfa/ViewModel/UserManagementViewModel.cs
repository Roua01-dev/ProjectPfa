using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ProjectPfa.Models;
using ProjectPfa.Data;

namespace ProjectPfa.ViewModel
{
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly Database _database;

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        private ObservableCollection<User> _allUsers = new ObservableCollection<User>();

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

        public ICommand RefreshCommand { get; }
        public ICommand LoadUsersCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SearchCommand { get; }

        public UserManagementViewModel()
        {
            _database = new Database();
            RefreshCommand = new Command(async () => await LoadUsersAsync());
            LoadUsersCommand = new Command(async () => await LoadUsersAsync());
            DeleteUserCommand = new Command<User>(async (user) => await DeleteUser(user));
            SearchCommand = new Command<string>(query => SearchText = query);

            LoadUsersCommand.Execute(null);
        }

        private async Task LoadUsersAsync()
        {
            Users.Clear();
            _allUsers.Clear();
            var usersFromDb = await _database.GetUsersAsync();
            foreach (var user in usersFromDb)
            {
                _allUsers.Add(user);
                Users.Add(user);
            }
        }

        private void SearchUsers()
        {
            if (_allUsers == null)
            {
                return;
            }

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
                    .Where(u => u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Users.Clear();
                foreach (var user in filteredUsers)
                {
                    Users.Add(user);
                }
            }
        }

        public async Task DeleteUser(User user)
        {
            if (user == null)
            {
                return;
            }

            await _database.DeleteUserAsync(user);
            _allUsers.Remove(user);
            Users.Remove(user);
        }
    }
}
