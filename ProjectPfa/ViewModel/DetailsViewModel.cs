using ProjectPfa.Models;
using System.Collections.Generic;
using System.Windows.Input;

using MediaBrowser.Model.Session;
using CommunityToolkit.Mvvm.Input;

namespace ProjectPfa.ViewModel
{
    public class DetailsViewModel : BaseViewModel
    {
        private Property selectedProperty;
        public Property SelectedProperty
        {
            get => selectedProperty;
            set
            {
                SetProperty(ref selectedProperty, value);
                OnPropertyChanged(nameof(SelectedProperty));
            }
        }

        public List<string> PropertyImages { get; set; }
        public int MoreItems { get; set; }

        public ICommand CloseCommand => new RelayCommand(() => Application.Current.MainPage.Navigation.PopAsync());
        public ICommand ChangeImageCommand => new RelayCommand<string>(ChangeImage);

        private void ChangeImage(string newImage)
        {
            if (SelectedProperty != null)
            {
                SelectedProperty.DefaultImage = newImage;
                OnPropertyChanged(nameof(SelectedProperty));
            }
        }
    }
}
