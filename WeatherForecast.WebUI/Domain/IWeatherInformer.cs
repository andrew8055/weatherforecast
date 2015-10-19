using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherForecast.WebUI.Domain
{
    public interface IWeatherInformer
    {
        Forecast GetWeather(string city);
    }
}
