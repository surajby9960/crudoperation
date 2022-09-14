using crudoperation.Model;
using crudoperation.Repository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace crudoperation.Repository
{
    public class ProductAsyncRepository : BaseAsyncRepository, IProductAsyncRepository
    {
        public ProductAsyncRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public object Id { get; private set; }
        public async Task<List<ProductModel>> GetAllProduct()
        {
            List<ProductModel> productModel = null;
            using (DbConnection dbConnection = SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var productModels = await dbConnection.QueryAsync<ProductModel>(@"Select Id,ProductName,Rate,CreatedBy,CreatedDate,IsDeleted,ModifiedBy,ModifiedDate from Ass3_Product");
                productModel = productModels.ToList();
            }
            return productModel;
        }

    }
}
