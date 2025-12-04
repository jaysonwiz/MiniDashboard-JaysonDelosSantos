using MiniDashboard.App.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniDashboard.App.Converters
{
    public class DialogTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DialogType type)
            {
                return type switch
                {
                    DialogType.Information => Brushes.DodgerBlue,
                    DialogType.Warning => Brushes.Orange,
                    DialogType.Error => Brushes.Red,
                    DialogType.Question => Brushes.Green,
                    DialogType.Custom => Brushes.Gray,
                    _ => Brushes.Black
                };
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
