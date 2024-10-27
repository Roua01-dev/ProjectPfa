using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ProjectPfa.Models;
using Microsoft.Maui.Controls;
using ProjectPfa.View;
using System.Collections.ObjectModel;

namespace ProjectPfa.ViewModel
{
    public class LandingViewModel : BaseViewModel
    {
        private string _selectedSection;

        private string _searchText;

        private string _username;
        private string _profilePicturePath;
        private List<Property> _allProperties;

        public LandingViewModel()
        {
        }

        //public LandingViewModel(string username, string profilePicturePath)
        //{
        //    Username = username;
        //    ProfilePicturePath = profilePicturePath;
        //    _allProperties = PropertyRepo.AllProperties; // Load all properties from the repository
        //    FilteredProperties = new ObservableCollection<Property>(_allProperties);

        //}

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

        public List<string> Sections => new List<string> { "All", "Trending", "Popular", "Buy", "Rent" };


        public Property SelectedProperty { get; set; }

        public string SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;

                    OnPropertyChanged();
                    UpdateFilteredProperties();

                }

            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                UpdateFilteredProperties();
            }
        }


        public ObservableCollection<Property> FilteredProperties { get; set; }

        public void UpdateFilteredProperties()
        {
            var filtered = _allProperties;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(p => p.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            switch (SelectedSection)
            {
                case "Trending":
                    filtered = filtered.Where(p => p.IsTrending).ToList();
                    break;
                case "Popular":
                    filtered = filtered.Where(p => p.IsPopular).ToList();
                    break;
                case "Buy":
                    filtered = filtered.Where(p => p.IsForSale).ToList();
                    break;
                case "Rent":
                    filtered = filtered.Where(p => p.IsForRent).ToList();
                    break;
                default:
                    break;
            }

            FilteredProperties.Clear();
            foreach (var property in filtered)
            {
                FilteredProperties.Add(property);
            }
        }



        //hedhy mregla
        public ICommand PropertySelected => new Command(obj =>
        {
            if (SelectedProperty != null)
                Application.Current.MainPage.Navigation.PushAsync(new DetailsPage(SelectedProperty));
            SelectedProperty = null;
        });











        public ICommand OpenMapCommand => new Command(OpenMap);

        private void OpenMap()
        {
            try
            {
                var location = new Location(36.8065, 10.1815); // Example coordinates for Tunis
                var options = new MapLaunchOptions { Name = "Tunis" };

                Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            }
        }


    }
}
