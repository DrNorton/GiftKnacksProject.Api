using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnackProject.NotificationTypes
{
    public class TotalClosedQueueNotification:UserGeneratedQueueNotification
    {
        public override string Type
        {
            get { return "totalclosechange"; } 
        }

        public long CloserId { get; set; }
        public long ClosedWishId { get; set; }
    }
}
