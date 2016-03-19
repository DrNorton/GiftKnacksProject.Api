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
    public class JoinNotificationHandler:BaseNotificationHandler<JoinQueueNotification>
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IWishRepository _wishRepository;
        private readonly IProfileRepository _profileRepository;
        public override bool IsMultipleNotification => false;

        public JoinNotificationHandler(IGiftRepository giftRepository,IWishRepository wishRepository,IProfileRepository profileRepository)
        {
            _giftRepository = giftRepository;
            _wishRepository = wishRepository;
            _profileRepository = profileRepository;
        }

        public override async Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(JoinQueueNotification messageFromQueue)
        {
            var gift = await _giftRepository.GetBasicInfo(messageFromQueue.TargetGiftId);
            var wish = await _wishRepository.GetBasicInfo(messageFromQueue.TargetWishId);
            long ownerId = 0;
            BasicWishGiftDto target = null;
            string targetType = "";
            if (messageFromQueue.CreatorId == messageFromQueue.OwnerWish)
            {
                //Инициатор владелец виша. Следовательно  уведомляем гифтера
                ownerId = gift.Owner;
                target = gift;
                targetType = "gift";
            }
            else
            {
                //Инициатор владелец гифта. Следовательно  уведомляем вишера
                ownerId = wish.Owner;
                target = wish;
                targetType = "wish";
            }
            var creator = await _profileRepository.GetTinyProfile(messageFromQueue.CreatorId);
            return new CloseJoinInfo() { OwnerId = ownerId, User = creator, Target = target,TargetType=targetType };
           
        }

        public override  Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(JoinQueueNotification messageFromQueue)
        {
            throw new NotImplementedException();
        }
    }
}
