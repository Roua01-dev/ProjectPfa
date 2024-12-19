using ProjectPfa.Models;

namespace ProjectPfa.ViewModel
{
    public class DetailsViewModel : BaseViewModel
    {
        private Property _property;

        public Property Property
        {
            get => _property;
            set
            {
                _property = value;
                OnPropertyChanged();
            }
        }

        public DetailsViewModel(Property property)
        {
            Property = property;
        }
    }
}
