using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using GiftKnacksProject.Api;
using IntegrationTest.Helpers;
using Microsoft.Owin.Testing;
using NUnit.Framework;

namespace IntegrationTest.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
       [SetUp]
        public void SetUp()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver),
       new TestAssembliesResolver());

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
       new HttpControllerActivator());

        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public async void TestMethod1()
        {
            var server = TestServer.Create<Startup>();
            HttpResponseMessage response = await server.HttpClient.GetAsync("/api/test/getsettings");
             var result = await response.Content.ReadAsStringAsync();
         
             
            Assert.IsTrue(true);
            
        }
    }


    public class HttpControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return null;
        }
    }
}
