using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnackProject.NotificationTypes
{
    public class CloseItemQueueNotification:UserGeneratedQueueNotification
    {
        public override string Type { get { return "closejoineditem"; } }
        public string TargetType { get; set; }
        public long ClosedItemId { get; set; }
       
    }
}
