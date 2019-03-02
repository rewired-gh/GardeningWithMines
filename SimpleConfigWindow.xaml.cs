using System.Windows;
using System.Windows.Controls;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines
{
    /// <summary>
    /// SimpleConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleConfigWindow : Window
    {
        private readonly string title = "Simple Config Window";
        private int count;
        private int currentRow = -1, currentColumn = -1, currentCount = -1;

        public SimpleConfigWindow()
        {
            InitializeComponent();
            SetCount(-1);
        }

        private int GetCount => ++count < 10 ? count : (count = 0);

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Default.MapRow = currentRow;
            Default.MapColumn = currentColumn;
            Default.MinesCount = currentCount;
            Default.Save();
            string info = $"[SavedToken: {GetCount}] ";
            Title = info + title;
            GameRefresh();
        }

        private void ColumnTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ColumnTextBox.Text = Default.MapColumn.ToString();
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

        private void CountTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            CountTextBox.Text = Default.MinesCount.ToString();
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

        private bool IsInRange()
        {
            return currentRow > 0 && currentColumn > 0 && currentCount > 0 &&
                currentRow * currentColumn > currentCount;
        }

        private void RowTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            RowTextBox.Text = Default.MapRow.ToString();
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

        private int SetCount(int value) => count = value;
    }
}