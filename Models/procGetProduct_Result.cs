//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThuongMaiDienTu.Models
{
    using System;
    
    public partial class procGetProduct_Result
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> QuantitySold { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public Nullable<int> CateID { get; set; }
        public int ShopID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
