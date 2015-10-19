using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Moq;
using NUnit.Framework;
using WeatherForecast.WebUI.Domain;

namespace WeatherForecast.WebUI.Tests.Domain
{
    public class YandexWeatherTest
    {
        private readonly YandexWeather _yaWeather = new YandexWeather();
        private const string NoConnection = "no connection";

        [Test]
        public void GetWeatherReturnEmptyForecastWhenCityNotFound()
        {
            var mock = new Mock<YandexWeather>();

            mock.Setup(x => x.GetCityId(It.IsAny<string>())).Returns(string.Empty);

            var forecast = mock.Object.GetWeather("Moskva");

            Assert.NotNull(forecast);
            Assert.NotNull(forecast.DayForecasts);
            Assert.AreEqual(forecast.DayForecasts.Count, 0);
        }

        [Test]
        public void GetWeatherReturnForecastSuccess()
        {
            var mock = new Mock<YandexWeather>();

            mock.Setup(x => x.GetCityId(It.IsAny<string>())).Returns("6145");

            mock.Setup(x => x.GetCityDocNavigator(It.IsAny<string>()))
                .Returns(new KeyValuePair<XPathNavigator, XmlNamespaceManager>(It.IsAny<XPathNavigator>(),
                    It.IsAny<XmlNamespaceManager>()));

            mock.Setup(
                x =>
                    x.GetWeatherCharacteristic(It.IsAny<XPathNavigator>(), It.IsAny<XmlNamespaceManager>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<string>());

            var forecast = mock.Object.GetWeather("some city");

            Assert.AreEqual(forecast.DayForecasts.Count, 3);
            Assert.NotNull(forecast.DayForecasts.FirstOrDefault(x=>x.Date.Date.Equals(DateTime.Now.Date)));
            Assert.NotNull(forecast.DayForecasts.FirstOrDefault(x => x.Date.Date.Equals(DateTime.Now.AddDays(2).Date)));
            Assert.AreEqual(forecast.DayForecasts.Count(x=>x.ForecastProvider.Equals(ForecastProviders.YandexWeather)), 3);
        }

        [Test]
        public void GetCityIdReturnId()
        {
            var cityId = CallGetCityId("данилов");

            if(cityId != NoConnection)
                Assert.AreEqual(cityId, "27235");
        }

        [Test]
        public void GetCityIdReturnStringEmpty()
        {
            var cityId = CallGetCityId("данил");

            Assert.AreEqual(cityId, string.Empty);
        }

        [Test]
        public void GetCityIdReturnIdFromCompositeCity()
        {
            var cityId = CallGetCityId("нью-йорк");

            if(cityId != NoConnection)
                Assert.AreEqual(cityId, "72503");
        }

        private string CallGetCityId(string city)
        {
            try
            {
                return _yaWeather.GetCityId(city);
            }
            catch (WebException ex)
            {
                //there is no coonection
            }

            return NoConnection;
        }
    }
}
