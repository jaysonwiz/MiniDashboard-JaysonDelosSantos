using FontAwesome.WPF;
using MiniDashboard.App.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MiniDashboard.App.Converters
{
    public class DialogTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DialogType type)
            {
                return type switch
                {
                    DialogType.Information => FontAwesomeIcon.InfoCircle,
                    DialogType.Warning => FontAwesomeIcon.ExclamationTriangle,
                    DialogType.Error => FontAwesomeIcon.TimesCircle,
                    DialogType.Question => FontAwesomeIcon.QuestionCircle,
                    _ => FontAwesomeIcon.Circle
                };
            }
            return FontAwesomeIcon.Circle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
