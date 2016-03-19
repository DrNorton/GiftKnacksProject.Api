using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos.Interesting
{
    public class UserNearDto:NearEntityDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
