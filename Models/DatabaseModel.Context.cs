﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class dbThuongMaiDienTuEntities : DbContext
    {
        public dbThuongMaiDienTuEntities()
            : base("name=dbThuongMaiDienTuEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_Address> tb_Address { get; set; }
        public virtual DbSet<tb_Brand> tb_Brand { get; set; }
        public virtual DbSet<tb_Cart> tb_Cart { get; set; }
        public virtual DbSet<tb_Customer> tb_Customer { get; set; }
        public virtual DbSet<tb_ListImage> tb_ListImage { get; set; }
        public virtual DbSet<tb_Order> tb_Order { get; set; }
        public virtual DbSet<tb_OrderDetail> tb_OrderDetail { get; set; }
        public virtual DbSet<tb_Product> tb_Product { get; set; }
        public virtual DbSet<tb_ProductCategory> tb_ProductCategory { get; set; }
        public virtual DbSet<tb_ProductComment> tb_ProductComment { get; set; }
        public virtual DbSet<tb_Role> tb_Role { get; set; }
        public virtual DbSet<tb_Shop> tb_Shop { get; set; }
        public virtual DbSet<tb_Supplier> tb_Supplier { get; set; }
    
        public virtual int procAddProduct(string name, byte[] image, Nullable<int> imageID, Nullable<decimal> price, Nullable<decimal> promotionPrice, Nullable<int> quantity, string description, string detail, Nullable<int> cateID, Nullable<int> shopID, Nullable<int> brandID, Nullable<int> supplierID)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var imageParameter = image != null ?
                new ObjectParameter("Image", image) :
                new ObjectParameter("Image", typeof(byte[]));
    
            var imageIDParameter = imageID.HasValue ?
                new ObjectParameter("ImageID", imageID) :
                new ObjectParameter("ImageID", typeof(int));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var promotionPriceParameter = promotionPrice.HasValue ?
                new ObjectParameter("PromotionPrice", promotionPrice) :
                new ObjectParameter("PromotionPrice", typeof(decimal));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var detailParameter = detail != null ?
                new ObjectParameter("Detail", detail) :
                new ObjectParameter("Detail", typeof(string));
    
            var cateIDParameter = cateID.HasValue ?
                new ObjectParameter("CateID", cateID) :
                new ObjectParameter("CateID", typeof(int));
    
            var shopIDParameter = shopID.HasValue ?
                new ObjectParameter("ShopID", shopID) :
                new ObjectParameter("ShopID", typeof(int));
    
            var brandIDParameter = brandID.HasValue ?
                new ObjectParameter("BrandID", brandID) :
                new ObjectParameter("BrandID", typeof(int));
    
            var supplierIDParameter = supplierID.HasValue ?
                new ObjectParameter("SupplierID", supplierID) :
                new ObjectParameter("SupplierID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddProduct", nameParameter, imageParameter, imageIDParameter, priceParameter, promotionPriceParameter, quantityParameter, descriptionParameter, detailParameter, cateIDParameter, shopIDParameter, brandIDParameter, supplierIDParameter);
        }
    
        public virtual int procAddProductComment(string detail, Nullable<int> star, Nullable<int> productID, Nullable<int> customerID)
        {
            var detailParameter = detail != null ?
                new ObjectParameter("Detail", detail) :
                new ObjectParameter("Detail", typeof(string));
    
            var starParameter = star.HasValue ?
                new ObjectParameter("Star", star) :
                new ObjectParameter("Star", typeof(int));
    
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procAddProductComment", detailParameter, starParameter, productIDParameter, customerIDParameter);
        }
    
        public virtual ObjectResult<procGetAllProductByShop_Result> procGetAllProductByShop(Nullable<int> shopID)
        {
            var shopIDParameter = shopID.HasValue ?
                new ObjectParameter("ShopID", shopID) :
                new ObjectParameter("ShopID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<procGetAllProductByShop_Result>("procGetAllProductByShop", shopIDParameter);
        }
    
        public virtual ObjectResult<procGetProduct_Result> procGetProduct(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<procGetProduct_Result>("procGetProduct", productIDParameter);
        }
    
        public virtual ObjectResult<procGetProductComment_Result> procGetProductComment(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<procGetProductComment_Result>("procGetProductComment", productIDParameter);
        }
    
        public virtual int procRemoveProduct(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procRemoveProduct", productIDParameter);
        }
    
        public virtual int procUpdateProduct(Nullable<int> productID, string name, byte[] image, Nullable<decimal> price, Nullable<decimal> promotionPrice, Nullable<int> quantity, string description, string detail, Nullable<int> cateID, Nullable<int> shopID, Nullable<int> brandID, Nullable<int> supplierID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var imageParameter = image != null ?
                new ObjectParameter("Image", image) :
                new ObjectParameter("Image", typeof(byte[]));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var promotionPriceParameter = promotionPrice.HasValue ?
                new ObjectParameter("PromotionPrice", promotionPrice) :
                new ObjectParameter("PromotionPrice", typeof(decimal));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var detailParameter = detail != null ?
                new ObjectParameter("Detail", detail) :
                new ObjectParameter("Detail", typeof(string));
    
            var cateIDParameter = cateID.HasValue ?
                new ObjectParameter("CateID", cateID) :
                new ObjectParameter("CateID", typeof(int));
    
            var shopIDParameter = shopID.HasValue ?
                new ObjectParameter("ShopID", shopID) :
                new ObjectParameter("ShopID", typeof(int));
    
            var brandIDParameter = brandID.HasValue ?
                new ObjectParameter("BrandID", brandID) :
                new ObjectParameter("BrandID", typeof(int));
    
            var supplierIDParameter = supplierID.HasValue ?
                new ObjectParameter("SupplierID", supplierID) :
                new ObjectParameter("SupplierID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("procUpdateProduct", productIDParameter, nameParameter, imageParameter, priceParameter, promotionPriceParameter, quantityParameter, descriptionParameter, detailParameter, cateIDParameter, shopIDParameter, brandIDParameter, supplierIDParameter);
        }
    }
}
