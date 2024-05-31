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
            var customerIdNullable = Session["CustomerID"] as int?;
            if (!customerIdNullable.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            int customerId = customerIdNullable.Value;
            var shop = db.tb_Shop.SingleOrDefault(s => s.OwnerID == customerId);

            if (shop == null)
            {
                return RedirectToAction("Signup");
            }

            // Tính toán số lượng đơn chờ lấy hàng và sản phẩm hết hàng
            var ordersWaitingForPickup = db.tb_Order
                                           .Where(o => o.ShopID == shop.ShopID && o.Status == "Chờ thanh toán")
                                           .Count();

            var outOfStockProducts = db.tb_Product
                                       .Where(p => p.ShopID == shop.ShopID && p.Quantity == 0)
                                       .Count();

            if (ordersWaitingForPickup == null)
            {
                ordersWaitingForPickup = 0;
            }

            if (outOfStockProducts == null)
            {
                outOfStockProducts = 0;
            }

            ViewBag.UserName = shop.Name;
            ViewBag.UserAvatar = shop.Avatar;
            ViewBag.OrdersWaitingForPickup = ordersWaitingForPickup;
            ViewBag.OutOfStockProducts = outOfStockProducts;

            return View();
        }


        [HttpGet]
        public ActionResult Signup()
        {
            var customerIdNullable = Session["CustomerID"] as int?;
            if (!customerIdNullable.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            int customerId = customerIdNullable.Value;
            var customer = db.tb_Customer.SingleOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.UserName = customer.Username;
            ViewBag.UserAvatar = customer.Avatar;
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string name, string email, string tel)
        {
            var customerIdNullable = Session["CustomerID"] as int?;
            if (!customerIdNullable.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            int customerId = customerIdNullable.Value;
            var customer = db.tb_Customer.SingleOrDefault(c => c.CustomerID == customerId);
            if (customer == null)
            {
                return RedirectToAction("Login", "User");
            }

            var shop = new tb_Shop
            {
                OwnerID = customerId,
                Name = name,
                Email = email,
                Phone = tel,
                CreatedDate = DateTime.Now,
                Avatar = "avatar-defauft.png"
            };

            db.tb_Shop.Add(shop);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Trong phương thức Detail của Controller
        public ActionResult Detail(int id, int? cateId)
        {
            // Load thông tin cửa hàng
            var shop = db.tb_Shop.FirstOrDefault(s => s.ShopID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            // Lấy tất cả danh mục của cửa hàng
            var allCategories = db.tb_ProductCategory
                                  .Where(c => db.tb_Product.Any(p => p.ShopID == id && p.CateID == c.CateID))
                                  .ToList();

            // Lấy tất cả sản phẩm của cửa hàng hoặc lọc theo danh mục nếu cateId được cung cấp
            var products = db.tb_Product.Where(p => p.ShopID == id && (!cateId.HasValue || p.CateID == cateId)).ToList();
            if (!products.Any())
            {
                return HttpNotFound("Không có sản phẩm nào trong cửa hàng này.");
            }

            ViewBag.Shop = shop;
            ViewBag.Products = products;
            ViewBag.Categories = allCategories; // Gán danh sách tất cả danh mục vào ViewBag.Categories
            ViewBag.JoinDate = shop.CreatedDate;
            ViewBag.PhoneNumber = shop.Phone;
            ViewBag.CateId = cateId; // Gán cateId để sử dụng trong view

            return View();
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
