using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectPfa.Models;
using ProjectPfa.Data;
using ProjectPfa.View;

namespace ProjectPfa.ViewModel
{
    public class PropertyManagmentViewModel : BaseViewModel
    {
        public Command AddPropertyCommand { get; }
        public ObservableCollection<Property> Properties { get; set; }
        public ObservableCollection<Property> FilteredProperties { get; set; } // Propriétés après filtrage
        private List<Property> _allProperties = new List<Property>(); // Liste complète des propriétés
        public ICommand LoadPropertiesCommand { get; }

        private string _selectedSection;
        public string SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;
                    OnPropertyChanged();
                    ApplyFilter(); // Applique le filtre lorsque la section change
                }
            }
        }

        public List<string> Sections => new List<string> { "All", "Trending", "Popular", "Buy", "Rent" };

        public PropertyManagmentViewModel()
        {
            AddPropertyCommand = new Command(OnAddProperty);
            Properties = new ObservableCollection<Property>();

            FilteredProperties = new ObservableCollection<Property>(_allProperties);
            LoadPropertiesCommand = new Command(async () => await LoadPropertiesAsync());
            LoadPropertiesCommand.Execute(null);
            LoadProperties();
        }



        private async Task LoadPropertiesAsync()
        {

            try
            {
                var properties = await DatabaseService.GetAllPropertiesAsync();
                Properties = new ObservableCollection<Property>(properties);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load properties: {ex.Message}", "OK");
            }
          
        }

        private async void OnAddProperty()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPropertyPage());
        }

        private async void LoadProperties()
        {
            try
            {
                var properties = await DatabaseService.GetAllPropertiesAsync();
                _allProperties.Clear(); // Reset full property list
                _allProperties.AddRange(properties);

                // Initialize `FilteredProperties` with all properties
                FilteredProperties.Clear();
                foreach (var property in _allProperties)
                {
                    // Ensure the `IsAll` flag is set if it doesn't belong to specific sections
                    if (!property.IsTrending && !property.IsPopular && !property.IsForRent && !property.IsForSale)
                    {
                        property.IsAll = true;
                    }
                    FilteredProperties.Add(property);
                }

                SelectedSection = "All"; // Default section
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load properties: {ex.Message}");
            }
        }


        private void ApplyFilter()
        {
            var filtered = _allProperties; // Start with all properties

            // Apply filtering based on the selected section
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
                default: // "All"
                    break;
            }

            // Update `FilteredProperties`
            FilteredProperties.Clear();
            foreach (var property in filtered)
            {
                FilteredProperties.Add(property);
            }
        }


    }
}
