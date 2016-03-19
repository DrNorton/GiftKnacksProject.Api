using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Controllers.Models
{
    public class RecoverPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
