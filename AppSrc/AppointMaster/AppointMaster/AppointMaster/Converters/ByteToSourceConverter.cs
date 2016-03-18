using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppointMaster.Converters
{
    class ByteToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var bytes = (byte[])value;
            if (bytes.Length < 50)
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return ImageSource.FromStream(() => new System.IO.MemoryStream((byte[])value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
