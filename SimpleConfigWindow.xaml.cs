using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Managers.ControlsManager;
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
        private double rawThickness = -1;

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
            Default.BlockBorderThickness = new Thickness(rawThickness);
            if (ColorSelecter.SelectedIndex < 0)
            {
                ColorSelecter.SelectedIndex = 0;
            }
            Default.BlockBackgroundBrush = ColorsList[ColorSelecter.SelectedIndex].Color.ToString();
            Default.Save();
            string info = $"[SavedToken: {GetCount}] ";
            Title = info + title;
            GameRefresh();
        }

        private void BorderThicknessTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            BorderThicknessTextBox.Text = Default.BlockBorderThickness.Left.ToString();
        }

        private void BorderThicknessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyButton.IsEnabled = double.TryParse(BorderThicknessTextBox.Text, out rawThickness)
                && rawThickness >= 0 && rawThickness <= 5;
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

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ColorConverter colorConverter = new ColorConverter();
            object _currentColor = colorConverter.ConvertFromInvariantString(Default.BlockBackgroundBrush);
            Color currentColor = _currentColor == null ? Colors.LightBlue : ((Color)_currentColor);
            ColorSelecter.SelectedIndex = ColorsList.FindIndex(p => p.Color == currentColor);
        }

        private void CountTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            CountTextBox.Text = Default.MinesCount.ToString();
        }

        private void CountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyButton.IsEnabled = int.TryParse(CountTextBox.Text, out currentCount) && IsInRange();
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
            ApplyButton.IsEnabled = int.TryParse(RowTextBox.Text, out currentRow) && IsInRange();
        }

        private int SetCount(int value) => count = value;
    }
}