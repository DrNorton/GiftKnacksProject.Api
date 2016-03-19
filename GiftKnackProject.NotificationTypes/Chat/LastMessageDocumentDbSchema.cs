using System;
using Newtonsoft.Json;

namespace GiftKnackProject.NotificationTypes.Chat
{
    public class LastMessageDocumentDbSchema
    {
        [JsonProperty("id")]
        public string Id
        {
            get
            {
                if (Recepient > Sender)
                {
                    return String.Format("{0}:{1}", Recepient, Sender);
                }
                else
                {
                    return String.Format("{0}:{1}", Sender, Recepient);
                }
            }
        }
        public long Recepient { get; set; }
        public long Sender { get; set; }
        public string LastMessage { get; set; }
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }
    }
}
