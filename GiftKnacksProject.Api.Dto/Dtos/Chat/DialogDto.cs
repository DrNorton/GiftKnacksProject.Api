using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnacksProject.Api.Dto.Dtos.Chat
{
    public class DialogDto
    {
        public TinyProfileDto Sender { get; set; }
        public TinyProfileDto Recipient { get; set; }
        public string LastMessage { get; set; }
        public DateTime Time { get; set; }
        public int NewCount { get; set; }
        public bool IsRead { get; set; }
    }
}
