using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Dto.Dtos.Wishes
{
    public class ImagedWishDto:WishDto
    {
        public ImageDto Image { get; set; }
    }
}
