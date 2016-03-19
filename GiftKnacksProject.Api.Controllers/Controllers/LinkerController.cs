using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnackProject.NotificationTypes;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Services.Interfaces;

using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/linker")]
    [EnableCors(origins: "http://giftknackapi.azurewebsites.net", headers: "*", methods: "*")]
    public class LinkerController:CustomApiController
    {
        private readonly ILinkRepository _linkRepository;
        private readonly INotificationService _notificationService;

        public LinkerController(ILinkRepository linkRepository,INotificationService notificationService)
        {
            _linkRepository = linkRepository;
            _notificationService = notificationService;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("link")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> LinkWithGift(WishGiftLinkDto participantDto)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var ownerWishUserId=await _linkRepository.LinkWithGift(userId, participantDto.WishId,participantDto.GiftId);
            await _notificationService.SentNotificationToQueue(new JoinQueueNotification()
            {
                CreatorId = userId,
                OwnerWish=ownerWishUserId,
                TargetWishId = participantDto.WishId,
                TargetGiftId = participantDto.GiftId
            });
            return EmptyApiResult();
        }

       
    }
}
