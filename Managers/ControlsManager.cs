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
    internal enum MineTouchType
    {
        Step, Mark, Clear
    }

    public static class ControlsManager
    {
        public static Binding BlockFontSizeBinding = new Binding();
        public static Binding IconFontSizeBinding = new Binding();

        public static MouseButtonEventHandler PreviewMouseLeftButtonUpAction = (s, e) =>
        {
            int i = (s as IntelliButton).Row;
            int j = (s as IntelliButton).Column;
            bool isSteppedOnMine = MapManager.Click(i, j);
            if (isSteppedOnMine)
            {
                JudgeTouch(MineTouchType.Step);
            }
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
                JudgeTouch(MineTouchType.Mark);
            }
            else
            {
                BlockButtons[i, j].FontFamily = new FontFamily(Default.BlockFontFamily);
                BlockButtons[i, j].Content = null;
                BlockButtons[i, j].SetBinding(Button.FontSizeProperty, BlockFontSizeBinding);
                JudgeTouch(MineTouchType.Clear);
            }
        };

        private static readonly int originalMinesCount = Default.MinesCount;
        private static bool haveShown = false;
        private static int markedCount = 0;
        private static int minesCount = Default.MinesCount;
        private static int steppedCount = 0;

        static ControlsManager()
        {
            BlockButtons = new Button[Default.MapRow, Default.MapColumn];
            BlockFontSizeBinding.Source = Default;
            BlockFontSizeBinding.Path = new PropertyPath("BlockFontSize");
            IconFontSizeBinding.Source = Default;
            IconFontSizeBinding.Path = new PropertyPath("IconFontSize");
        }

        public static Button[,] BlockButtons { get; set; }

        private static void JudgeTouch(MineTouchType type)
        {
            if (!haveShown)
            {
                switch (type)
                {
                    case MineTouchType.Clear:
                        ++minesCount;
                        --markedCount;
                        break;

                    case MineTouchType.Mark:
                        --minesCount;
                        ++markedCount;
                        ShowNotificationOrNot();
                        break;

                    case MineTouchType.Step:
                        --minesCount;
                        ++steppedCount;
                        ShowNotificationOrNot();
                        break;
                }
            }
        }

        private static void ShowNotificationOrNot()
        {
            if (minesCount == 0)
            {
                haveShown = true;
                string contentText;
                if (steppedCount == 0)
                {
                    contentText = $"Congratulations!\nYou have marked all {markedCount} mines without stepping on any of them.";
                }
                else
                {
                    double percentage = Math.Round(((double)markedCount / (double)originalMinesCount), 4) * 100;
                    contentText = $"Don't worry, these mines won't explode.\nYou just stepped on {steppedCount} of them,\nand correctly marked {markedCount} mines ({percentage}% of all).";
                }
                MessageBox.Show(contentText, "All mines are found!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}