﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace GuestlogixTestXF
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? true : false;
        }
    }
}
