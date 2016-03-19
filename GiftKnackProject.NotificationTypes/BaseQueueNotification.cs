using System;
using Newtonsoft.Json;

namespace GiftKnackProject.NotificationTypes
{
    public abstract class BaseQueueNotification
    {
        private DateTime _notificationTime;

        public DateTime NotificationTime
        {
            get { return _notificationTime; }
          
        }

        public abstract string Type { get; }
        //Кому адресовано сообщение

        public BaseQueueNotification()
        {
            _notificationTime=DateTime.Now;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

      
    }
}
