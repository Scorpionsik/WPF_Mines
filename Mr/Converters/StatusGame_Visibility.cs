using Mr.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mr.Converters
{
    class StatusGame_Visibility : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((GameStatus)value == GameStatus.InGame) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
        //---метод Convert

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible) return false;
            else return true;
        }
        //--метод ConvertBack
    }
}
