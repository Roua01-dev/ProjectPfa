using ProjectPfa.ViewModel;
using System.Collections.ObjectModel;

namespace ProjectPfa.View
{
    public partial class LandingPage : Shell

    {

        //public LandingPage(string username, string profilePicturePath)
        //{
        //    InitializeComponent();
        //    this.BindingContext = new LandingViewModel(username, profilePicturePath);
        //    ////SetDefaultSelection();


        //}
        public LandingPage()
        {
            InitializeComponent();
            this.BindingContext = new LandingViewModel();
            //SetDefaultSelection();


        }


        //private void SetDefaultSelection()
        //{
        //    var viewModel = BindingContext as LandingViewModel;
        //    if (viewModel != null)
        //    {
        //        // Assurez-vous que la section "All" est sélectionnée par défaut
        //        viewModel.SelectedSection = viewModel.Sections.First();

        //        // Rechercher et vérifier le RadioButton correspondant à "All"
        //        var radioButton = FindRadioButtonForSection(viewModel.SelectedSection);
        //        if (radioButton != null)
        //        {
        //            radioButton.IsChecked = true;
        //        }
        //    }
        //}

        //private RadioButton FindRadioButtonForSection(string section)
        //{
        //    foreach (var child in SectionList.Children)
        //    {
        //        if (child is RadioButton radioButton && radioButton.Content.ToString() == section)
        //        {
        //            return radioButton;
        //        }
        //    }
        //    return null;
        //}

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var radioButton = sender as RadioButton;
                var section = radioButton?.Content.ToString();
                var viewModel = BindingContext as LandingViewModel;

                if (viewModel != null && section != null)
                {
                    viewModel.SelectedSection = section;

                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}