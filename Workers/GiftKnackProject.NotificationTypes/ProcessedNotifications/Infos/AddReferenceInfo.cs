using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos
{
    public class AddReferenceInfo:BaseNotificationInfo
    {
        public TinyProfileDto User { get; set; }

        public byte? Rate { get; set; }
        public long TargetUserId { get; set; }
    }
}
