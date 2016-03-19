using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace GiftKnacksProject.Api.Controllers.ApiResults
{
    public class CustomApiController : ApiController
    {
        public ApiResult ErrorApiResult(int errorCode, string message)
        {
            return new ApiResult(Request, errorCode, message, null);
        }

        public ApiResult ErrorApiResult(int errorCode, IEnumerable<string> messages)
        {
            var result = new StringBuilder();
            foreach (var message in messages)
            {
                result.Append(message);
                
            }

            return new ApiResult(Request, errorCode, result.ToString(), null);
        }

       
        public ApiResult EmptyApiResult()
        {
            return new ApiResult(Request, 0, null, null);
        }

        public ApiResult SuccessApiResult(object result)
        {
            return new ApiResult(Request, 0, null, result);
        }
    }
}