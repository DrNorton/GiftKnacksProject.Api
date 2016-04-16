using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos
{
    public class NewMessageInfo:BaseNotificationInfo
    {
        public TinyProfileDto From { get; set; }
        public string Message { get; set; }
    }
}
