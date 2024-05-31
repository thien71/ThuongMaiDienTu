using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThuongMaiDienTu.Models
{
    public class OrderBuy
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ShopName { get; set; }
        public int ShopID { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}