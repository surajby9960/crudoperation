using crudoperation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Repository.Interface
{
   public interface IDiscountAysncRepository
    {
        Task<List<DiscountModel>> GetAllDiscount();
    }
}
