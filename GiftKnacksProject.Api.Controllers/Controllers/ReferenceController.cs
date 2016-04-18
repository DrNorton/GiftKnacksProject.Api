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
using GiftKnacksProject.Api.Dto.Dtos.Reference;
using GiftKnacksProject.Api.EfDao;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/reference")]

    public class ReferenceController:CustomApiController
    {
        private readonly IReferenceRepository _referenceRepository;
        private readonly INotificationService _notificationService;

        public ReferenceController(IReferenceRepository referenceRepository,INotificationService notificationService)
        {
            _referenceRepository = referenceRepository;
            _notificationService = notificationService;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("add")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Add(AddReferenceDto referenceDto)
        {
            var replyerId = long.Parse(User.Identity.GetUserId());
            var referenceId=await
                _referenceRepository.AddOrUpdateReference(referenceDto.OwnerId, replyerId, referenceDto.Rate,
                    referenceDto.ReferenceText);
            await _notificationService.SentNotificationToQueue(new AddReferenceQueueNotification()
            {
                CreatorId = replyerId,
                RefefenceId = referenceId
              
            });
            
            return SuccessApiResult(null);
        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("getall")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetAll(IdModel model)
        {
            long userId =0;
            if (model.Id == null)
            {
                userId = long.Parse(User.Identity.GetUserId());
            }
            else
            {
                userId = (long) model.Id;
            }
            
            var references=await _referenceRepository.GetByOwnerId(userId);
            return SuccessApiResult(references);
        }

    
    }
}
