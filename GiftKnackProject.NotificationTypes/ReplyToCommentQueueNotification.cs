using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnackProject.NotificationTypes
{
    public class ReplyToCommentQueueNotification:UserGeneratedQueueNotification
    {
        public override string Type
        {
            get
            {
                return "commentreply";
            }
        }
        public long TargetId { get; set; }
        public string TargetType { get; set; }
        public long CommentId { get; set; }
        public long ParentCommentId { get; set; }
    }
}
