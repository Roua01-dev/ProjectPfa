using ProjectPfa.ViewModel;

namespace ProjectPfa.View;

public partial class PropertyManagmentPage : ContentPage
{
	public PropertyManagmentPage()
	{
		InitializeComponent();
		this.BindingContext = new PropertyManagmentViewModel();
        SetDefaultSelection();
    }


    public void RefreshProperties()
    {
        if (BindingContext is PropertyManagmentViewModel viewModel)
        {
            viewModel.LoadPropertiesCommand.Execute(null);
        }
    }

    private void SetDefaultSelection()
    {
        var viewModel = BindingContext as PropertyManagmentViewModel;
        if (viewModel != null)
        {
            // Assurez-vous que la section "All" est sélectionnée par défaut
            viewModel.SelectedSection = viewModel.Sections.First();

            // Rechercher et vérifier le RadioButton correspondant à "All"
            var radioButton = FindRadioButtonForSection(viewModel.SelectedSection);
            if (radioButton != null)
            {
                radioButton.IsChecked = true;
            }
        }
    }


    private async void OnPropertySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Property selectedProperty)
        {
            await Navigation.PushAsync(new DetailsPage(selectedProperty));
        }
    }

    private RadioButton FindRadioButtonForSection(string section)
    {
        foreach (var child in SectionList.Children)
        {
            if (child is RadioButton radioButton && radioButton.Content.ToString() == section)
            {
                return radioButton;
            }
        }
        return null;
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            var radioButton = sender as RadioButton;
            var section = radioButton?.Content.ToString();
            var viewModel = BindingContext as PropertyManagmentViewModel;

            if (viewModel != null && section != null)
            {
                viewModel.SelectedSection = section; // Set the selected section in ViewModel
            }
        }
    }

}