using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Comments
{
    public class GetCommentDto:PagingDto
    {
        public long CommentId { get; set; }
        public long Id { get; set; }
    }
}
