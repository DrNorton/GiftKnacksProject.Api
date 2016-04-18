using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Helpers
{
    public class ExceptionWithCode:Exception
    {
        public int ErrorCode { get; set; }


        public ExceptionWithCode(int errorCode,string message)
            :base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
