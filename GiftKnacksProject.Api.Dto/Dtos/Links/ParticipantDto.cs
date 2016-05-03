namespace GiftKnacksProject.Api.Dto.Dtos.Links
{
    public class ParticipantDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TargetLinkItem Target { get; set; }

    }

    public class TargetLinkItem
    {
        public string Type { get; set; }
        public string TargetName { get; set; }
        public long TargetId { get; set; }
    }
    
}
