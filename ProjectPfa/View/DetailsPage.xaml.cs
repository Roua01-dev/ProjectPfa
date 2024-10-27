using ProjectPfa.ViewModel;

namespace ProjectPfa.View
{
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Models.Property selectedProperty)
        {
            InitializeComponent();
            var viewModel = new DetailsViewModel
            {
                SelectedProperty = selectedProperty,
                PropertyImages = selectedProperty.Images.Take(2).ToList(),
                MoreItems = selectedProperty.Images.Count - 2
            };
            BindingContext = viewModel;
        }
    }
}
