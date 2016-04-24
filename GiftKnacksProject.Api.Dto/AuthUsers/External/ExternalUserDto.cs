using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.AuthUsers.External
{
    public class ExternalUserDto
    {
        public System.Guid ExternalLoginId { get; set; }
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public ApplicationUser User { get; set; }
    }
}
