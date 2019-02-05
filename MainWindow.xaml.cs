using GardeningWithMines.Model;
using System.Windows;
using System.Windows.Controls;
using static GardeningWithMines.Properties.Settings;
using static GardeningWithMines.Model.ControlsManager;
using System.Windows.Media;
using System.Windows.Input;

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
            //List<Button> buttons = new List<Button>();
            
            for (int i = 0; i < Default.MapRow; i++)
            {
                for (int j = 0; j < Default.MapColumn; j++)
                {
                    /*buttons.Add(new Button()
                    {
                        Style = Application.Current.Resources["BlockButtonStyle"] as Style,
                        Content = "8"
                    });*/

                    BlockButtons[i, j] = new Button()
                    {
                        Style = Application.Current.Resources["BlockButtonStyle"] as Style,
                        //Content = MapManager.MineMap[i, j].ToString(),
                        Padding = new Thickness(0)
                    };
                    int ti = i, tj = j;
                    BlockButtons[i, j].PreviewMouseLeftButtonDown += (s, e) => 
                        { MapManager.Click(ti, tj); };
                    BlockButtons[i, j].PreviewMouseRightButtonUp += (s, e) =>
                        {
                            if(BlockButtons[ti, tj].Content == null)
                            {
                                BlockButtons[ti, tj].FontFamily = new FontFamily(Default.IconFontFamily);
                                BlockButtons[ti, tj].Content = Default.FlagCharacter;
                                BlockButtons[ti, tj].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
                            }
                            else
                            {
                                BlockButtons[ti, tj].FontFamily = new FontFamily(Default.BlockFontFamily);
                                BlockButtons[ti, tj].Content = null;
                                BlockButtons[ti, tj].SetBinding(Button.FontSizeProperty, BlockFontSizeBinding);
                            }
                        };

                    MapGrid.Children.Add(BlockButtons[i, j]);
                    Grid.SetRow(BlockButtons[i, j], i);
                    Grid.SetColumn(BlockButtons[i, j], j);
                }
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

        private void UpdateWindow(double delta = 0)
        {
            // double deltaHeight = 48;
            // double deltaWidth = 28;
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

        private void RecalculateBlockFontSize()
        {
            double blockFontSize = (Default.MapHeight < MapGrid.MinHeight ? MapGrid.MinHeight : Default.MapHeight)
                / Default.MapRow * Default.FontSizeRatio;
            Default.BlockFontSize = blockFontSize < Default.BlockMaxFontSize ?
                blockFontSize : Default.BlockMaxFontSize;
            Default.IconFontSize = Default.BlockFontSize * Default.IconSizeRatio;
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case Key.A:
                    AboutBox1 aboutDialog = new AboutBox1();
                    aboutDialog.Show();
                    break;
                case Key.F12:
                    SimpleConfigWindow configWindow = new SimpleConfigWindow();
                    configWindow.Show();
                    break;
                /*case Key.H:
                    HelpWindow helpDialog = new HelpWindow();
                    helpDialog.Show();
                    break;*/
            }

        }
    }
}
