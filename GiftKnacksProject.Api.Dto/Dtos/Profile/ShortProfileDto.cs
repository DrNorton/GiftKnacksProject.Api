using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.JsonConverters;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Dto.Dtos.Profile
{
    public class ShortProfileDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CountryDto Country { get; set; }
        public string City { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<System.DateTime> Birthday { get; set; }

        public ContactDto FavoriteContact { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Age { get; set; }


        [JsonProperty("UploadAvatar")]
        public ImageDto Image { get; set; }

        public string Gender { get; set; }

        public double AvgRate { get; set; }

        public long TotalClosed { get; set; }

        public bool IsOnline { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<System.DateTime> LastLoginTime { get; set; }

    }
}
