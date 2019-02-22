using GardeningWithMines.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
            BlockButtons = new Button[Default.MapRow, Default.MapColumn];
            BlockFontSizeBinding.Source = Default;
            BlockFontSizeBinding.Path = new PropertyPath("BlockFontSize");
            IconFontSizeBinding.Source = Default;
            IconFontSizeBinding.Path = new PropertyPath("IconFontSize");
        }

        public static Button[,] BlockButtons { get; set; }

        private static void ShowNotificationOrNot()
        {
            if (MapManager.unclickedSafeBlockCount == 0)
            {
                string contentText;
                if (MapManager.steppedCount == 0)
                {
                    contentText = $"Congratulations!\nYou have found all {MapManager.minesCount} mines without stepping on any of them.";
                }
                else
                {
                    contentText = $"Don't worry, these mines won't explode.\nYou just stepped on {MapManager.steppedCount} of them,\nand safely found {MapManager.minesCount - MapManager.steppedCount} mines.";
                }
                MessageBox.Show(contentText, "All mines are found!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}