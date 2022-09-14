using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Model
{
    public class OrderModel : BaseModel
    {
        public int Id { get; set; }

        public int OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remark { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderDetailsModel> OrderDetails { get; set; }
    }
    public class OrderDetailsModel : BaseModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int DiscountId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
    public class DeleteOrder
    {
        public int Id { get; set; }
    }
}
