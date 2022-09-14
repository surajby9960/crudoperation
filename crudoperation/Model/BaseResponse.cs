using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Model
{
    public class BaseResponse
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ResponseData { get; set; }
        public object ResponseData1 { get; set; }
        public object ResponseData2 { get; set; }
    }
}
