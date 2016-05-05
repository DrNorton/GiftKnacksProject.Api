using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using GiftKnacksProject.Api;
using GiftKnacksProject.Api.Controllers.Models;
using IntegrationTest.Helpers;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTest
{
    [TestFixture]
    public class IntegrationTests
    {
       [SetUp]
        public void SetUp()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver),
       new TestAssembliesResolver());
            
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public async void TestMethod1()
        {
            using (var server = TestServer.Create<Startup>())
            {
                
                // Execute test against the web API.
                var test = JsonConvert.SerializeObject(new IdModel() {Id = 1});
                var result = await server.HttpClient.GetAsync("/api/test/getSettings");
            }

           
            Assert.IsTrue(true);
            
        }
    }
}
