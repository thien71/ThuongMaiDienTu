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
    public class CartController : Controller
    {
        private dbThuongMaiDienTuEntities db = new dbThuongMaiDienTuEntities();

        // GET: Cart
        public ActionResult Index()
        {
            var tb_Cart = db.tb_Cart.Include(t => t.tb_Customer).Include(t => t.tb_Product);
            return View(tb_Cart.ToList());
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.tb_Customer, "CustomerID", "Username");
            ViewBag.ProductID = new SelectList(db.tb_Product, "ProductID", "Name");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartID,CustomerID,ProductID,Price,Quantity")] tb_Cart tb_Cart)
        {
            if (ModelState.IsValid)
            {
                db.tb_Cart.Add(tb_Cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.tb_Customer, "CustomerID", "Username", tb_Cart.CustomerID);
            ViewBag.ProductID = new SelectList(db.tb_Product, "ProductID", "Name", tb_Cart.ProductID);
            return View(tb_Cart);
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
