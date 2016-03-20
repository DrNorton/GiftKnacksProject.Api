using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;

namespace GiftKnackNotificationAgent.Models.Handlers
{
    public class TotalClosedNotificationHandler:BaseNotificationHandler<TotalClosedQueueNotification>
    {
        private readonly IWishRepository _wishRepository;
        private readonly IProfileRepository _profileRepository;
        public override bool IsMultipleNotification => false;

        public TotalClosedNotificationHandler(IWishRepository wishRepository,IProfileRepository profileRepository)
        {
            _wishRepository = wishRepository;
            _profileRepository = profileRepository;
        }

        public async override Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(TotalClosedQueueNotification messageFromQueue)
        {
            var wishOwner = await _profileRepository.GetTinyProfile(messageFromQueue.CreatorId); //человек закрывающий wish
            var userWhoDoWish = messageFromQueue.CloserId;
            var wish = await _wishRepository.GetBasicInfo(messageFromQueue.ClosedWishId);
            return new TotalClosedInfo() { OwnerId = userWhoDoWish, WishOwner = wishOwner, ClosedWish = wish};
        }

        public override Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(TotalClosedQueueNotification messageFromQueue)
        {
            throw new NotImplementedException();
        }
    }
}
