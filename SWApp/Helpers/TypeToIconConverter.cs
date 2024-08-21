using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SWApp.Helpers
{
    public class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value as string;
            string imagePath = default;

            switch (type?.ToLower())
            {
                case "assembly":
                    imagePath = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\assembly.bmp";
                    //imagePath = "C:\\Users\\BIP\\source\\repos\\SWAddinByBS\\SWApp\\assets\\assembly.bmp";
                    break;
                case "part":
                    imagePath = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\part.bmp";
                    //imagePath = "C:\\Users\\BIP\\source\\repos\\SWAddinByBS\\SWApp\\assets\\part.bmp";
                    break;
                case "sheet":
                    imagePath = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\sheet.bmp";
                    //imagePath = "C:\\Users\\BIP\\source\\repos\\SWAddinByBS\\SWApp\\assets\\sheet.bmp";
                    break;
            }

            return new BitmapImage(new Uri(imagePath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
