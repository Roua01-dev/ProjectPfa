using System.Collections.ObjectModel;

namespace ProjectPfa.View;

public partial class HomePage : ContentPage
{

    public HomePage()

	{
        InitializeComponent();
        //SaveProperties();


    }
    private async void SaveProperties()
    {
    }
    private  void OnSignUpTapped(object sender, EventArgs e)
    {
        App.Current.MainPage = new SignUp();
       // await Navigation.PushAsync(new SignUp());
    }

    private  void OnSignInTapped(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new SignIn());
        App.Current.MainPage = new SignIn();
    }

}