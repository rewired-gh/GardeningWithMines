using GardeningWithMines.Models;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GardeningWithMines.Managers.ControlsManager;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //Remove blocks
            MapGrid.Children.Clear();

            //Remove grid
            MapGrid.RowDefinitions.Clear();
            MapGrid.ColumnDefinitions.Clear();
        }

        public void Init()
        {
            //Prepare for generating blocks
            BlockButtons = new IntelliButton[CurrentGameData.MapRow, CurrentGameData.MapColumn];
            MapGrid.MinHeight = Default.MinBlockHeight * CurrentGameData.MapRow;
            MapGrid.MinWidth = Default.MinBlockWidth * CurrentGameData.MapColumn;

            //Build grid
            for (int i = 0; i < CurrentGameData.MapRow; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < CurrentGameData.MapColumn; i++)
            {
                MapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //Add blocks
            for (int i = 0; i < CurrentGameData.MapRow; i++)
            {
                for (int j = 0; j < CurrentGameData.MapColumn; j++)
                {
                    SetBlock(i, j);
                }
            }

            //Triger 'update event'
            UpdateWindow();
        }

        private void MapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //Hot loading
            Init();

            //Show 'tips window'
            if (!Default.NeedNotToShowTips)
            {
                TipsWindow tipsWindow = new TipsWindow();
                tipsWindow.ShowDialog();
            }
        }

        private void RecalculateBlockFontSize()
        {
            //Algorithm for recalculating
            double blockFontSize = (Default.MapHeight < MapGrid.MinHeight ?
                MapGrid.MinHeight : Default.MapHeight) //Get current map height
                / CurrentGameData.MapRow * Default.FontSizeRatio; //Calculate font size depending on block height

            Default.BlockFontSize = blockFontSize < Default.BlockMaxFontSize ?
                blockFontSize : Default.BlockMaxFontSize; //Set 'font' size

            Default.IconFontSize = Default.BlockFontSize * Default.IconSizeRatio; //Set 'icon' size
        }

        private void SetBlock(int i, int j)
        {
            //Create new block
            BlockButtons[i, j] = new IntelliButton(i, j)
            {
                Style = Application.Current.Resources["BlockButtonStyle"] as Style, //Get and use the style
                FontFamily = Managers.ControlsManager.TextFontFamily
            };
            BlockButtons[i, j].PreviewMouseLeftButtonDown += PreviewMouseLeftButtonUpAction;
            BlockButtons[i, j].PreviewMouseRightButtonDown += PreviewMouseRightButtonUpAction;

            //Add block
            MapGrid.Children.Add(BlockButtons[i, j]);
            Grid.SetRow(BlockButtons[i, j], i);
            Grid.SetColumn(BlockButtons[i, j], j);
        }

        private void UpdateWindow()
        {
            //Add margin when window is maximized
            double delta = WindowState == WindowState.Maximized ? Default.MaxViewMargin : 0;

            //Alogrithm for recalculating map size
            double tempHeight, tempWidth;
            if (ActualHeight / CurrentGameData.MapRow < ActualWidth / CurrentGameData.MapColumn)
            {
                tempHeight = ActualHeight - delta - Default.DeltaHeight;
                tempWidth = tempHeight / CurrentGameData.MapRow * CurrentGameData.MapColumn;
            }
            else
            {
                tempWidth = ActualWidth - delta - Default.DeltaWidth;
                tempHeight = tempWidth / CurrentGameData.MapColumn * CurrentGameData.MapRow;
            }
            if (Default.ForceCompleteView)
            {
                if (Width < tempWidth)
                {
                    Width = tempWidth + Default.DeltaWidth;
                }
            }

            //Set map size
            Default.MapHeight = tempHeight;
            Default.MapWidth = tempWidth;

            RecalculateBlockFontSize();
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //Awful code
            switch (e.Key)
            {
                case Key.F5:
                    GameRefresh();
                    break;

                case Key.A:
                    AboutBox1 aboutDialog = new AboutBox1();
                    aboutDialog.ShowDialog();
                    break;

                case Key.H:
                    TipsWindow tipsWindow = new TipsWindow();
                    tipsWindow.ShowDialog();
                    break;

                case Key.F12:
                    SimpleConfigWindow configWindow = new SimpleConfigWindow();
                    configWindow.ShowDialog();
                    break;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWindow();
        }
    }
}