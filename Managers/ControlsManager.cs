using GardeningWithMines.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Properties.Settings;
using MaterialDesignThemes.Wpf;

namespace GardeningWithMines.Managers
{
    public static class ControlsManager
    {
        //Grungy bindings
        public static Binding BlockFontSizeBinding = new Binding();

        public static FontFamily IconFontFamily;
        public static Binding IconFontSizeBinding = new Binding();

        public static MouseButtonEventHandler PreviewMouseLeftButtonUpAction = (s, e) =>
        {
            //Get row and column
            int i = (s as IntelliButton).Row;
            int j = (s as IntelliButton).Column;

            //"Hey, it's your time, Map Manager."
            MapManager.Click(i, j);

            //Judge whether gamer has finished the game
            ShowNotificationOrNot();
        };

        public static MouseButtonEventHandler PreviewMouseRightButtonUpAction = (s, e) =>
        {
            //Get row and column
            int i = (s as IntelliButton).Row;
            int j = (s as IntelliButton).Column;

            //Change block content
            if (BlockButtons[i, j].Content == null)
            {
                BlockButtons[i, j].FontFamily = IconFontFamily;
                BlockButtons[i, j].Content = Default.FlagCharacter;
                BlockButtons[i, j].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
            }
            else
            {
                BlockButtons[i, j].FontFamily = TextFontFamily;
                BlockButtons[i, j].Content = null;
                BlockButtons[i, j].SetBinding(Button.FontSizeProperty, BlockFontSizeBinding);
            }
        };

        public static FontFamily TextFontFamily;

        static ControlsManager()
        {
            //Create color list ( in a shitty way )
            ColorsList = new List<ColorWithName>
            {
                new ColorWithName(Colors.Cyan, "Cyan (Default)"),
                new ColorWithName(Colors.DarkCyan, "Dark Cyan"),
                new ColorWithName(Colors.LightBlue, "Light Blue"),
                new ColorWithName(Colors.SteelBlue, "Steel Blue"),
                new ColorWithName(Colors.Aquamarine, "Aquamarine"),
                new ColorWithName(Colors.LightGreen, "Light Green"),
                new ColorWithName(Colors.Lime, "Lime"),
                new ColorWithName(Colors.Olive, "Olive"),
                new ColorWithName(Colors.SeaGreen, "Sea Green"),
                new ColorWithName(Colors.Orchid, "Orchid"),
                new ColorWithName(Colors.LightPink, "Light Pink"),
                new ColorWithName(Colors.HotPink, "Hot Pink"),
                new ColorWithName(Colors.DeepPink, "Deep Pink"),
                new ColorWithName(Colors.Crimson, "Crimson"),
                new ColorWithName(Colors.Tomato, "Tomato"),
                new ColorWithName(Colors.Gold, "Gold"),
                new ColorWithName(Colors.DarkGoldenrod, "Dark Goldenrod"),
                new ColorWithName(Colors.PeachPuff, "Peach Puff"),
                new ColorWithName(Colors.MintCream, "Mint Cream"),
                new ColorWithName(Colors.Azure, "Azure"),
                new ColorWithName(Colors.FloralWhite, "Floral White"),
                new ColorWithName(Colors.Azure, "Ivory"),
                new ColorWithName(Colors.WhiteSmoke, "White Smoke"),
                new ColorWithName(Colors.White, "Pure White"),
                new ColorWithName(Colors.Azure, "Gray"),
                new ColorWithName(Colors.Black, "Pure Black"),
                new ColorWithName(Colors.Transparent, "Air")
            };

            //Set bindings
            BlockFontSizeBinding.Source = Default;
            BlockFontSizeBinding.Path = new PropertyPath("BlockFontSize");
            IconFontSizeBinding.Source = Default;
            IconFontSizeBinding.Path = new PropertyPath("IconFontSize");

            //Prepare font families
            TextFontFamily = new FontFamily(Default.TextFontFamily);
            IconFontFamily = new FontFamily(Default.IconFontFamily);
        }

        public static IntelliButton[,] BlockButtons { get; set; }
        public static List<ColorWithName> ColorsList { get; }

        private static void ShowNotificationOrNot()
        {
            //TODO: Improve UI
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