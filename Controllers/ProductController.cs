using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class ProductController : Controller
    {
        private dbThuongMaiDienTuEntities db = new dbThuongMaiDienTuEntities();

        // GET: Product
        public ActionResult Index()
        {
            ViewBag.Products = db.tb_Product.Include(t => t.tb_Brand).Include(t => t.tb_ProductCategory).Include(t => t.tb_Shop).Include(t => t.tb_Supplier).ToList();
            ViewBag.Categories = db.tb_ProductCategory.ToList();
            ViewBag.Brands = db.tb_Brand.ToList();

            return View();
        }

        public ActionResult Details(int id)
        {
            using (var db = new dbThuongMaiDienTuEntities())
            {
                // Load thông tin sản phẩm, cửa hàng và danh sách hình ảnh của sản phẩm
                var product = db.tb_Product.Include(p => p.tb_ListImage).FirstOrDefault(p => p.ProductID == id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                // Load thông tin cửa hàng của sản phẩm
                var shop = db.tb_Shop.FirstOrDefault(s => s.ShopID == product.ShopID);
                if (shop == null)
                {
                    return HttpNotFound();
                }

                // Đếm số lượng sản phẩm có ShopID tương ứng
                var productCount = db.tb_Product.Count(p => p.ShopID == id);

                // Load danh sách bình luận của sản phẩm
                var comments = db.tb_ProductComment.Where(c => c.ProductID == id).ToList();

                // Truyền thông tin sản phẩm, cửa hàng và danh sách bình luận vào view
                ViewBag.Product = product;
                ViewBag.Shop = shop;
                ViewBag.Comments = comments;
                ViewBag.ProductCount = productCount;
                //ViewBag.JoinDate = shop.CreateDate; // Giả sử có trường JoinDate trong bảng tb_Shop
                ViewBag.PhoneNumber = shop.Phone; // Giả sử có trường PhoneNumber trong bảng tb_Shop

                return View();
            }
        }



        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name");
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name");
            ViewBag.ShopID = new SelectList(db.tb_Shop, "ShopID", "Name");
            ViewBag.SupplierID = new SelectList(db.tb_Supplier, "SupplierID", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Image,Price,PromotionPrice,Quantity,Description,Detail,CateID,ShopID,BrandID,SupplierID,CreatedDate")] tb_Product tb_Product)
        {
            if (ModelState.IsValid)
            {
                db.tb_Product.Add(tb_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name", tb_Product.BrandID);
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name", tb_Product.CateID);
            ViewBag.ShopID = new SelectList(db.tb_Shop, "ShopID", "Name", tb_Product.ShopID);
            ViewBag.SupplierID = new SelectList(db.tb_Supplier, "SupplierID", "Name", tb_Product.SupplierID);
            return View(tb_Product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name", tb_Product.BrandID);
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name", tb_Product.CateID);
            ViewBag.ShopID = new SelectList(db.tb_Shop, "ShopID", "Name", tb_Product.ShopID);
            ViewBag.SupplierID = new SelectList(db.tb_Supplier, "SupplierID", "Name", tb_Product.SupplierID);
            return View(tb_Product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Image,Price,PromotionPrice,Quantity,Description,Detail,CateID,ShopID,BrandID,SupplierID,CreatedDate")] tb_Product tb_Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name", tb_Product.BrandID);
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name", tb_Product.CateID);
            ViewBag.ShopID = new SelectList(db.tb_Shop, "ShopID", "Name", tb_Product.ShopID);
            ViewBag.SupplierID = new SelectList(db.tb_Supplier, "SupplierID", "Name", tb_Product.SupplierID);
            return View(tb_Product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Product tb_Product = db.tb_Product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            return View(tb_Product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Product tb_Product = db.tb_Product.Find(id);
            db.tb_Product.Remove(tb_Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
