using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Gifts
{
    public class FilterDto:PagingDto
    {
        public CountryDto Country { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Keyword { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int StatusCode { get; set; }
        public long? UserId { get; set; }
    }
}
