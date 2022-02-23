﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Kagic_UI.ViewModels.UtilitiesVM
{
    public class clsUnitNumberConverter : IValueConverter
    {
        private const string URL_IMAGE_NUMBER = "/Assets/Images/Numbers/*.png";
        public static String ConvertUnitNumberToURLString(int value)
        {
            if(value >= 10)
            {
                value %= 10;
            }
            String url = URL_IMAGE_NUMBER.Replace("*", value.ToString());
            return url;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConvertUnitNumberToURLString((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
