using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThuongMaiDienTu.Models
{
    public class Order
    {
        public int CustomerID { get; set; }
        public int ShopID { get; set; }
        public string Address { get; set; }
        public decimal Discount { get; set; }
        public List<Cart> SelectedProducts { get; set; }
    }
}