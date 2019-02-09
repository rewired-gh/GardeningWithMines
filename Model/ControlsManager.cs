using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Model
{
    public static class ControlsManager
    {
        public static Binding BlockFontSizeBinding = new Binding();

        public static Binding IconFontSizeBinding = new Binding();

        static ControlsManager()
        {
            BlockButtons = new Button[Default.MapRow, Default.MapColumn];
            BlockFontSizeBinding.Source = Default;
            BlockFontSizeBinding.Path = new PropertyPath("BlockFontSize");
            IconFontSizeBinding.Source = Default;
            IconFontSizeBinding.Path = new PropertyPath("IconFontSize");
        }

        public static Button[,] BlockButtons { get; set; }
    }
}