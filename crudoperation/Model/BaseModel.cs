using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Model
{
    public class BaseModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IsSelected { get; set; }
        public string IsDeleted { get; set; }
    }
}
