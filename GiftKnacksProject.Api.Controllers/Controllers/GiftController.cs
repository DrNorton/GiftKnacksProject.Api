using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnackProject.NotificationTypes;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.EfDao;
using GiftKnacksProject.Api.Services.Interfaces;
using GiftKnacksProject.Api.Services.Services;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/gift")]

    public class GiftController:CustomApiController
    {
        private readonly IGiftRepository _giftRepository;
        private readonly INotificationService _notificationService;
        private readonly IFileService _fileService;

        public GiftController(IGiftRepository giftRepository,INotificationService notificationService, IFileService fileService)
        {
            _giftRepository = giftRepository;
            _notificationService = notificationService;
            _fileService = fileService;
        }

        //[System.Web.Http.Authorize]
        [System.Web.Http.Route("close")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Close(IdModel model)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            await _giftRepository.CloseGift((long)model.Id, userId);

            await
             _notificationService.SentNotificationToQueue(new CloseItemQueueNotification()
             {
                 CreatorId = userId,
                 TargetType = "gift"
             });
            return SuccessApiResult(null);
        }

        //[System.Web.Http.Authorize]
        [System.Web.Http.Route("Getall")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetAll(FilterDto filter)
        {
            filter.StatusCode = 0;//только открытые
            var result=await _giftRepository.GetGifts(filter);
            return SuccessApiResult(result);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetByUser")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetByUser(FilterDto filter)
        {
            long userId = 0;
            if (filter.UserId == null)
            {
                filter.UserId = long.Parse(User.Identity.GetUserId());
            }
            else
            {
                filter.UserId = (long)filter.UserId;
            }
            var gifts = await _giftRepository.GetGifts(filter);
            return SuccessApiResult(gifts);
        }

        [System.Web.Http.Route("Get")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Get(IdModel id)
        {
            if (id == null)
            {
                return ErrorApiResult(300, "Не передан id.Либо левый формат");
            }
            var result = await _giftRepository.GetGift((long) id.Id);
            if (result == null)
            {
                return ErrorApiResult(300, "Не найден");
            }
            return SuccessApiResult(result);
        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetEmptyGift")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetEmptyGift()
        {
            var emptyWish = await _giftRepository.GetEmptyDtoWithAdditionalInfo();
            return SuccessApiResult(emptyWish);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("AddGift")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddGift(GiftDto gift)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var id=await _giftRepository.AddGift(userId, gift);
            return SuccessApiResult(new IdModel(){Id = id});
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("UpdateGift")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UpdateGift(UpdatedGiftDto updatedGift)
        {
            var userId = long.Parse(User.Identity.GetUserId());
           
            var updatedResult = await _giftRepository.UpdateGift(userId, updatedGift);
            return SuccessApiResult(updatedResult);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetJoined")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetJoined()
        {
            var userId = long.Parse(User.Identity.GetUserId());

           // var updatedResult = await _giftRepository.UpdateGift(userId, updatedGift);
            return SuccessApiResult(true);
        }

    }
}
