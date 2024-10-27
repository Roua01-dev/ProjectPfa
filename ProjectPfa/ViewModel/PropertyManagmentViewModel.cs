using Microsoft.Maui.Controls;
using ProjectPfa.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPfa.ViewModel
{
    public class PropertyManagmentViewModel :BaseViewModel
    {
        public Command AddPropertyCommand { get; }
        

        public List<string> Sections => new List<string> { "All", "Trending", "Popular", "Buy", "Rent" };

        public PropertyManagmentViewModel() {
            AddPropertyCommand = new Command(OnAddProperty);
        }
        private async void OnAddProperty()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPropertyPage());
        }

    }
}
