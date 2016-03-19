using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Hubs;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/push")]
    public class RealTimePushController : CustomApiController
    {
        private readonly IUserOnlineSignalService _userOnlineSignalService;

        public RealTimePushController(IUserOnlineSignalService userOnlineSignalService)
        {
            _userOnlineSignalService = userOnlineSignalService;
        }

        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Send")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Send(JArray notifications)
        {
            string data = notifications.ToString();
            //Фигачим с базовым классом
            var receivedNotification = JsonConvert.DeserializeObject<List<Notification>>(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            var context = GlobalHost.ConnectionManager.GetHubContext<OnlineHub>();
            foreach (var notification in receivedNotification)
            {
               var findedUser= _userOnlineSignalService.FindUser(notification.Info.OwnerId);
                if (findedUser != null)
                {
                    foreach (var connection in findedUser.Connections)
                    {
                        context.Clients.Client(connection).showMessage(notification);
                    }
                }
            }
           
           
            return SuccessApiResult(receivedNotification);
        }
    }
}
