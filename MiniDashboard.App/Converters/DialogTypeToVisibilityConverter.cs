using MiniDashboard.App.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniDashboard.App.Converters
{
    public class DialogTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DialogType type && parameter is string param)
            {
                return param switch
                {
                    "OK" => type == DialogType.Information || type == DialogType.Custom ? Visibility.Visible : Visibility.Collapsed,
                    "Cancel" => type == DialogType.Warning || type == DialogType.Error ? Visibility.Visible : Visibility.Collapsed,
                    "Yes" => type == DialogType.Question ? Visibility.Visible : Visibility.Collapsed,
                    "No" => type == DialogType.Question ? Visibility.Visible : Visibility.Collapsed,
                    _ => Visibility.Collapsed
                };
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
