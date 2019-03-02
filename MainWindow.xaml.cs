using GardeningWithMines.Models;
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
            Default.Save();
        }

        public void Clear()
        {
            MapGrid.Children.Clear();
            MapGrid.RowDefinitions.Clear();
            MapGrid.ColumnDefinitions.Clear();
        }

        public void Init()
        {
            BlockButtons = new IntelliButton[CurrentGameData.MapRow, CurrentGameData.MapColumn];
            MapGrid.MinHeight = Default.MinBlockHeight * CurrentGameData.MapRow;
            MapGrid.MinWidth = Default.MinBlockWidth * CurrentGameData.MapColumn;

            for (int i = 0; i < CurrentGameData.MapRow; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < CurrentGameData.MapColumn; i++)
            {
                MapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < CurrentGameData.MapRow; i++)
            {
                for (int j = 0; j < CurrentGameData.MapColumn; j++)
                {
                    SetBlock(i, j);
                }
            }
            UpdateWindow();
        }

        private void MapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void RecalculateBlockFontSize()
        {
            double blockFontSize = (Default.MapHeight < MapGrid.MinHeight ?
                MapGrid.MinHeight : Default.MapHeight) / CurrentGameData.MapRow * Default.FontSizeRatio;
            Default.BlockFontSize = blockFontSize < Default.BlockMaxFontSize ?
                blockFontSize : Default.BlockMaxFontSize;
            Default.IconFontSize = Default.BlockFontSize * Default.IconSizeRatio;
        }

        private void SetBlock(int i, int j)
        {
            BlockButtons[i, j] = new IntelliButton(i, j)
            {
                Style = Application.Current.Resources["BlockButtonStyle"] as Style,
                Padding = new Thickness(0)
            };
            BlockButtons[i, j].PreviewMouseLeftButtonUp += PreviewMouseLeftButtonUpAction;
            BlockButtons[i, j].PreviewMouseRightButtonUp += PreviewMouseRightButtonUpAction;

            MapGrid.Children.Add(BlockButtons[i, j]);
            Grid.SetRow(BlockButtons[i, j], i);
            Grid.SetColumn(BlockButtons[i, j], j);
        }

        private void UpdateWindow()
        {
            double delta = WindowState == WindowState.Maximized ? Default.MaxViewMargin : 0;
            double tempHeight = ActualHeight - Default.DeltaHeight - delta;
            double tempWidth = tempHeight / CurrentGameData.MapRow * CurrentGameData.MapColumn;
            if (Default.ForceCompleteView)
            {
                if (Width < tempWidth)
                {
                    Width = tempWidth + Default.DeltaWidth;
                }
            }
            Default.MapHeight = tempHeight;
            if (Default.IsSquareBlock)
            {
                Default.MapWidth = tempWidth;
            }
            else
            {
                Default.MapWidth = ActualWidth - Default.DeltaWidth - delta;
            }
            RecalculateBlockFontSize();
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    GameRefresh();
                    break;

                case Key.A:
                    AboutBox1 aboutDialog = new AboutBox1();
                    aboutDialog.ShowDialog();
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