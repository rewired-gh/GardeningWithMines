using GardeningWithMines.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GardeningWithMines.Managers.ControlsManager;
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
            MyInit();
        }

        private void MyInit()
        {
            MapGrid.MinHeight = Default.MinBlockHeight * Default.MapRow;
            MapGrid.MinWidth = Default.MinBlockWidth * Default.MapColumn;

            for (int i = 0; i < Default.MapRow; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < Default.MapColumn; i++)
            {
                MapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < Default.MapRow; i++)
            {
                for (int j = 0; j < Default.MapColumn; j++)
                {
                    SetBlock(i, j);
                }
            }
        }

        private void RecalculateBlockFontSize()
        {
            double blockFontSize = (Default.MapHeight < MapGrid.MinHeight ?
                MapGrid.MinHeight : Default.MapHeight) / Default.MapRow * Default.FontSizeRatio;
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

        private void UpdateWindow(double delta = 0)
        {
            double tempHeight = ActualHeight - Default.DeltaHeight - delta;
            double tempWidth = tempHeight / Default.MapRow * Default.MapColumn;
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
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
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
            if (WindowState == WindowState.Maximized)
            {
                UpdateWindow(Default.MaxViewMargin);
            }
            else
            {
                UpdateWindow();
            }
        }
    }
}