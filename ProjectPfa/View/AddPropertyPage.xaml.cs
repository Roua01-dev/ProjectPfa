using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Linq;
using System.Threading.Tasks;
using ProjectPfa.ViewModel;
using ProjectPfa.Models;
using ProjectPfa.Data;

namespace ProjectPfa.View
{
    public partial class AddPropertyPage : ContentPage
    {
        private Pin singlePin;

        public AddPropertyPage()
        {
            InitializeComponent();

            // Binding ViewModel with Map for Location Management
            var viewModel = new AddPropertyViewModel();
            this.BindingContext = viewModel;
            viewModel.Map = Map;


     


        }

        // Event handler for MapClicked
        private async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            await UpdatePinLocationAsync(e.Location);
        }


        private async void OnSearchLocation(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBar.Text))
                return;

            try
            {
                // Utiliser le service Geocoding pour chercher l'adresse
                var locations = await Geocoding.GetLocationsAsync(SearchBar.Text);
                var location = locations?.FirstOrDefault();

                if (location != null)
                {
                    // Supprimer tous les anciens pins
                    Map.Pins.Clear();

                    // Créer et ajouter un nouveau Pin
                    await UpdatePinLocationAsync(location);

                    // Déplacer la carte vers l'emplacement trouvé
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(0.5)));
                }
                else
                {
                    await DisplayAlert("Not Found", "Location not found. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Placez un breakpoint ici pour voir si le bouton sauvegarde déclenche l'action
            if (BindingContext is AddPropertyViewModel viewModel)
            {
                // Inspectez les données du ViewModel avant la sauvegarde
                viewModel.SaveCommand.Execute(null);
            }
        }



        private async Task UpdatePinLocationAsync(Location location)
        {
            // Reverse Geocoding pour obtenir l'adresse
            var placemarks = await Geocoding.GetPlacemarksAsync(location);
            var placemark = placemarks?.FirstOrDefault();

            string label = "Unknown Location";
            string address = "No address available";

            if (placemark != null)
            {
                label = placemark.FeatureName ?? "Unnamed Place";
                address = $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
            }

            // Supprimer tous les anciens pins
            Map.Pins.Clear();

            // Créer un nouveau Pin
            var pin = new Pin
            {
                Label = label,
                Address = address,
                Location = location
            };

            // Ajouter le nouveau Pin
            Map.Pins.Add(pin);

            // Centrer la carte sur la nouvelle position
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(0.1)));


            // Mettre à jour l'adresse dans le ViewModel
            if (BindingContext is AddPropertyViewModel viewModel)
            {
                viewModel.Address = address;
            }
        }
    }
}
