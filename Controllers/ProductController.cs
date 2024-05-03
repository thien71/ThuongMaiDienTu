using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

            return View();
        }


        // GET: Product/Details/5
        public ActionResult Details(int? id)
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
