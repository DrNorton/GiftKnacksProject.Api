using System.Collections.Generic;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnacksProject.Api.Dto.Dtos.Interesting
{
    public class InterestingNearDto
    {
        public List<NearEntityDto> Wishes { get; set; }
        public List<NearEntityDto> Gifts { get; set; }
        public List<NearEntityDto> Users { get; set; }

        public InterestingNearDto()
        {
            Wishes=new List<NearEntityDto>();
            Gifts=new List<NearEntityDto>();
            Users=new List<NearEntityDto>();
        }
    }
}
