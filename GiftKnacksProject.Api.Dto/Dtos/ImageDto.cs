using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Dto.Dtos
{
    public class ImageDto
    {
        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
