using GardeningWithMines.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Managers
{
    public static class ControlsManager
    {
        public static Binding BlockFontSizeBinding = new Binding();
        public static Binding IconFontSizeBinding = new Binding();

        public static MouseButtonEventHandler PreviewMouseLeftButtonUpAction = (s, e) =>
        {
            int i = (s as IntelliButton).Row;
            int j = (s as IntelliButton).Column;
            MapManager.Click(i, j);
            ShowNotificationOrNot();
        };

        public static MouseButtonEventHandler PreviewMouseRightButtonUpAction = (s, e) =>
        {
            int i = (s as IntelliButton).Row;
            int j = (s as IntelliButton).Column;
            if (BlockButtons[i, j].Content == null)
            {
                BlockButtons[i, j].FontFamily = new FontFamily(Default.IconFontFamily);
                BlockButtons[i, j].Content = Default.FlagCharacter;
                BlockButtons[i, j].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
            }
            else
            {
                BlockButtons[i, j].FontFamily = new FontFamily(Default.BlockFontFamily);
                BlockButtons[i, j].Content = null;
                BlockButtons[i, j].SetBinding(Button.FontSizeProperty, BlockFontSizeBinding);
            }
        };

        static ControlsManager()
        {
            ColorsList = new List<ColorWithName>
            {
                new ColorWithName(Colors.Cyan, "Cyan (Default)"),
                new ColorWithName(Colors.LightBlue, "Light Blue"),
                new ColorWithName(Colors.Aquamarine, "Aquamarine"),
                new ColorWithName(Colors.LightGreen, "Light Green"),
                new ColorWithName(Colors.LightPink, "Light Pink"),
                new ColorWithName(Colors.Lime, "Lime"),
                new ColorWithName(Colors.Gold, "Gold"),
                new ColorWithName(Colors.PeachPuff, "Peach Puff"),
                new ColorWithName(Colors.WhiteSmoke, "White Smoke"),
                new ColorWithName(Colors.White, "Pure White"),
                new ColorWithName(Colors.Black, "Pure Black"),
                new ColorWithName(Colors.Transparent, "Air")
            };
            BlockFontSizeBinding.Source = Default;
            BlockFontSizeBinding.Path = new PropertyPath("BlockFontSize");
            IconFontSizeBinding.Source = Default;
            IconFontSizeBinding.Path = new PropertyPath("IconFontSize");
        }

        public static IntelliButton[,] BlockButtons { get; set; }
        public static List<ColorWithName> ColorsList { get; }

        private static void ShowNotificationOrNot()
        {
            if ((!CurrentGameData.HaveShownNotification) && CurrentGameData.UnclickedSafeBlockCount == 0)
            {
                CurrentGameData.HaveShownNotification = true;
                string contentText;
                if (CurrentGameData.SteppedCount == 0)
                {
                    contentText = $"Congratulations!\nYou have found all {CurrentGameData.MinesCount} mines without stepping on any of them.";
                }
                else
                {
                    contentText = $"Don't worry, these mines won't explode.\nYou just stepped on {CurrentGameData.SteppedCount} of them,\nand safely found {CurrentGameData.MinesCount - CurrentGameData.SteppedCount} mines.";
                }
                MessageBox.Show(contentText, "All mines are found!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}