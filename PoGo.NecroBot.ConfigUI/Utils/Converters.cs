using System;
using System.Windows;
using System.Windows.Data;

namespace PoGo.NecroBot.ConfigUI.Utils
{

    public class EnumToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.GetValues(value as Type);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null; // I don't care about this
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool hide = (value is string) ? string.IsNullOrEmpty(value as string)
                      : (value is bool)   ? !(bool)value
                      : (value is int)    ? (int)value == 0
                      : (value is uint)   ? (uint)value == 0
                      : (value is double) ? (double)value == 0.0
                      : null == value;
            return hide ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class VisibilityReversedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool show = (value is string) ? string.IsNullOrEmpty(value as string)
                      : (value is bool)   ? !(bool)value
                      : (value is int)    ? (int)value == 0
                      : (value is uint)   ? (uint)value == 0
                      : (value is double) ? (double)value == 0.0
                      : null == value;
            return show ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

}
