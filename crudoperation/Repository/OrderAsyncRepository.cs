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
    public class OrderAsyncRepository : BaseAsyncRepository, IOrderAsyncRepository
    {
        public OrderAsyncRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public object Id { get; private set; }
        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            List<OrderModel> orderModel = null;
            using (DbConnection dbConnection=SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var order = await dbConnection.QueryAsync<OrderModel>(@"Select Id,OrderCode,OrderDate,SubTotal,TotalDiscount,GrandTotal,Remark,BillingAddress,ShippingAddress,CreatedBy,CreatedDate,IsDeleted from Ass3_TrnOrder");
                orderModel = order.ToList();
                foreach (OrderModel orderModels in orderModel)
                {
                    var orderDetails = await dbConnection.QueryAsync<OrderDetailsModel>(@"select Id,OrderId,ProductId,DiscountId,Quantity,Rate,Amount,Remark,DiscountAmount,NetAmount,CreatedBy,CreatedDate,IsDeleted from Ass3_TrnOrdeDetails 
                                                                                 where OrderId=@OrderId",
                                                                                 new { OrderId = orderModels.Id });

                    orderModels.OrderDetails = orderDetails.ToList();
                }
            }
            return orderModel;
        }
        public async Task<OrderModel> GetOrderById(int Id)
        {
            OrderModel orderModel = null;
            using (DbConnection dbConnection = SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var orders = await dbConnection.QueryAsync<OrderModel>(@"Select Id,OrderCode,OrderDate,SubTotal,TotalDiscount,GrandTotal,Remark,BillingAddress,ShippingAddress,CreatedBy,CreatedDate from Ass3_TrnOrder where Id=@Id and IsDeleted=0", new { Id });
                orderModel = orders.FirstOrDefault();
                foreach (OrderModel orderModel1 in orders)
                {
                    int orderId = orderModel.Id;
                    var listOrderofSpecificId = await dbConnection.QueryAsync<OrderDetailsModel>(@"Select id,OrderId,ProductId,DiscountId,Quantity,Rate,Amount,Remark,DiscountAmount,NetAmount,CreatedBy,CreatedDate from Ass3_TrnOrdeDetails where OrderId=@OrderId and IsDeleted=0", new { OrderId = orderModel.Id });
                    orderModel1.OrderDetails = listOrderofSpecificId.ToList();
                }
            }
            return orderModel;
        }
        public async Task<int> SaveOrder(OrderModel orderModel)
        {
            int result = 0;
            int resultResult = 0;
            using (DbConnection dbConnection = SqlWriterConnection)
            {
                await dbConnection.OpenAsync();
                var orderListByOrderCode = await dbConnection.QueryAsync(@"SELECT OrderCode from Ass3_TrnOrder WHERE OrderCode=@OrderCode", new { OrderCode = orderModel.OrderCode });
                var FirstOrderByOrderCode = orderListByOrderCode.FirstOrDefault();
                if (FirstOrderByOrderCode != null)
                {
                    return -1;
                }
                orderModel.CreatedDate = DateTime.Now;
                orderModel.ModifiedDate = DateTime.Now;

                result = await dbConnection.QuerySingleAsync<int>(@"INSERT INTO Ass3_TrnOrder(OrderCode,OrderDate,SubTotal,
                                                           TotalDiscount,GrandTotal,Remark,BillingAddress,ShippingAddress,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsDeleted) 
                                                           VALUES(@OrderCode,@OrderDate,@SubTotal,@TotalDiscount,@GrandTotal,
                                                           @Remark,@BillingAddress,@ShippingAddress,1,@CreatedDate,1,@ModifiedDate,0);
                                                           SELECT CAST(SCOPE_IDENTITY() as bigint)", orderModel);
                if (result >= 1)
                {
                    resultResult = result;
                    int OrderId = result;
                    foreach (OrderDetailsModel orderDetails in orderModel.OrderDetails)
                    {
                        orderDetails.CreatedDate = DateTime.Now;
                        orderDetails.ModifiedDate = DateTime.Now;
                        result = await dbConnection.ExecuteAsync(@"INSERT INTO Ass3_TrnOrdeDetails(OrderId,ProductId,DiscountId,Quantity,Rate,
                                                                   Amount,Remark,DiscountAmount,NetAmount,CreatedBy,CreatedDate,IsDeleted,ModifiedBy,ModifiedDate) 
                                                                   VALUES(@OrderId,@ProductId,@DiscountId,@Quantity,@Rate,@Amount,@Remark,
                                                                   @DiscountAmount,@NetAmount,1,@CreatedDate,0,@ModifiedBy,@ModifiedDate)",
                        new
                        {
                            OrderId = OrderId,
                            ProductId = orderDetails.ProductId,
                            DiscountId = orderDetails.DiscountId,
                            Quantity = orderDetails.Quantity,
                            Rate = orderDetails.Rate,
                            Amount = orderDetails.Amount,
                            Remark = orderDetails.Remark,
                            DiscountAmount = orderDetails.DiscountAmount,
                            NetAmount = orderDetails.NetAmount,
                            CreatedDate= orderDetails.CreatedDate,
                            ModifiedBy = orderDetails.ModifiedBy,
                            ModifiedDate = orderDetails.ModifiedDate

                        }); ;
                    }


                }
                return resultResult;
            }
        }
        public async Task<int> UpdateOrder(OrderModel orderModel)
        {
            int result = 0;
            int returnResult = 0;
            using (DbConnection dbConnection = SqlWriterConnection)
            {
                await dbConnection.OpenAsync();
                var tdsList = await dbConnection.QueryAsync<OrderModel>(@"SELECT Id FROM Ass3_TrnOrder WHERE Id = @Id AND IsDeleted!='True'",
                   new { Id = orderModel.Id });
                var FirstOrder = tdsList.FirstOrDefault();
                if (FirstOrder != null)
                {
                    orderModel.ModifiedDate = DateTime.Now;
                    result = await dbConnection.ExecuteAsync(@"Update Ass3_TrnOrder SET OrderCode = @OrderCode, OrderDate = @OrderDate, 
                                                            SubTotal = @SubTotal, TotalDiscount = @TotalDiscount, GrandTotal = @GrandTotal,
                                                            Remark = @Remark, BillingAddress = @BillingAddress, ShippingAddress = @ShippingAddress, ModifiedBy = '1' 
                                                            WHERE Id = @Id", orderModel);
                    if (result >= 1)
                    {
                        returnResult = result;
                        foreach (var orderDetails in orderModel.OrderDetails)
                        {
                            int Id = orderDetails.Id;
                            result = await dbConnection.ExecuteAsync(@"Update Ass3_TrnOrdeDetails SET ProductId = @ProductId, Quantity = @Quantity, Rate = @Rate,
                                                                      Amount = @Amount, DiscountId = @DiscountId, DiscountAmount = @DiscountAmount, NetAmount = @NetAmount, ModifiedBy ='1' WHERE Id = @Id", orderDetails);
                        }
                    }
                }
                return returnResult;
            }
        }
        public async Task<int> DeleteOrder(DeleteOrder deleteOrder)
        {
            int result = 0;
            int result1 = 0;
            int ReturnResult = 0;
            if (deleteOrder.Id != 0)
            {
                using (DbConnection dbConnection = SqlWriterConnection)
                {
                    await dbConnection.OpenAsync();
                    result = await dbConnection.ExecuteAsync(@"UPDATE  Ass3_TrnOrder SET IsDeleted='True'
                                                              WHERE Id=@Id", new { Id = deleteOrder.Id });
                    if (result == 1)
                    {
                        OrderDetailsModel orderDetails = new OrderDetailsModel();
                        orderDetails.OrderId = deleteOrder.Id;
                        result1 = await dbConnection.ExecuteAsync(@"UPDATE  Ass3_TrnOrdeDetails SET IsDeleted='True'
                                                              WHERE OrderId=@OrderId", new { OrderId = deleteOrder.Id });
                        ReturnResult = result;
                    }
                }

            }
            return ReturnResult;
        }
        public async Task<BaseResponse> GetAllOrderByPagination(int pageno, int pagesize)
        {
            BaseResponse baseResponse = new BaseResponse();
            Pagination pagination1 = new Pagination();
            List<OrderModel> orderList = new List<OrderModel>();
            if (pageno == 0)
            {
                pageno = 1;
            }
            if (pagesize == 0)
            {
                pagesize = 10;
            }
            using (DbConnection dbConnection = SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var sql = (@"SELECT * FROM Ass3_TrnOrder order by Id desc 
                           OFFSET(@pageno - 1) * @pagesize ROWS FETCH NEXT @pagesize ROWS ONLY; 
                           select @pageno as PageNo, count(distinct ort.Id) as TotalPages from Ass3_TrnOrder ort");
                var values = new { pageno = pageno, pagesize = pagesize };
                var result = await dbConnection.QueryMultipleAsync(sql, values);
                var dataList = await result.ReadAsync<OrderModel>();
                var pagination = await result.ReadAsync<Pagination>();

                orderList = dataList.ToList();
                pagination1 = pagination.FirstOrDefault();
                int PageCount = 0;
                int last = 0;
                last = pagination1.TotalPages % pagesize;
                PageCount = pagination1.TotalPages / pagesize;
                pagination1.TotalPages = PageCount;
                if (last > 0)
                {
                    pagination1.TotalPages = PageCount + 1;
                }
                foreach (OrderModel order2 in orderList)
                {
                    var orderDetailsList = await dbConnection.QueryAsync<OrderDetailsModel>(@"SELECT * FROM Ass3_TrnOrdeDetails
                                                                                             where OrderId=@OrderId",
                                                                                             new { OrderId = order2.Id });
                    order2.OrderDetails = orderDetailsList.ToList();
                }
                baseResponse.ResponseData1 = orderList;
                baseResponse.ResponseData2 = pagination1;
            }
            return baseResponse;
        }

        public async Task<OrderModel> GetOrderByOrderCode(int OrderCode)
        {

            OrderModel OrderModel = null;
            using (DbConnection dbConnection = SqlReaderConnection)
            {
                await dbConnection.OpenAsync();
                var OrderModels = await dbConnection.QueryAsync<OrderModel>(@"Select Id,OrderCode,OrderDate,SubTotal,TotalDiscount,GrandTotal,Remark,BillingAddress,ShippingAddress,CreatedBy,CreatedDate from Ass3_TrnOrder where OrderCode = @OrderCode", new { OrderCode = OrderCode });
                OrderModel = OrderModels.FirstOrDefault();
                foreach (var orderModel in OrderModels)
                {
                    var listOrderofSpecificOrderId = await dbConnection.QueryAsync<OrderDetailsModel>(@"Select id,OrderId,ProductId,DiscountId,Quantity,Rate,Amount,Remark,DiscountAmount,NetAmount,CreatedBy,CreatedDate from Ass3_TrnOrdeDetails where OrderId=@orderId", new { OrderId = orderModel.Id });
                    OrderModel.OrderDetails = listOrderofSpecificOrderId.ToList();
                }
            }
            return OrderModel;
        }
    }
}
