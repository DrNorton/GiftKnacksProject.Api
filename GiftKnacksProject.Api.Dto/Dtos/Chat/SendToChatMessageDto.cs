using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Chat
{
    public class SendToChatMessageDto
    {
       public long To { get; set; }
       public long From { get; set; }
       public string Message { get; set; }
    }
}
