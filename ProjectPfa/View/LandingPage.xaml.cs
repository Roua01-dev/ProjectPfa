using ProjectPfa.ViewModel;
using System.Collections.ObjectModel;

namespace ProjectPfa.View
{
    public partial class LandingPage : Shell

    {

      

        //}
        public LandingPage()
        {
            InitializeComponent();
            BindingContext = new LandingViewModel();

        }









        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}