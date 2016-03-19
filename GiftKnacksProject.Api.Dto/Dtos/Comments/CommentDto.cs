using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnacksProject.Api.Dto.Dtos.Comments
{
    public class CommentDto
    {
        public long Id { get; set; }
        public TinyProfileDto User { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Text { get; set; }

        public List<CommentDto> ChildComments { get; set; } 
    }
}
