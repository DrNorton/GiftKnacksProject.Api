﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GiftKnacksProject.Api.Controllers.ApiResults;
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
            return SuccessApiResult(list);
        }
    }
}