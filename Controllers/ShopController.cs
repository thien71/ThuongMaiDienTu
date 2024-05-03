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
    public class ShopController : Controller
    {
        private dbThuongMaiDienTuEntities db = new dbThuongMaiDienTuEntities();

        // GET: Shop
        public ActionResult Index()
        {
            var tb_Shop = db.tb_Shop.Include(t => t.tb_Customer);
            return View(tb_Shop.ToList());
        }

        // GET: Shop/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.tb_Customer, "CustomerID", "Username");
            return View();
        }

        // POST: Shop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShopID,Name,Description,Detail,OwnerID")] tb_Shop tb_Shop)
        {
            if (ModelState.IsValid)
            {
                db.tb_Shop.Add(tb_Shop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.tb_Customer, "CustomerID", "Username", tb_Shop.OwnerID);
            return View(tb_Shop);
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
