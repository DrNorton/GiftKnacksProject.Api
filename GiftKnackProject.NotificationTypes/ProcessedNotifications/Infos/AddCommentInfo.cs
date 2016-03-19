using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos
{
    public class AddCommentInfo:BaseNotificationInfo
    {
        public string TargetType { get; set; }
        public TinyProfileDto User { get; set; }
        public BasicWishGiftDto Target { get; set; }
        public long CommentId { get; set; }
    }
}
