namespace GiftKnackProject.NotificationTypes.Chat
{
    public class ChatMqMessage
    {
        public long To { get; set; }
        public long From { get; set; }
        public string Message { get; set; }
    }
}
