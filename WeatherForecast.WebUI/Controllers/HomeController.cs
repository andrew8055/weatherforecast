using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Win32.SafeHandles;
using NLog;
using WeatherForecast.WebUI.Domain;

namespace WeatherForecast.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IWeatherAggregation weatherAggregation;

        public HomeController(IWeatherAggregation _weatherAggregation)
        {
            weatherAggregation = _weatherAggregation;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var citiesCookie = Request.Cookies["cities"];

            return View(weatherAggregation.GetForecasts(citiesCookie));
        }

    }
}
