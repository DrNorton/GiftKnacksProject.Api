using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos
{
    public class CloseJoinInfo:BaseNotificationInfo
    {
        public TinyProfileDto User { get; set; }
        public BasicWishGiftDto Target { get; set; }
        public string TargetType { get; set; }
    }
}
