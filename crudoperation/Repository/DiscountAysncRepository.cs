using crudoperation.Model;
using crudoperation.Repository.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Repository
{
    public class DiscountAysncRepository : BaseAsyncRepository, IDiscountAysncRepository
    {
        public DiscountAysncRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<DiscountModel>> GetAllDiscount()
        {
            List<DiscountModel> discountModel = null;
            using (DbConnection dbConnection = SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var discountModels = await dbConnection.QueryAsync<DiscountModel>(@" Select Id,DiscountType,Value,CreatedBy,CreatedDate,IsDeleted,ModifiedBy,ModifiedDate from Ass3_Discount");
                discountModel = discountModels.ToList();
            }
            return discountModel;
        }
    }
}
