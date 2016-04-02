using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Controllers.Models
{
    public class StringIdModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
