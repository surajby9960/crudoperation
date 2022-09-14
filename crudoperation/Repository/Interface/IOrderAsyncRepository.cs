using crudoperation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Repository.Interface
{
    public interface IOrderAsyncRepository
    {
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderById(int Id);
        Task<int> SaveOrder(OrderModel orderModel);

        Task<int> UpdateOrder(OrderModel orderModel);
        Task<int> DeleteOrder(DeleteOrder deleteOrder);
        Task<BaseResponse> GetAllOrderByPagination(int pageno, int pagesize);
        Task<OrderModel> GetOrderByOrderCode(int OrderCode);
    }
}
