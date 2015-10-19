using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeatherForecast.WebUI.Controllers;

namespace WeatherForecast.WebUI.Domain
{
    public interface IWeatherAggregation
    {
        Dictionary<string, Forecast> GetForecasts(HttpCookie citiesCookie);
    }
}
