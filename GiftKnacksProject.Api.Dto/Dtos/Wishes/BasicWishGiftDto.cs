using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Wishes
{
    public class BasicWishGiftDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        
        public long Owner { get; set; }
        public string ImageUrl { get; set; }
    }
}
