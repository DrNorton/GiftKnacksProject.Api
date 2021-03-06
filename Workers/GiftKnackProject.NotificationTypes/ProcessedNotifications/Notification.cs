﻿using System;
using Newtonsoft.Json;

namespace GiftKnackProject.NotificationTypes.ProcessedNotifications
{
    public class Notification
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Action { get; set; }

        public long TargetUserId
        {
            get { return Info.OwnerId; }
        }

        [JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        public BaseNotificationInfo Info { get; set; }
    }
}
