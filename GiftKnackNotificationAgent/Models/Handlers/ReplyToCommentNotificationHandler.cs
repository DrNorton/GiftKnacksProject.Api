using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnackNotificationAgent.Models.Handlers
{
    public class ReplyToCommentNotificationHandler : BaseNotificationHandler<ReplyToCommentQueueNotification>
    {
        private readonly IWishRepository _wishRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ICommentRepository _commentRepository;

        public ReplyToCommentNotificationHandler(IWishRepository wishRepository,IGiftRepository giftRepository,IProfileRepository profileRepository,ICommentRepository commentRepository)
        {
            _wishRepository = wishRepository;
            _giftRepository = giftRepository;
            _profileRepository = profileRepository;
            _commentRepository = commentRepository;
        }

        public override bool IsMultipleNotification => false;
        public override async Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(ReplyToCommentQueueNotification messageFromQueue)
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
            var userCommentId = await _commentRepository.GetAutorUserIdByCommentId(messageFromQueue.ParentCommentId);
            var user = await _profileRepository.GetTinyProfile(messageFromQueue.CreatorId);

            return new AddCommentInfo()
            {
                TargetType = messageFromQueue.TargetType,
                User = user,
                Target = wishGiftDto,
                OwnerId = userCommentId,
                CommentId = messageFromQueue.CommentId
            };
        }

        public override Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(ReplyToCommentQueueNotification messageFromQueue)
        {
            throw new NotImplementedException();
        }
    }
}
