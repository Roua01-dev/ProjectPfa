using ProjectPfa.Data;
using ProjectPfa.View;
namespace ProjectPfa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new NavigationPage(new HomePage());
            Task.Run(async () => await DatabaseService.InitializeDatabaseAsync());

            //  MainPage = new NavigationPage(new PropertyManagmentPage());
            //  MainPage = new LandingPage();

            //    MainPage = new HomePage();
            // MainPage = new NavigationPage(new SignIn());



              MainPage = new NavigationPage(new HomePage());



            //MainPage = new NavigationPage(new UserManagementPage());
         //   MainPage = new NavigationPage(new AddPropertyPage());


        }
    }
    
}
