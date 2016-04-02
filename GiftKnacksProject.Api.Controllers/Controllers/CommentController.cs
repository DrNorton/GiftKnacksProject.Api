using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GiftKnackProject.NotificationTypes;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Comments;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Comment")]
    public class CommentController : CustomApiController
    {
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationService _service;
        private readonly INotificationService _notificationService;

        public CommentController(ICommentRepository commentRepository,INotificationService service,INotificationService notificationService)
        {
            _commentRepository = commentRepository;
            _service = service;
            _notificationService = notificationService;
        }

        //[System.Web.Http.Authorize]
        [System.Web.Http.Route("addtoWish")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddCommentToWish(AddCommentToWishDto model)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var insertedComment=await  _commentRepository.AddCommentToWish(model.WishId, userId, model.Text, model.ParentCommentId);
           
            await
                _notificationService.SentNotificationToQueue(new AddCommentQueueNotification()
                {
                    TargetType="wish",
                    TargetId = model.WishId,
                    CreatorId = userId,
                    CommentId=insertedComment.Id
                });

            if (model.ParentCommentId != null)
            {
                //Если это ответ к комменту то шлём юзеру владельцу коммента
                await
               _notificationService.SentNotificationToQueue(new AddCommentQueueNotification()
               {
                   TargetType = "wish",
                   TargetId = model.WishId,
                   CreatorId = userId,
                   CommentId = insertedComment.Id
               });
            }
            return SuccessApiResult(insertedComment);
        }

        

        [System.Web.Http.Route("addtoGift")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AddCommentToGift(AddCommentToGiftDto model)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var insertedComment = await _commentRepository.AddCommentToGift(model.GiftId, userId, model.Text, model.ParentCommentId);
           

            await
               _notificationService.SentNotificationToQueue(new AddCommentQueueNotification()
               {
                   TargetType = "gift",
                   TargetId = model.GiftId,
                   CreatorId = userId,
                   CommentId = insertedComment.Id
               });


            if (model.ParentCommentId != null)
            {
                //Если это ответ к комменту то шлём ещё и юзеру владельцу коммента
                await
               _notificationService.SentNotificationToQueue(new ReplyToCommentQueueNotification()
               {
                   TargetType = "gift",
                   TargetId = model.GiftId,
                   CreatorId = userId,
                   CommentId = insertedComment.Id,
                   ParentCommentId=(long)model.ParentCommentId
               });
            }
            return SuccessApiResult(insertedComment);
        }

        [System.Web.Http.Route("getbywishid")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetByWishId(GetCommentsDto idModel)
        {
           return SuccessApiResult(await _commentRepository.GetCommentListByWishId(idModel));
        }

        [System.Web.Http.Route("getwishcommentbyid")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetWishCommentById(GetCommentDto idModel)
        {
        
            return SuccessApiResult(await _commentRepository.GetWishCommentsOlderById(idModel));
        }

        [System.Web.Http.Route("getgiftcommentbyid")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetGiftCommentById(GetCommentDto idModel)
        {
            return SuccessApiResult(await _commentRepository.GetGiftCommentsOlderById(idModel));
        }

        [System.Web.Http.Route("getbygiftid")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetByGiftId(GetCommentsDto idModel)
        {
            return SuccessApiResult(await _commentRepository.GetCommentListByGiftId(idModel));
        }

    }
}
