using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GardeningWithMines.Models
{
    public class ColorWithName
    {
        public ColorWithName(Color color)
        {
            Color = color;
            Name = color.ToString();
        }

        public ColorWithName(Color color, string name)
        {
            Color = color;
            Name = name;
        }

        public Color Color { get; set; }
        public string Name { get; set; }
    }
}