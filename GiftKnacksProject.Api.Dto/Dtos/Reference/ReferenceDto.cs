using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnacksProject.Api.Dto.Dtos.Reference
{
    public class ReferenceDto
    {
        public long? OwnerId { get; set; }
        public byte? Rate { get; set; }
        public string ReferenceText { get; set; }
        public TinyProfileDto Replyer { get; set; }

        public DateTime? CreatedTime { get; set; }


    }
}
