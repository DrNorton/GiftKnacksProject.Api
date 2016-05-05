using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using GiftKnacksProject.Api;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest
{
    [TestClass]
    public class IntegrationTests
    {
        public class TestAssembliesResolver : IAssembliesResolver
        {
            public ICollection<Assembly> GetAssemblies()
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver),
        new TestAssembliesResolver());
           
            using (var webApp = WebApp.Start<Startup>("http://localhost:9443/"))
            {
                // Execute test against the web API.
                webApp.Dispose();
                Assert.IsTrue(true);
            }
        }
    }
}
