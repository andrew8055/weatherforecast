using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherForecast.WebUI.Domain
{
    public class DayForecast
    {
        public DateTime Date { get; set; }

        public string ForecastProvider { get; set; }

        public string TemperatureDay { get; set; }

        public string WindSpeedDay { get; set; }

        public string HumidityDay { get; set; }

        public string PressureDay { get; set; }

        public string TemperatureNight { get; set; }

        public string WindSpeedNight { get; set; }

        public string HumidityNight { get; set; }

        public string PressureNight { get; set; }
    }
}