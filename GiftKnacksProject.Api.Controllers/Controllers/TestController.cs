using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Dto.Dtos.Reference;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/test")]

    public class TestController:CustomApiController
    {
        public TestController()
        {
            
        }


       
        [System.Web.Http.Route("getSettings")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetSettings()
        {
            var list=new List<ApiSetting>();
            var settings=ConfigurationManager.AppSettings;
            foreach (var sett in settings.AllKeys)
            {
               list.Add(new ApiSetting(sett, settings.Get(sett)));
            }
            return SuccessApiResult(list);
        }




        [System.Web.Http.Route("test")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Test(IdModel id)
        {
            return SuccessApiResult(new TestResult() {Id=id.Id});
        }


        
    }


    public class ApiSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ApiSetting(string key,string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class TestResult
    {
        public long? Id { get; set; }
    }
}
