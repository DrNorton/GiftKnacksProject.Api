using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Providers
{
    public class VkGetUserResult
    {
        [JsonProperty("response")]
        public List<UserInfo> Response { get; set; } 
    }

    public class UserInfo
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}