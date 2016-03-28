using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnackProject.NotificationTypes;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;
using GiftKnacksProject.Api.EfDao.Base;
using GiftKnacksProject.Api.Services.Interfaces;
using GiftKnacksProject.Api.Services.Services;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Wish")]

    public class WishController : CustomApiController
    {
        private readonly IWishRepository _wishRepository;
 
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        private readonly ILinkRepository _linkRepository;

        public WishController(IWishRepository wishRepository,ICountryRepository countryRepository,IFileService fileService,INotificationService notificationService)
        {
            _wishRepository = wishRepository;
            _fileService = fileService;
            _notificationService = notificationService;
        }

        [System.Web.Http.Route("Getall")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetAll(FilterDto filter)
        {
            filter.StatusCode = 0;
            var result = await _wishRepository.GetWishes(filter);
            return SuccessApiResult(result);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetByUser")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetByUser(FilterDto filter)
        {
            if (filter.UserId == null)
            {
                filter.UserId = long.Parse(User.Identity.GetUserId());
            }
            else
            {
                filter.UserId = (long)filter.UserId;
            }
            var wishes = await _wishRepository.GetWishes(filter);
            return SuccessApiResult(wishes);
        }

        [System.Web.Http.Route("Get")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Get(IdModel id)
        {
            if (id == null)
            {
                return ErrorApiResult(300, "Не передан id.Либо левый формат");
            }
            var result = await _wishRepository.GetWish((long) id.Id);
            if (result == default(WishDto))
            {
                return ErrorApiResult(300, "Не найден");
            }
            return SuccessApiResult(result);
        }


        [System.Web.Http.Route("close")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Close(IdModel model)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            await _wishRepository.CloseWish((long)model.Id, userId,model.CloserId);
            await
                _notificationService.SentNotificationToQueue(new CloseItemQueueNotification()
                {
                    CreatorId = userId,
                    TargetType = "wish",
                    ClosedItemId= (long)model.Id
                });
            if (model.CloserId != null)
            {
                await
               _notificationService.SentNotificationToQueue(new TotalClosedQueueNotification()
               {
                   CreatorId = userId,
                   CloserId =(long)model.CloserId,
                   ClosedWishId=(long)model.Id
               });
            }
          
            return SuccessApiResult(null);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetEmptyWish")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetEmptyWish()
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var emptyWish = await _wishRepository.GetEmptyDtoWithAdditionalInfo(userId);
            
            return SuccessApiResult(emptyWish);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("AddWish")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddWish(ImagedWishDto wish)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            if (wish.Image != null)
            {
                wish.ImageUrl = _fileService.SaveBase64FileReturnUrl(FileType.Image, wish.Image.Type, wish.Image.Result);
            }
             var result=await _wishRepository.AddWish(userId,wish);
         //    await _feedService.AddActivityFeed(userId);
             return SuccessApiResult(new IdModel() { Id = result });
        }



    }
}
