using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.XPath;
using WeatherForecast.WebUI.Extensions;

namespace WeatherForecast.WebUI.Domain
{
    public class OpenWeather : IWeatherInformer
    {
        private const string TemperaturePattern = "/weatherdata/forecast/time[@day='{0}']/temperature",
            WindSpeedPattern = "/weatherdata/forecast/time[@day='{0}']/windSpeed",
            PressurePattern = "/weatherdata/forecast/time[@day='{0}']/pressure",
            HumidityPattern = "/weatherdata/forecast/time[@day='{0}']/humidity";

        public Forecast GetWeather(string city)
        {
            var forecast = new Forecast();
            var dates = new List<DateTime>
            {
                DateTime.Now,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            };

            var cityDocNav = GetCityDocNavigator(city);

            foreach (var date in dates)
            {
                var dateConv = date.ToString("yyyy-MM-dd");

                forecast.DayForecasts.Add(new DayForecast
                {
                    Date = date,
                    ForecastProvider = ForecastProviders.OpenWeather,
                    TemperatureDay = GetWeatherCharacteristic(cityDocNav, TemperaturePattern, dateConv, "day"),
                    TemperatureNight = GetWeatherCharacteristic(cityDocNav, TemperaturePattern, dateConv, "night"),
                    WindSpeedDay = GetWeatherCharacteristic(cityDocNav, WindSpeedPattern, dateConv, "mps"),
                    PressureDay = GetWeatherCharacteristic(cityDocNav, PressurePattern, dateConv, "value").MmHgConversion(),
                    HumidityDay = GetWeatherCharacteristic(cityDocNav, HumidityPattern, dateConv, "value"),
                });
            }

            return forecast;
        }

        public virtual XPathNavigator GetCityDocNavigator(string city)
        {
            var xPathDoc =
                new XPathDocument(
                    string.Format(
                        "http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&mode=xml&units=metric&cnt=4&appid={1}",
                        city, WebConfigurationManager.AppSettings["OpenWeatherMapId"]));
            XPathNavigator nav = xPathDoc.CreateNavigator();

            return nav;
        }

        public virtual string GetWeatherCharacteristic(XPathNavigator nav, string pattern, string date, string att)
        {
            var iterator = nav.Select(string.Format(pattern, date));

            iterator.MoveNext();

            var val = iterator.Current.GetAttribute(att, string.Empty);

            if (val.TryParse2DoubleAnyCulture())
                return val;

            return string.Empty;
        }
    }
}