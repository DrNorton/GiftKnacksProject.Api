using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using GiftKnacksProject.Api;
using GiftKnacksProject.Api.Controllers.Controllers;
using IntegrationTest.Helpers;
using IntegrationTest.Models;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTest.Tests.Integrational
{
    public class BaseTestClass
    {
        protected TestServer _server;

        [SetUp]
        public void SetUp()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver),
       new TestAssembliesResolver());
            _server = TestServer.Create<Startup>();
        }

        [TearDown]
        public async void TearDown()
        {
            _server.Dispose();
        }

        protected async Task<ApiStandartResponse<List<ApiSetting>>> HttpGet(string pathRoute)
        {
            var response= await _server.HttpClient.GetAsync(pathRoute);
            var result = await response.Content.ReadAsStringAsync();
            var serial= JsonConvert.DeserializeObject<ApiStandartResponse<List<ApiSetting>>>(result);
            return serial;
        }

        protected async Task<ApiStandartResponse<T>> HttpPost<T>(string pathRoute,string serializatedInputParam)
        {
            var response = await _server.HttpClient.PostAsync(pathRoute,new StringContent(serializatedInputParam,Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiStandartResponse<T>>(result);
        }

    }
}
