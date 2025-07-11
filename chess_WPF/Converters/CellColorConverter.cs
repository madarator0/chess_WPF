using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace chess_WPF
{
    public class CellColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tag = value.ToString();
            string[] coordinates = tag.Split(',');
            int row = int.Parse(coordinates[0]);
            int column = int.Parse(coordinates[1]);

            if ((row + column) % 2 == 0)
            {
                return Brushes.LightGreen;
            }
            else
            {
                return Brushes.DimGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
