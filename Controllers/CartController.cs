using System;
using System.Collections.Generic;
using System.Linq;
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
            int? customerIdNullable = Session["CustomerID"] as int?;

            if (!customerIdNullable.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            int customerId = customerIdNullable.Value;

            var cartItems = db.tb_Cart
                              .Where(c => c.CustomerID == customerId)
                              .Join(db.tb_Product, // Thực hiện join với tb_Product để lấy tên sản phẩm
                                    c => c.ProductID,
                                    p => p.ProductID,
                                    (c, p) => new Cart // Sử dụng lớp Cart để ánh xạ kết quả
                                    {
                                        ProductID = c.ProductID ?? 0,
                                        Name = p.Name,
                                        Image = c.Image,
                                        Quantity = c.Quantity.HasValue ? c.Quantity.Value : 0, // Xử lý giá trị null
                                        Price = c.Price.HasValue ? c.Price.Value : 0 // Xử lý giá trị null
                                    }).ToList();

            ViewBag.TotalQuantity = cartItems.Sum(c => c.Quantity);
            ViewBag.TotalPrice = cartItems.Sum(c => c.Quantity * c.Price);
            ViewBag.Cart = cartItems;

            return View(cartItems);
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            int? customerIdNullable = Session["CustomerID"] as int?;
            if (customerIdNullable.HasValue)
            {
                int customerId = customerIdNullable.Value;
                var cartItem = db.tb_Cart.SingleOrDefault(c => c.CustomerID == customerId && c.ProductID == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    db.SaveChanges();
                }
            }
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            int? customerIdNullable = Session["CustomerID"] as int?;
            if (customerIdNullable.HasValue)
            {
                int customerId = customerIdNullable.Value;
                var cartItem = db.tb_Cart.SingleOrDefault(c => c.CustomerID == customerId && c.ProductID == productId);
                if (cartItem == null)
                {
                    // Lấy giá sản phẩm từ tb_Product
                    var product = db.tb_Product.SingleOrDefault(p => p.ProductID == productId);
                    if (product != null)
                    {
                        // Thêm sản phẩm mới vào giỏ hàng
                        cartItem = new tb_Cart
                        {
                            CustomerID = customerId,
                            ProductID = productId,
                            Image = product.Image,
                            Quantity = quantity,
                            Price = (int?)product.Price // Ép kiểu từ decimal? sang int?
                        };
                        db.tb_Cart.Add(cartItem);
                    }
                }
                else
                {
                    // Cập nhật số lượng sản phẩm hiện có
                    cartItem.Quantity += quantity;
                }
                db.SaveChanges();
            }

            // Chuyển hướng đến trang giỏ hàng
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public ActionResult Remove(int productId)
        {
            int? customerIdNullable = Session["CustomerID"] as int?;
            if (customerIdNullable.HasValue)
            {
                int customerId = customerIdNullable.Value;
                var cartItem = db.tb_Cart.SingleOrDefault(c => c.CustomerID == customerId && c.ProductID == productId);
                if (cartItem != null)
                {
                    db.tb_Cart.Remove(cartItem);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Clear()
        {
            int? customerIdNullable = Session["CustomerID"] as int?;
            if (customerIdNullable.HasValue)
            {
                int customerId = customerIdNullable.Value;
                var cartItems = db.tb_Cart.Where(c => c.CustomerID == customerId).ToList();
                db.tb_Cart.RemoveRange(cartItems);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult BuyNow(int productId, int quantity)
        {
            int? customerIdNullable = Session["CustomerID"] as int?;
            if (customerIdNullable.HasValue)
            {
                int customerId = customerIdNullable.Value;
                var cartItem = db.tb_Cart.SingleOrDefault(c => c.CustomerID == customerId && c.ProductID == productId);
                if (cartItem == null)
                {
                    // Lấy giá sản phẩm từ tb_Product
                    var product = db.tb_Product.SingleOrDefault(p => p.ProductID == productId);
                    if (product != null)
                    {
                        // Thêm sản phẩm mới vào giỏ hàng
                        cartItem = new tb_Cart
                        {
                            CustomerID = customerId,
                            ProductID = productId,
                            Quantity = quantity,
                            Price = (int?)product.Price // Ép kiểu từ decimal? sang int?
                        };
                        db.tb_Cart.Add(cartItem);
                    }
                }
                else
                {
                    // Cập nhật số lượng sản phẩm hiện có
                    cartItem.Quantity += quantity;
                }
                db.SaveChanges();
            }

            // Chuyển hướng đến trang thanh toán
            return RedirectToAction("Checkout", "Cart");
        }
    }
}
