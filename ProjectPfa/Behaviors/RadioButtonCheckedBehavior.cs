using Microsoft.Maui.Controls;

namespace ProjectPfa.Behaviors
{
    public class RadioButtonCheckedBehavior : Behavior<RadioButton>
    {
        protected override void OnAttachedTo(RadioButton bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.CheckedChanged += OnCheckedChanged;
        }

        protected override void OnDetachingFrom(RadioButton bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.CheckedChanged -= OnCheckedChanged;
        }

        private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton.IsChecked)
            {
                radioButton.BackgroundColor = Color.FromHex("#4C65FF");
            }
            else
            {
                radioButton.BackgroundColor = Color.FromHex("#F0F0F1");
            }
        }
    }
}
