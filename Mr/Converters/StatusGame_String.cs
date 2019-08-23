using Mr.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mr.Converters
{
    class StatusGame_String : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((GameStatus)value == GameStatus.InGame) return "o-o";
            else if ((GameStatus)value == GameStatus.Win) return "^-^";
            else return "x-x";
        }
        //---метод Convert

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        //--метод ConvertBack
    }
}
