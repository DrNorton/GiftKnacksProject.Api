using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos
{
    public class TotalClosedInfo: BaseNotificationInfo
    {
        public TinyProfileDto WishOwner { get; set; }
        public BasicWishGiftDto ClosedWish { get; set; }

    }
}
