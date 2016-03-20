namespace GiftKnackProject.NotificationTypes
{
    public  class AddCommentQueueNotification: UserGeneratedQueueNotification
    {
        public override string Type
        {
            get
            {
                return "addcomment";
            }
        }
        public long TargetId { get; set; }
        public  string TargetType { get; set; }
        public long CommentId { get; set; }
    }

}
