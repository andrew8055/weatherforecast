using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WeatherForecast.WebUI.Extensions
{
    public static class StringExtensions
    {
        public static string MmHgConversion(this string valInHpa)
        {
            double valConv;

            if (double.TryParse(valInHpa, out valConv) || double.TryParse(valInHpa.Replace('.', ','), out valConv))
            {
                valConv = Math.Truncate(valConv / 1.333);

                return valConv.ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        public static bool TryParse2DoubleAnyCulture(this string val)
        {
            double valConv;

            return double.TryParse(val, out valConv) || double.TryParse(val.Replace('.', ','), out valConv);
        }
    }
}