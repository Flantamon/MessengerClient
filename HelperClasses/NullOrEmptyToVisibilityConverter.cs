using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Messenger.HelperClasses
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, является ли значение null или пустой строкой
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return Visibility.Collapsed; // Возвращаем Collapsed если значение null или пустая строка
            }
            return Visibility.Visible; // Возвращаем Visible если значение не пустое
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
