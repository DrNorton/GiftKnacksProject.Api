using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Controllers.Models
{
    public class AddCommentToWishDto
    {
        public long WishId { get; set; }
        public long? ParentCommentId { get; set; }
        public string Text { get; set; }
    }
}
