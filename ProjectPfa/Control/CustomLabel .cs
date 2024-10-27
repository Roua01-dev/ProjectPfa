using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ProjectPfa.Control
{
    public class CustomLabel : Label
    {
        public CustomLabel()
        {
            ApplyCustomStyle();
        }

        private void ApplyCustomStyle()
        {
            this.FontSize = 32;
            this.FontAttributes = FontAttributes.Bold;
            this.TextColor = Colors.DarkGray;
            this.HorizontalOptions = LayoutOptions.Center;

           
        }
    }
}
