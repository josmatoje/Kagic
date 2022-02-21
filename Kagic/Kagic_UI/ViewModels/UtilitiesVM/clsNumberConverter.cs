using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Kagic_UI.ViewModels.UtilitiesVM
{
    public class clsNumberConverter : IValueConverter
    {
        private const string URL_IMAGE_NUMBER = "/Assets/Images/Numbers/*.png";
        public static String ConvertNumberToURLString(int value)
        {
            String url = URL_IMAGE_NUMBER.Replace("*", value.ToString());
            return url;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConvertNumberToURLString((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

