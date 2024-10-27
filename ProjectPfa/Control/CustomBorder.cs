using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPfa.Control
{
    class CustomBorder : Border
    {
        public CustomBorder()
        {
            AddCornerRadius();
        }

        private List<int> CornerValues = new List<int> { 10, 20, 30, 40 };
        private void AddCornerRadius()
        {
            var index = new Random().Next(CornerValues.Count);
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerValues[index])
            };
        }
    }
}
