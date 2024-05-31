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

        public ActionResult Checkout(string productIds)
        {
            var productIdList = productIds.Split(',').Select(id => int.Parse(id)).ToList();

            var selectedProducts = db.tb_Cart
                                    .Where(item => productIdList.Contains((int)item.ProductID)) // Handle nullable int
                                    .Join(db.tb_Product,
                                          cart => cart.ProductID,
                                          product => product.ProductID,
                                          (cart, product) => new Cart
                                          {
                                              ProductID = (int)cart.ProductID,
                                              Name = product.Name,
                                              Price = (int)cart.Price,
                                              Quantity = (int)cart.Quantity,
                                              Image = product.Image
                                          })
                                    .ToList();

            decimal productTotal = selectedProducts.Sum(item => item.Quantity * item.Price);
            decimal shippingCost = 30000; // Example shipping cost
            decimal discount = 0; // Example discount, update as needed
            decimal totalAmount = productTotal + shippingCost - discount;

            var checkoutViewModel = new Checkout
            {
                SelectedProducts = selectedProducts,
                ProductTotal = productTotal,
                ShippingCost = shippingCost,
                Discount = discount,
                TotalAmount = totalAmount
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Checkout model)
        {
            try
            {
                // Check if the CustomerID exists in the tb_Customer table
                int? customerIdNullable = Session["CustomerID"] as int?;

                if (!customerIdNullable.HasValue)
                {
                    return RedirectToAction("Login", "User");
                }

                int customerId = customerIdNullable.Value;

                // Group products by ShopID
                var productsGroupedByShop = model.SelectedProducts
                                                  .GroupBy(p => db.tb_Product
                                                                  .Where(prod => prod.ProductID == p.ProductID)
                                                                  .Select(prod => prod.ShopID)
                                                                  .FirstOrDefault())
                                                  .ToList();

                // Create orders for each shop
                foreach (var shopGroup in productsGroupedByShop)
                {
                    int shopId = shopGroup.Key;

                    // Create and save the order
                    tb_Order order = new tb_Order
                    {
                        OrderDate = DateTime.Now,
                        Status = "Chờ thanh toán",
                        Address = model.Address,
                        CustomerID = customerId,
                        ShopID = shopId,
                        Discount = (int?)model.Discount, // Explicit conversion from decimal to int?
                        DeliveredDate = null // Update as necessary
                    };

                    db.tb_Order.Add(order);
                    db.SaveChanges();

                    // Retrieve the saved order ID
                    int orderId = order.OrderID;

                    // Save order details
                    foreach (var product in shopGroup)
                    {
                        tb_OrderDetail orderDetail = new tb_OrderDetail
                        {
                            OrderID = orderId,
                            ProductID = product.ProductID,
                            Price = product.Price,
                            Quantity = product.Quantity,
                            Status = "" // Update as necessary
                        };

                        db.tb_OrderDetail.Add(orderDetail);
                    }

                    db.SaveChanges();
                }

                // Redirect to Orders page
                return RedirectToAction("Orders");
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log).
                Console.WriteLine(ex);
                ModelState.AddModelError("", "An error occurred while placing the order. Please try again.");
                return View("Checkout", model); // Return Checkout view with the current model if error
            }
        }


        public ActionResult Orders()
        {
            int? customerIdNullable = Session["CustomerID"] as int?;

            if (!customerIdNullable.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            int customerId = customerIdNullable.Value;

            var orders = db.tb_Order
                           .Where(o => o.CustomerID == customerId)
                           .Select(o => new OrderBuy
                           {
                               OrderID = o.OrderID,
                               OrderDate = (DateTime)o.OrderDate,
                               Status = o.Status,
                               ShopName = db.tb_Shop.Where(s => s.ShopID == o.ShopID).Select(s => s.Name).FirstOrDefault(),
                               ShopID = (int)o.ShopID,
                               OrderDetails = db.tb_OrderDetail
                                                .Where(od => od.OrderID == o.OrderID)
                                                .Select(od => new OrderDetail
                                                {
                                                    ProductID = (int)od.ProductID,
                                                    ProductName = db.tb_Product.Where(p => p.ProductID == od.ProductID).Select(p => p.Name).FirstOrDefault(),
                                                    ProductImage = db.tb_Product.Where(p => p.ProductID == od.ProductID).Select(p => p.Image).FirstOrDefault(),
                                                    Price = (decimal)od.Price,
                                                    Quantity = (int)od.Quantity,
                                                    TotalPrice = (decimal)(od.Price * od.Quantity)
                                                }).ToList()
                           }).ToList();

            return View(orders);
        }

    }
}
