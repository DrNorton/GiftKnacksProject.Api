using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Wishes
{
    public class WishCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentName { get; set; }
    }
}
