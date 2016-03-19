using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GiftKnackProject.NotificationTypes.Chat
{
    public class MessageDbSchema
    {
        public string DialogId
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
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
