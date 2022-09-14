using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Model
{
    public class DiscountModel : BaseModel
    {
        public int Id { get; set; }
        public string DiscountType { get; set; }
         public string Value { get; set; }
    }
}
