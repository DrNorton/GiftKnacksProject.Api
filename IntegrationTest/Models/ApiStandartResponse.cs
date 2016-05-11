using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.Models
{
    public class ApiStandartResponse<T>
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public T Result { get; set; }
    }
}
