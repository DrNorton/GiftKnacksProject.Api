using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dto.Dtos
{
    public class PagingDto
    {
        private int _offset=0;
        private int _length=0;

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }
}
