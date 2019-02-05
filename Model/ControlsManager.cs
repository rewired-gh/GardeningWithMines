using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Model
{
    public static class ControlsManager
    {
        static ControlsManager()
        {
            BlockButtons = new Button[Default.MapRow, Default.MapColumn];
        }
        public static Button[,] BlockButtons { get; set; }
    }
}
