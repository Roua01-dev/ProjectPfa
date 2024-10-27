using ProjectPfa.View;
namespace ProjectPfa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new NavigationPage(new HomePage());
            MainPage = new NavigationPage(new PropertyManagmentPage());
            // MainPage = new LandingPage("","");

            //  MainPage = new HomePage();

        }
    }
}
