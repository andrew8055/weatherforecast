using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WeatherForecast.WebUI.Domain;

namespace WeatherForecast.WebUI.Tests.Domain
{
    public class WeatherAggregationTest
    {
        private readonly WeatherAggregation _weatherAgg =
            new WeatherAggregation(new List<IWeatherInformer>() {new YandexWeather()});

        [Test]
        public void GetForecastsReturnEmptyForecastsWhenCookieIsNull()
        {
            var forecasts = _weatherAgg.GetForecasts(null);

            Assert.AreEqual(forecasts.Count, 0);
        }

        [Test]
        public void YaWeatherNotFoundCityReturnTrue()
        {
            string city = "Omsk";

            var isYaWeatherNotFound = _weatherAgg.YaWeatherNotFoundCity(new Forecast(), new YandexWeather(), city,
                new Dictionary<string, Forecast>
                {
                    {city, new Forecast()}
                });

            Assert.IsTrue(isYaWeatherNotFound);
        }

        [Test]
        public void YaWeatherNotFoundCityReturnFalse()
        {
            string city = "Omsk";

            var isYaWeatherNotFound =
                _weatherAgg.YaWeatherNotFoundCity(
                    new Forecast() {DayForecasts = new List<DayForecast> {new DayForecast()}}, new YandexWeather(), city,
                    new Dictionary<string, Forecast>
                    {
                        {city, new Forecast()}
                    });

            Assert.IsFalse(isYaWeatherNotFound);
        }

        [Test]
        public void OpenWeatherNotFoundCityReturnFalse()
        {
            string city = "Omsk";

            var isYaWeatherNotFound = _weatherAgg.YaWeatherNotFoundCity(new Forecast(), new OpenWeather(), city,
                new Dictionary<string, Forecast>
                {
                    {city, new Forecast()}
                });

            Assert.IsFalse(isYaWeatherNotFound);
        }
    }
}
