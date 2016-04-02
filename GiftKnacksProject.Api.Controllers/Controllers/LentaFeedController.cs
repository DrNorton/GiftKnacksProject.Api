using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Lenta")]

    public class LentaFeedController : CustomApiController
    {
        private readonly INotificationService _notificationService;

        public LentaFeedController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetLenta")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetLenta()
        {
           var userId = long.Parse(User.Identity.GetUserId());
           return SuccessApiResult(await _notificationService.GetLenta(userId));

        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("Delete")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> DeleteNotificationFromLenta(StringIdModel id)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            await _notificationService.DeleteNotificationFromLenta(id.Id);
            return SuccessApiResult(true);

        }
    }
}
