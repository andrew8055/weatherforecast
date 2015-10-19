using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using NLog;
using WeatherForecast.WebUI.Controllers;

namespace WeatherForecast.WebUI.Domain
{
    public class WeatherAggregation : IWeatherAggregation
    {
        private List<IWeatherInformer> weatherInformers;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public WeatherAggregation(List<IWeatherInformer> _weatherInformers)
        {
            weatherInformers = _weatherInformers;
        }

        public Dictionary<string, Forecast> GetForecasts(HttpCookie citiesCookie)
        {
            var serializer = new JavaScriptSerializer();
            var forecasts = new Dictionary<string, Forecast>();
            var cities = new List<string>();

            if (citiesCookie != null)
            {
                cities = serializer.Deserialize<List<string>>(HttpUtility.UrlDecode(citiesCookie.Value));

                foreach (var city in cities)
                {
                    foreach (var weatherInformer in weatherInformers)
                    {
                        var weather = new Forecast();

                        try
                        {
                            weather = weatherInformer.GetWeather(city);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("Inner exception: {0}\r\nStack Trace: {1}", ex.InnerException, ex.StackTrace);
                        }

                        if (!forecasts.ContainsKey(city))
                            forecasts.Add(city, weather);
                        else
                        {
                            foreach (var forecast in weather.DayForecasts)
                            {
                                if (
                                    !forecasts[city].DayForecasts.Exists(
                                        x =>
                                            x.ForecastProvider.Equals(forecast.ForecastProvider) &&
                                            x.Date.Date.Equals(forecast.Date.Date)))
                                    forecasts[city].DayForecasts.Add(forecast);
                            }
                        }

                        if (YaWeatherNotFoundCity(weather, weatherInformer, city, forecasts))
                        {
                            forecasts[city].DayForecasts = new List<DayForecast>();

                            break;
                        }
                    }
                }
            }

            return forecasts;
        }

        public virtual bool YaWeatherNotFoundCity(Forecast weather, IWeatherInformer weatherInformer, string city, Dictionary<string, Forecast> forecasts)
        {
            return (weather.DayForecasts == null || weather.DayForecasts.Count.Equals(0)) &&
                   weatherInformer is YandexWeather;
        }
    }
}