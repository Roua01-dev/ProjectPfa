using ProjectPfa.Models;
using ProjectPfa.ViewModel;

namespace ProjectPfa.View;

public partial class UpdateUserPage : ContentPage
{
	public UpdateUserPage(User selectedUser)
	{
		InitializeComponent();
        var viewModel = new UpdateUserViewModel(selectedUser);
        BindingContext = viewModel;
    }
}