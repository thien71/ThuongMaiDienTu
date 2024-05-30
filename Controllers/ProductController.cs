using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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

        public ActionResult Details(int id, int page = 1, int pageSize = 5)
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

                var shopID = product.ShopID;
                // Đếm số lượng sản phẩm có ShopID tương ứng
                var productCount = db.tb_Product.Count(p => p.ShopID == shopID);

                // Load danh sách bình luận của sản phẩm
                var commentsQuery = db.tb_ProductComment
                            .Where(c => c.ProductID == id)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(c => new
                            {
                                c.Detail,
                                c.Star,
                                c.CreatedDate,
                                CustomerName = c.tb_Customer.Name,
                                CustomerAvatar = c.tb_Customer.Avatar
                            });

                
                // Tính toán phân trang
                var totalComments = commentsQuery.Count();
                var comments = commentsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var expandoComments = comments.Select(c =>
                {
                    dynamic expando = new ExpandoObject();
                    expando.Detail = c.Detail;
                    expando.Star = c.Star;
                    expando.CreatedDate = c.CreatedDate;
                    expando.CustomerName = c.CustomerName;
                    expando.CustomerAvatar = c.CustomerAvatar;
                    return expando;
                }).ToList();

                // Load danh sách sản phẩm liên quan
                var relatedProducts = db.tb_Product
                                        .Where(p => p.CateID == product.CateID && p.ProductID != id)
                                        .ToList();

                // Xử lý mô tả và chi tiết sản phẩm
                ////var descriptionSegments = product.Description?.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new List<string>();
                var detailSegments = product.Detail?.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new List<string>();


                // Truyền thông tin sản phẩm, cửa hàng và danh sách bình luận vào view
                ViewBag.Product = product;
                ViewBag.Shop = shop;
                ViewBag.Comments = expandoComments;
                ViewBag.ProductCount = productCount;
                ViewBag.JoinDate = shop.CreatedDate; // Giả sử có trường JoinDate trong bảng tb_Shop
                ViewBag.PhoneNumber = shop.Phone;

                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalComments = totalComments;

                ViewBag.RelatedProducts = relatedProducts;
                ViewBag.DetailSegments = detailSegments;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Manage()
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

            var shop = db.tb_Shop.FirstOrDefault(s => s.OwnerID == customerId);
            if (shop == null)
            {
                return HttpNotFound();
            }

            var model = new ProductManage
            {
                Brands = db.tb_Brand.Select(b => new SelectListItem
                {
                    Value = b.BrandID.ToString(),
                    Text = b.Name
                }).ToList(),
                Categories = db.tb_ProductCategory.Select(c => new SelectListItem
                {
                    Value = c.CateID.ToString(),
                    Text = c.Name
                }).ToList(),
                Products = db.tb_Product.Where(p => p.ShopID == shop.ShopID).ToList()
            };

            ViewBag.ShopAvatar = shop.Avatar;
            ViewBag.ShopName = shop.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult Manage(ProductManage model)
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

            var shop = db.tb_Shop.FirstOrDefault(s => s.OwnerID == customerId);
            if (shop == null)
            {
                return HttpNotFound();
            }

            var products = db.tb_Product.Where(p => p.ShopID == shop.ShopID).AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchName))
            {
                products = products.Where(p => p.Name.Contains(model.SearchName));
            }

            if (model.SelectedCategory.HasValue)
            {
                products = products.Where(p => p.CateID == model.SelectedCategory.Value);
            }

            if (model.SelectedBrand.HasValue)
            {
                products = products.Where(p => p.BrandID == model.SelectedBrand.Value);
            }

            model.Brands = db.tb_Brand.Select(b => new SelectListItem
            {
                Value = b.BrandID.ToString(),
                Text = b.Name
            }).ToList();

            model.Categories = db.tb_ProductCategory.Select(c => new SelectListItem
            {
                Value = c.CateID.ToString(),
                Text = c.Name
            }).ToList();

            model.Products = products.ToList();

            ViewBag.ShopAvatar = shop.Avatar;
            ViewBag.ShopName = shop.Name;

            return View(model);
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
