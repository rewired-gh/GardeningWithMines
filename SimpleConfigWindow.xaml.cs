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
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorToOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string colorType = parameter.ToString();
            switch (colorType)
            {
                case "Black":
                    return Default.BlockForegroundBrush == Properties.Resources.ClearBlack;

                case "White":
                    return Default.BlockForegroundBrush == Properties.Resources.ClearWhite;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool isChecked = (bool)value;
            if (isChecked)
            {
                string colorType = parameter.ToString();
                switch (colorType)
                {
                    case "Black":
                        return Properties.Resources.ClearBlack;

                    case "White":
                        return Properties.Resources.ClearWhite;
                }
            }
            return Default.BlockForegroundBrush;
        }
    }

    /// <summary>
    /// SimpleConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleConfigWindow : Window
    {
        private int currentRow = -1, currentColumn = -1, currentCount = -1;
        private double rawThickness = -1;

        public SimpleConfigWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Default.MapRow = currentRow;
            Default.MapColumn = currentColumn;
            Default.MinesCount = currentCount;
            Default.Save();
            GameRefresh();
        }

        private void BorderThicknessTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            BorderThicknessTextBox.Text = Default.BlockBorderThickness.Left.ToString();
        }

        private void BorderThicknessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(BorderThicknessTextBox.Text, out rawThickness)
                && rawThickness >= 0 && rawThickness <= 5)
            {
                Default.BlockBorderThickness = new Thickness(rawThickness);
                Default.Save();
            }
        }

        private void ColorSelecter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorSelecter.SelectedIndex < 0)
            {
                ColorSelecter.SelectedIndex = 0;
            }
            Default.BlockBackgroundBrush = ColorsList[ColorSelecter.SelectedIndex].Color.ToString();
            Default.Save();
        }

        private void ColumnTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ColumnTextBox.Text = Default.MapColumn.ToString();
        }

        private void ColumnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyButton.IsEnabled = int.TryParse(ColumnTextBox.Text, out currentColumn) && IsInRange();
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
            //DEBUG
            //RowTextBox.Text = DWB.IsChecked.ToString();
            //DEBUG
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
    }
}