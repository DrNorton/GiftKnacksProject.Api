using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnackProject.NotificationTypes
{
    public class JoinQueueNotification:UserGeneratedQueueNotification
    {
        public override string Type
        {
            get { return "join"; } 
        }

        public long TargetWishId { get; set; }
        public long TargetGiftId { get; set; }
        public long OwnerWish { get; set; }
    }
}
