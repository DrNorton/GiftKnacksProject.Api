using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnackProject.NotificationTypes
{
    public class AddReferenceQueueNotification:UserGeneratedQueueNotification
    {
        public override string Type { get { return "addreference"; } }
        public long RefefenceId { get; set; }
       
    }
}
