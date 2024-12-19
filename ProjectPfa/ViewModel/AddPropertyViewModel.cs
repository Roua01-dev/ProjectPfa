using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using IMap = Microsoft.Maui.Controls.Maps.Map;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using SQLite;
using ProjectPfa.Models;
using ProjectPfa.Data;
using ProjectPfa.View;
namespace ProjectPfa.ViewModel
{
    public partial class AddPropertyViewModel : BaseViewModel
    {
        public ICommand SearchLocationCommand { get; }
        private string defaultImage;
        private ObservableCollection<string> images = new ObservableCollection<string>();

        public string Title { get; set; }
        public decimal? Price { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? Garage { get; set; }
        public string Description { get; set; }
        public string SearchText { get; set; }
        // Propriétés pour les statuts


        private bool _isAll = true; // Par défaut, toutes les propriétés sont dans "All"
        public bool IsAll
        {
            get => _isAll;
            set => SetProperty(ref _isAll, value);
        }


        private bool _isTrending;
        public bool IsTrending
        {
            get => _isTrending;
            set => SetProperty(ref _isTrending, value);
        }

        private bool _isPopular;
        public bool IsPopular
        {
            get => _isPopular;
            set => SetProperty(ref _isPopular, value);
        }

        private bool _isForRent;
        public bool IsForRent
        {
            get => _isForRent;
            set => SetProperty(ref _isForRent, value);
        }

        private bool _isForSale;
        public bool IsForSale
        {
            get => _isForSale;
            set => SetProperty(ref _isForSale, value);
        }
        public string DefaultImage
        {
            get => defaultImage;
            set
            {
                defaultImage = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }




        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }




        public ObservableCollection<string> Images
        {
            get => images;
            set
            {
                images = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectDefaultImageCommand { get; }
        public ICommand SelectOtherImagesCommand { get; }


        private Location _propertyLocation;
        public Location PropertyLocation
        {
            get => _propertyLocation;
            set => SetProperty(ref _propertyLocation, value);
        }

        private Map _map;
        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public ICommand SaveCommand { get; }

        public AddPropertyViewModel()
        {
            SaveCommand = new Command(OnSave);
            InitializeMap();
            SearchLocationCommand = new AsyncRelayCommand(OnSearchLocationAsync);
            SelectDefaultImageCommand = new Command(async () => await SelectDefaultImage());
            SelectOtherImagesCommand = new Command(async () => await SelectOtherImages());


        }


        // Méthode pour sélectionner l'image par défaut
        private async Task SelectDefaultImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                // Mettez à jour la propriété DefaultImage avec le chemin du fichier sélectionné
                DefaultImage = result.FullPath;
                Debug.WriteLine($"Selected Image Path: {DefaultImage}");

                // OnForce la mise à jour de la liaison
                OnPropertyChanged(nameof(DefaultImage));
            }
        }



        private async Task SelectOtherImages()
        {
            var results = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (results != null)
            {
                Images.Clear();
                foreach (var file in results)
                {
                    Images.Add(file.FullPath);
                }
                DefaultImage = Images.FirstOrDefault();

            }
        }





        private async Task SelectImages()
        {
            // Logic to open the gallery and pick images
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                Images.Clear();
                foreach (var file in result)
                {
                    Images.Add(file.FullPath); // Add the selected image's path to the collection
                }

                // Set the first image as the default image
                DefaultImage = Images.FirstOrDefault();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }






        private async Task OnSearchLocationAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            try
            {
                var locations = await Geocoding.GetLocationsAsync(SearchText);
                var location = locations?.FirstOrDefault();

                if (location != null && Map != null)
                {
                    // Supprimer tous les anciens pins
                    Map.Pins.Clear();

                    var pin = new Pin
                    {
                        Label = "Searched Location",
                        Address = SearchText,
                        Location = location
                    };

                    // Ajouter un nouveau Pin
                    Map.Pins.Add(pin);

                    // Déplacer la carte vers l'emplacement trouvé
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(0.5)));
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs
            }
        }

        private async void InitializeMap()
        {
            await RequestLocationPermissionIfNeeded();
        }

        private async Task RequestLocationPermissionIfNeeded()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }

        private async void OnSave()
        {
            var newProperty = new Property
            {
                Title = Title,
                Price = Price ?? 0,
                Bedrooms = Bedrooms ?? 0,
                Bathrooms = Bathrooms ?? 0,
                Garage = Garage ?? 0,
                Address = Address,
                Description = Description,
                Numero = Phone,
                DefaultImage = DefaultImage,
                Images = string.Join(",", Images),
                IsTrending = IsTrending,
                IsPopular = IsPopular,
                IsForRent = IsForRent,
                IsForSale = IsForSale,
                IsAll = IsAll
            };

            try
            {
                // Ajouter la propriété dans la base de données
                await DatabaseService.AddPropertyAsync(newProperty);

                // Affichez un message de confirmation
                await App.Current.MainPage.DisplayAlert("Success", "Property saved successfully!", "OK");

                // Naviguer vers PropertyManagementPage
                await App.Current.MainPage.Navigation.PushAsync(new PropertyManagmentPage());

                // Rafraîchir PropertyManagementPage (si applicable)
                if (App.Current.MainPage.Navigation.NavigationStack.Last() is PropertyManagmentPage propertyManagementPage)
                {
                    propertyManagementPage.RefreshProperties();
                }
            }
            catch (Exception ex)
            {
                // Gérez les erreurs
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to save property: {ex.Message}", "OK");
            }
        }


    }
}
