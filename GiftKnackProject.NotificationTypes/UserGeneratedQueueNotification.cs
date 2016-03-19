namespace GiftKnackProject.NotificationTypes
{
    public abstract class UserGeneratedQueueNotification:BaseQueueNotification
    {
        public long CreatorId { get; set; }
    }
}
