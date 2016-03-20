using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Links;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnackNotificationAgent.Models.Handlers
{
    public class CloseItemNotificationHandler : BaseNotificationHandler<CloseItemQueueNotification>
    {
        private readonly IWishRepository _wishRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly IProfileRepository _profileRepository;

        public CloseItemNotificationHandler(IWishRepository wishRepository,IGiftRepository giftRepository,IProfileRepository profileRepository)
        {
            _wishRepository = wishRepository;
            _giftRepository = giftRepository;
            _profileRepository = profileRepository;
        }

        public override bool IsMultipleNotification => true;
        public override Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(CloseItemQueueNotification messageFromQueue)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(CloseItemQueueNotification messageFromQueue)
        {
            var list = new List<CloseJoinInfo>();
            List<ParticipantDto> participants=null;
            BasicWishGiftDto wishOrGiftDto = null;
            if (messageFromQueue.TargetType == "wish")
            {
                participants = await _wishRepository.GetAllParticipants(messageFromQueue.ClosedItemId);
                wishOrGiftDto = await _wishRepository.GetBasicInfo(messageFromQueue.ClosedItemId);
            }



            if (messageFromQueue.TargetType == "gift")
            {
                participants = await _giftRepository.GetAllParticipants(messageFromQueue.ClosedItemId);
                wishOrGiftDto = await _giftRepository.GetBasicInfo(messageFromQueue.ClosedItemId);
            }

            var ownerWishOrGift = await _profileRepository.GetTinyProfile(wishOrGiftDto.Owner);
            foreach (var participant in participants)
            {
                list.Add(new CloseJoinInfo() {OwnerId = participant.Id,Target = wishOrGiftDto,User = ownerWishOrGift,TargetType = messageFromQueue.TargetType});
            }

            return list;
        }
    }
}
