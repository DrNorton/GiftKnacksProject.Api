using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnacksProject.Api.Dto.Dtos.Chat
{
    public class DialogsResultDto
    {
        public TinyProfileDto Owner { get; set; }
        public List<DialogItemInListDto> Dialogs { get; set; } 
    }
    public class DialogItemInListDto
    {
        public TinyProfileDto Opponent { get; set; }
  
        public LastMessage LastMessage { get; set; }
        public int NewCount { get; set; }
        public bool IsRead { get; set; }
    }




    public class LastMessage
    {
        public long UserId { get; set; }
        public string Text { get; set; }

        public DateTime Time { get; set; }
    }
}
