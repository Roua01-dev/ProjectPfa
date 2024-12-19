using ProjectPfa.Models;
using ProjectPfa.ViewModel;

namespace ProjectPfa.View
{
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Property property)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(property);
        }
    }
}
