using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Profile;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiftKnacksProject.Api.Dto.Dtos.Chat
{
    public class DialogWithMessagesDto
    {
        public JObject Members { get; set; }

        public List<MessageFromDialog> Messages { get; set; } 
        
    }


    
   
}
