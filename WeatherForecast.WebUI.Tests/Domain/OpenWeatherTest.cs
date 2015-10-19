using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Moq;
using NUnit.Framework;
using WeatherForecast.WebUI.Domain;

namespace WeatherForecast.WebUI.Tests.Domain
{
    public class OpenWeatherTest
    {
        [Test]
        public void GetWeatherReturnForecast()
        {
            var mock = new Mock<OpenWeather>();

            mock.Setup(x => x.GetCityDocNavigator(It.IsAny<string>())).Returns(It.IsAny<XPathNavigator>());

            mock.Setup(
                x =>
                    x.GetWeatherCharacteristic(It.IsAny<XPathNavigator>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>())).Returns("characteristic");

            var forecast = mock.Object.GetWeather("city");

            Assert.AreEqual(forecast.DayForecasts.Count, 3);
            Assert.NotNull(forecast.DayForecasts.FirstOrDefault(x=>x.Date.Date.Equals(DateTime.Now.Date)));
            Assert.NotNull(forecast.DayForecasts.FirstOrDefault(x => x.Date.Date.Equals(DateTime.Now.AddDays(2).Date)));
        }
    }
}
