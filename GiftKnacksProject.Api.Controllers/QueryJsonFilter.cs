using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Controllers
{
    public class QueryJsonFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        Type _type;
        string _queryStringKey;
        public QueryJsonFilter(Type type, string queryStringKey)
        {
            _type = type;
            _queryStringKey = queryStringKey;
        }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var json = actionContext.Request.RequestUri.ParseQueryString()[_queryStringKey];
            actionContext.ActionArguments[_queryStringKey] = JsonConvert.DeserializeObject(json, _type);
        }
    }
}