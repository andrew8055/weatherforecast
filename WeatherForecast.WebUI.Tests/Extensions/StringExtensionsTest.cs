using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WeatherForecast.WebUI.Extensions;

namespace WeatherForecast.WebUI.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Test]
        public void MmHgConversionReturnCorrectVal()
        {
            string val = "978", expectedConvVal = "734";

            var valConv = val.MmHgConversion();

            Assert.LessOrEqual(Int32.Parse(valConv) - Int32.Parse(expectedConvVal), 1);
        }

        [Test]
        public void ReturnEmptyStringWhenTryParseFail()
        {
            string val = "978f";

            var valConv = val.MmHgConversion();

            Assert.AreEqual(valConv, string.Empty);
        }

        [Test]
        public void SuccessConversionInDiffFormat()
        {
            string val = "978,5", expectedConvVal = "734";

            var valConv = val.MmHgConversion();

            Assert.LessOrEqual(Int32.Parse(valConv) - Int32.Parse(expectedConvVal), 1);
        }

        [Test]
        public void SuccessTryParseInDiffFormat()
        {
            string val1 = "500,1", val2 = "500.2", expectedVal = "375";

            Assert.AreEqual(val1.MmHgConversion(), expectedVal);
            Assert.AreEqual(val2.MmHgConversion(), expectedVal);
        }
    }
}
