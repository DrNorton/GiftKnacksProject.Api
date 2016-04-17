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
            var list=new List<Tuple<string,string>>();
            var settings=ConfigurationManager.AppSettings;
            foreach (var sett in settings.AllKeys)
            {
               list.Add(new Tuple<string, string>(sett, settings.Get(sett)));
            }
            var strings = ConfigurationManager.ConnectionStrings;

            

            return SuccessApiResult(list);
        }




        [System.Web.Http.Route("test")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Test(IdModel id)
        {
            return SuccessApiResult(new TestResult() {Id=id.Id});
        }


        
    }

    public class TestResult
    {
        public long? Id { get; set; }
    }
}
