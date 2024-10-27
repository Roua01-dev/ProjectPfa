using ProjectPfa.ViewModel;

namespace ProjectPfa.View;

public partial class PropertyManagmentPage : ContentPage
{
	public PropertyManagmentPage()
	{
		InitializeComponent();
		this.BindingContext = new PropertyManagmentViewModel();
	}

  
}