using System.Collections.Generic;

namespace WeatherForecast.WebUI.Domain
{
    public class Forecast
    {
        public List<DayForecast> DayForecasts { get; set; }

        public Forecast()
        {
            DayForecasts = new List<DayForecast>();
        }
    }
}