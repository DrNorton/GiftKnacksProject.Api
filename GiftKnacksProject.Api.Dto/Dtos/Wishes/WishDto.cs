using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Links;
using GiftKnacksProject.Api.Dto.JsonConverters;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Dto.Dtos.Wishes
{
    public class WishDto:BaseEntityDto
    {
        public CountryDto Country { get; set; }

        public string City { get; set; }
        

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<System.DateTime> FromDate { get; set; }
         [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Benefit { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public int Emergency { get; set; }
        public StatusDto Status { get; set; }
        public CreatorDto Creator { get; set; }

        public IEnumerable<ParticipantDto> Participants { get; set; }

         
    }
}
