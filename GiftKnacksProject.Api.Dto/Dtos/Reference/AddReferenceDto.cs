using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Reference
{
    public class AddReferenceDto
    {
        public long OwnerId { get; set; }
        public string ReferenceText { get; set; }
        public byte Rate { get; set; }
    }
}
