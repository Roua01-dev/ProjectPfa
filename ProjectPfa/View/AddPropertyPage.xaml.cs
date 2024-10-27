using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using ProjectPfa.ViewModel;
using System.Threading.Tasks;

namespace ProjectPfa.View
{
    public partial class AddPropertyPage : ContentPage
    {
        public AddPropertyPage()
        {
            InitializeComponent();
            BindingContext = new AddPropertyViewModel();

            // Set map to user's current location
            SetInitialLocation();
        }

        private async Task SetInitialLocation()
        {
            var viewModel = BindingContext as AddPropertyViewModel;
            var location = await Geolocation.GetLastKnownLocationAsync() ??
                           await Geolocation.GetLocationAsync(new GeolocationRequest
                           {
                               DesiredAccuracy = GeolocationAccuracy.Best
                           });

            if (location != null)
            {
                viewModel.PropertyLocation = new Location(location.Latitude, location.Longitude);
                PropertyMap.MoveToRegion(MapSpan.FromCenterAndRadius(viewModel.PropertyLocation, Distance.FromMiles(0.5)));

                // Pin for the current location
                var pin = new Pin
                {
                    Label = "Current Location",
                    Location = viewModel.PropertyLocation
                };
                PropertyMap.Pins.Add(pin);
            }
        }
    }
}
