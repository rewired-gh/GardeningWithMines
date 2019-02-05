using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines
{
    /// <summary>
    /// SimpleConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleConfigWindow : Window
    {
        public SimpleConfigWindow()
        {
            InitializeComponent();
        }

        private int currentRow = -1, currentColumn = -1, currentCount = -1;

        private void RowTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            RowTextBox.Text = Default.MapRow.ToString();
        }

        private void ColumnTextBox_Loaded(object sender, RoutedEventArgs e)
        {

            ColumnTextBox.Text = Default.MapColumn.ToString();
        }

        private void CountTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            CountTextBox.Text = Default.MinesCount.ToString();
        }

        private void ColumnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ColumnTextBox.Text, out currentColumn) && IsInRange())
            {
                ApplyButton.IsEnabled = true;
            }
            else
            {
                ApplyButton.IsEnabled = false;
            }
        }

        private void CountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(CountTextBox.Text, out currentCount) && IsInRange())
            {
                ApplyButton.IsEnabled = true;
            }
            else
            {
                ApplyButton.IsEnabled = false;
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Default.MapRow = currentRow;
            Default.MapColumn = currentColumn;
            Default.MinesCount = currentCount;
            Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void RowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(RowTextBox.Text, out currentRow) && IsInRange())
            {
                ApplyButton.IsEnabled = true;
            }
            else
            {
                ApplyButton.IsEnabled = false;
            }
        }

        private bool IsInRange()
        {
            return currentRow > 0 && currentColumn > 0 && currentCount > 0 &&
                currentRow * currentColumn > currentCount;
        }
    }
}
