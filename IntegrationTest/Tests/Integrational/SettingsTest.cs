using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace IntegrationTest.Tests.Integrational
{
    [TestFixture]
    public class SettingsTest:BaseTestClass
    {
        [Test]
        public async void TestSettingsCompare()
        {
            var settingsResult=await base.HttpGet("/api/test/getsettings");
            if (settingsResult.ErrorCode != 0)
            {
                throw new Exception(settingsResult.ErrorMessage);
            }

            var allSettingsNormal = true;
            var settings = ConfigurationManager.AppSettings;
            foreach (var sett in settings.AllKeys)
            {
                var value = settings.Get(sett);
                var valueFromApi=settingsResult.Result.FirstOrDefault(x => x.Key == sett);
                if (valueFromApi == null)
                {
                    allSettingsNormal = false;
                }
                else
                {
                    if (value != valueFromApi.Value)
                    {
                        allSettingsNormal = false;
                    }
                }
               
            }
            Assert.IsTrue(allSettingsNormal);
        }
    }
}
