using System.Collections.ObjectModel;
using ProjectPfa.Services;

namespace ProjectPfa.View;

public partial class HomePage : ContentPage
{

    public HomePage()

	{
        InitializeComponent();
        SaveProperties();


    }
    private async void SaveProperties()
    {
        var propertyService = new PropertyService();
        await propertyService.SaveAllPropertiesAsync();
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