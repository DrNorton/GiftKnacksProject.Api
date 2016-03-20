using System.Collections.Generic;
using System.Threading.Tasks;

using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;
using Microsoft.ServiceBus.Messaging;

namespace GiftKnackNotificationAgent.Models.Handlers
{
    public class AddCommentNotificationHandler:BaseNotificationHandler<AddCommentQueueNotification>
    {
        private readonly IWishRepository _wishRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly IProfileRepository _profileRepository;


        public AddCommentNotificationHandler(IWishRepository wishRepository,IGiftRepository giftRepository,IProfileRepository profileRepository)
        {
            _wishRepository = wishRepository;
            _giftRepository = giftRepository;
            _profileRepository = profileRepository;
        }

        public override bool IsMultipleNotification => false;

        public override async Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(AddCommentQueueNotification messageFromQueue)
        {
            BasicWishGiftDto wishGiftDto = null;
            switch (messageFromQueue.TargetType)
            {
                case "wish":
                    wishGiftDto = await _wishRepository.GetBasicInfo(messageFromQueue.TargetId);
                    break;

                case "gift":
                    wishGiftDto = await _giftRepository.GetBasicInfo(messageFromQueue.TargetId);
                    break;
            }
            var user = await _profileRepository.GetTinyProfile(messageFromQueue.CreatorId);

           return new AddCommentInfo()
            {
                TargetType = messageFromQueue.TargetType,
                User = user,
                Target = wishGiftDto,
                OwnerId = wishGiftDto.Owner,
                CommentId=messageFromQueue.CommentId
            };
        }

        public override Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(AddCommentQueueNotification messageFromQueue)
        {
            throw new System.NotImplementedException();
        }
    }
}
