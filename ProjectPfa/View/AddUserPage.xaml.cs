using ProjectPfa.ViewModel;

namespace ProjectPfa.View;

public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
        BindingContext = new AddUserViewModel();
    }
}
