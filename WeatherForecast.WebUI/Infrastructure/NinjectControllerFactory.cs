using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using WeatherForecast.WebUI.Controllers;
using WeatherForecast.WebUI.Domain;

namespace WeatherForecast.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : ninjectKernel.Get(controllerType) as IController;
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IWeatherAggregation>().To<WeatherAggregation>();
            ninjectKernel.Bind<IWeatherInformer>().To<YandexWeather>();
            ninjectKernel.Bind<IWeatherInformer>().To<OpenWeather>();
            ninjectKernel.Bind<List<IWeatherInformer>>().ToConstant(new List<IWeatherInformer>
            {
                new YandexWeather(),
                new OpenWeather()
            });
        }
    }
}