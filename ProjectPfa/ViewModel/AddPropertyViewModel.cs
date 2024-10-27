using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace ProjectPfa.ViewModel
{
    public class AddPropertyViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string FullAddress { get; set; }
        public decimal? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? Garage { get; set; }
        public string Description { get; set; }
        public string SearchText { get; set; }

        // New Properties
        private Location _propertyLocation;
        public Location PropertyLocation
        {
            get => _propertyLocation;
            set
            {
                _propertyLocation = value;
                OnPropertyChanged(nameof(PropertyLocation));
            }
        }

        public ICommand SearchLocationCommand { get; }
        public ICommand SaveCommand { get; }

        public AddPropertyViewModel()
        {
            SaveCommand = new Command(OnSave);
            SearchLocationCommand = new Command(async () => await OnSearchLocation());
        }

        private async Task OnSearchLocation()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                try
                {
                    var locations = await Geocoding.GetLocationsAsync(SearchText);
                    var location = locations?.FirstOrDefault();
                    if (location != null)
                    {
                        PropertyLocation = new Location(location.Latitude, location.Longitude);
                        FullAddress = SearchText;

                        // Notify map of updated location
                        OnPropertyChanged(nameof(PropertyLocation));
                        OnPropertyChanged(nameof(FullAddress));
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., showing an alert to the user
                }
            }
        }

        private void OnSave()
        {
            // Save property logic here
        }
    }
}
