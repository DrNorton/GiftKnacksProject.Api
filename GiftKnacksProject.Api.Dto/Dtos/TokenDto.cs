﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Dto.Dtos
{
    public class TokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public double ExpiredIn { get; set; }
        [JsonProperty("isFilled")]
        public bool IsFilled { get; set; }
        [JsonProperty("userId")]
        public long UserId { get; set; }
        [JsonProperty("isSocial")]
        public bool IsSocial { get; set; }
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
