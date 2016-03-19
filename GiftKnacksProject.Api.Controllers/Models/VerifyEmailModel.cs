using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Controllers.Models
{
    public class VerifyEmailModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
