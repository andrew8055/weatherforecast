using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WeatherForecast.WebUI.Extensions;

namespace WeatherForecast.WebUI.Domain
{
    public class YandexWeather : IWeatherInformer
    {
        private const string TemperaturePattern = "/x:forecast/x:day[@date='{0}']/x:day_part[@type='{1}']/x:temperature",
            WindSpeedPattern = "/x:forecast/x:day[@date='{0}']/x:day_part[@type='{1}']/x:wind_speed",
            HumidityPattern = "/x:forecast/x:day[@date='{0}']/x:day_part[@type='{1}']/x:humidity",
            PressurePattern = "/x:forecast/x:day[@date='{0}']/x:day_part[@type='{1}']/x:pressure[@units='torr']";

        public Forecast GetWeather(string city)
        {
            var forecast = new Forecast();
            string id = GetCityId(city);

            if (string.IsNullOrEmpty(id))
                return forecast;
            
            var dates = new List<DateTime>
            {
                DateTime.Now,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            };

            var cityDocNav = GetCityDocNavigator(id);

            foreach (var date in dates)
            {
                string dateConv = date.ToString("yyyy-MM-dd");

                forecast.DayForecasts.Add(new DayForecast
                {
                    Date = date,
                    ForecastProvider = ForecastProviders.YandexWeather,
                    PressureDay = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, PressurePattern, dateConv, true),
                    TemperatureDay = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, TemperaturePattern, dateConv, true),
                    HumidityDay = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, HumidityPattern, dateConv, true),
                    WindSpeedDay = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, WindSpeedPattern, dateConv, true),
                    PressureNight = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, PressurePattern, dateConv, false),
                    TemperatureNight = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, TemperaturePattern, dateConv, false),
                    HumidityNight = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, HumidityPattern, dateConv, false),
                    WindSpeedNight = GetWeatherCharacteristic(cityDocNav.Key, cityDocNav.Value, WindSpeedPattern, dateConv, false),
                });
            }

            return forecast;
        }

        public virtual KeyValuePair<XPathNavigator, XmlNamespaceManager> GetCityDocNavigator(string id)
        {
            var cityForecastXPath = new XPathDocument(string.Format("http://export.yandex.ru/weather-ng/forecasts/{0}.xml", id));
            XPathNavigator nav = cityForecastXPath.CreateNavigator();
            var xmlns = new XmlNamespaceManager(new NameTable());

            xmlns.AddNamespace("x", "http://weather.yandex.ru/forecast");

            return new KeyValuePair<XPathNavigator, XmlNamespaceManager>(nav, xmlns);
        }

        public virtual string GetCityId(string city)
        {
            string id = string.Empty;
            var citiesXPath = new XPathDocument("https://pogoda.yandex.ru/static/cities.xml");
            XPathNavigator citiesNavigator = citiesXPath.CreateNavigator();

            XPathNodeIterator citiesIt = citiesNavigator.Select("/cities/country/city");

            while (citiesIt.MoveNext())
            {
                var cityToLower = citiesIt.Current.Value.ToLower();

                if (cityToLower.Equals(city.ToLower()) || cityToLower.StartsWith(string.Format("{0},", city)))
                    id = citiesIt.Current.GetAttribute("id", string.Empty);
            }

            return id;
        }

        public virtual string GetWeatherCharacteristic(XPathNavigator navigator, XmlNamespaceManager xmlns, string pattern, string date, bool isDay)
        {
            XPathNodeIterator iterator = navigator.Select(string.Format(pattern, date, isDay ? "day_short" : "night_short"), xmlns);

            iterator.MoveNext();

            string val = iterator.Current.Value;

            if (val.TryParse2DoubleAnyCulture())
                return val;
             
            return string.Empty;
        }
    }
}