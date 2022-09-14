using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Model
{
    public class BaseResponseStatus
    {
        public object ResponseData1 { get; set; }
        public object ResponseData2 { get; set; }
        public string StatusCode { get; set; }
    }
}
