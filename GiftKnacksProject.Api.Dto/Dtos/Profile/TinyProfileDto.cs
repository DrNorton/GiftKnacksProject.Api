using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Profile
{
    public class TinyProfileDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

         public string AvatarUrl { get; set; }

        public double AvgRate { get; set; }

        public long TotalClosed { get; set; }
    }
}
