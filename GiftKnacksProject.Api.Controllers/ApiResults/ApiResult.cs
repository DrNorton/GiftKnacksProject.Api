using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Controllers.ApiResults
{
    [Serializable]
    public class ApiResult: IHttpActionResult    {
        private readonly HttpRequestMessage _request;
        private readonly int _errorCode;
        private readonly string _errorMessage;
        private readonly object _result;

        public ApiResult(HttpRequestMessage request, int errorCode, string errorMessage, object result)
        {
            _request = request;
            _errorCode = errorCode;
            _errorMessage = errorMessage;
            _result = result;
        }

        public int ErrorCode
        {
            get { return _errorCode; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public object Result
        {
            get { return _result; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                
                Content = new StringContent(JsonConvert.SerializeObject(this)),
                RequestMessage = _request
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           
            return response;
        }
    }
}