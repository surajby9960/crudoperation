using crudoperation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Repository.Interface
{
  public  interface IProductAsyncRepository
    {
        Task<List<ProductModel>> GetAllProduct();
    }
}
