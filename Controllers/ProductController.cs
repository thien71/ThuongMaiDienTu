using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Dynamic;
using System.IO;
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
        //public ActionResult Index()
        //{
        //    ViewBag.Products = db.tb_Product.Include(t => t.tb_Brand).Include(t => t.tb_ProductCategory).Include(t => t.tb_Shop).Include(t => t.tb_Supplier).ToList();
        //    ViewBag.Categories = db.tb_ProductCategory.ToList();
        //    ViewBag.Brands = db.tb_Brand.ToList();

        //    return View();
        //}

        public ActionResult Index(string search, string sortOrder, int? page)
        {
            var products = db.tb_Product.Include(t => t.tb_Brand)
                                        .Include(t => t.tb_ProductCategory)
                                        .Include(t => t.tb_Shop)
                                        .Include(t => t.tb_Supplier)
                                        .ToList();

            var categories = db.tb_ProductCategory.ToList();
            var brands = db.tb_Brand.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "newest":
                    products = products.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
                case "bestselling":
                    products = products.OrderByDescending(x => x.QuantitySold).ToList();
                    break;
                case "price_low_high":
                    products = products.OrderBy(x => x.Price).ToList();
                    break;
                case "price_high_low":
                    products = products.OrderByDescending(x => x.Price).ToList();
                    break;
                default:
                    products = products.OrderBy(x => Guid.NewGuid()).ToList();
                    break;
            }

            // Pagination logic
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Products = products;
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Search = search;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;

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
            ViewBag.ShopID = shop.ShopID;

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
            ViewBag.ShopID = shop.ShopID;
        
            return View(model);
        }


        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //int currentShopID = (int)Session["CurrentShopID"];

            var shop = db.tb_Shop.Find(id);
            if (shop == null)
            {
                return HttpNotFound("Shop không tồn tại.");
            }

            ViewBag.ShopAvatar = shop.Avatar;
            ViewBag.ShopName = shop.Name;

            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name");
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name");
            ViewBag.ShopID = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int shopID, [Bind(Include = "Name,Price,PromotionPrice,Quantity,Description,Detail,CateID,BrandID")] tb_Product tb_Product, HttpPostedFileBase uploadImage, IEnumerable<HttpPostedFileBase> uploadSubImages)
        {
            if (ModelState.IsValid)
            {
                tb_Product.ShopID = shopID;

                // Xử lý ảnh bìa
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string uploadDir = Server.MapPath("~/uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string path = Path.Combine(uploadDir, Path.GetFileName(uploadImage.FileName));
                    uploadImage.SaveAs(path);
                    tb_Product.Image = Path.GetFileName(uploadImage.FileName);
                }

                db.tb_Product.Add(tb_Product);
                db.SaveChanges();

                // Xử lý ảnh phụ
                if (uploadSubImages != null)
                {
                    foreach (HttpPostedFileBase file in uploadSubImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string uploadDir = Server.MapPath("~/uploads");
                            if (!Directory.Exists(uploadDir))
                            {
                                Directory.CreateDirectory(uploadDir);
                            }

                            string path2 = Path.Combine(uploadDir, Path.GetFileName(file.FileName));
                            file.SaveAs(path2);
                            tb_ListImage productImage = new tb_ListImage
                            {
                                ProductID = tb_Product.ProductID,
                                SRC = Path.GetFileName(file.FileName)
                            };
                            db.tb_ListImage.Add(productImage);
                        }
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Manage");
            }

            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name", tb_Product.BrandID);
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name", tb_Product.CateID);
            ViewBag.ShopID = shopID;
            return View(tb_Product);
        }


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

            var shop = db.tb_Shop.Find(tb_Product.ShopID);
            if (shop != null)
            {
                ViewBag.ShopAvatar = shop.Avatar;
                ViewBag.ShopName = shop.Name;
            }

            ViewBag.ListImages = db.tb_ListImage.Where(i => i.ProductID == id).ToList();

            return View(tb_Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Price,PromotionPrice,Quantity,Description,Detail,CateID,BrandID")] tb_Product tb_Product, HttpPostedFileBase uploadImage, IEnumerable<HttpPostedFileBase> uploadSubImages)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.tb_Product.Find(tb_Product.ProductID);

                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                existingProduct.Name = tb_Product.Name;
                existingProduct.Price = tb_Product.Price;
                existingProduct.PromotionPrice = tb_Product.PromotionPrice;
                existingProduct.Quantity = tb_Product.Quantity;
                existingProduct.Description = tb_Product.Description;
                existingProduct.Detail = tb_Product.Detail;
                existingProduct.CateID = tb_Product.CateID;
                existingProduct.BrandID = tb_Product.BrandID;

                // Xử lý ảnh bìa
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string uploadDir = Server.MapPath("~/uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string path = Path.Combine(uploadDir, Path.GetFileName(uploadImage.FileName));
                    uploadImage.SaveAs(path);
                    existingProduct.Image = Path.GetFileName(uploadImage.FileName);
                }

                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();

                // Xử lý ảnh phụ
                if (uploadSubImages != null)
                {
                    var existingImages = db.tb_ListImage.Where(img => img.ProductID == tb_Product.ProductID).ToList();
                    foreach (var img in existingImages)
                    {
                        db.tb_ListImage.Remove(img);
                    }
                    db.SaveChanges();

                    foreach (HttpPostedFileBase file in uploadSubImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string uploadDir = Server.MapPath("~/uploads");
                            if (!Directory.Exists(uploadDir))
                            {
                                Directory.CreateDirectory(uploadDir);
                            }

                            string path2 = Path.Combine(uploadDir, Path.GetFileName(file.FileName));
                            file.SaveAs(path2);
                            tb_ListImage productImage = new tb_ListImage
                            {
                                ProductID = tb_Product.ProductID,
                                SRC = Path.GetFileName(file.FileName)
                            };
                            db.tb_ListImage.Add(productImage);
                        }
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Manage");
            }

            ViewBag.BrandID = new SelectList(db.tb_Brand, "BrandID", "Name", tb_Product.BrandID);
            ViewBag.CateID = new SelectList(db.tb_ProductCategory, "CateID", "Name", tb_Product.CateID);
            ViewBag.ShopID = new SelectList(db.tb_Shop, "ShopID", "Name", tb_Product.ShopID);
            return View(tb_Product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                tb_Product tb_Product = db.tb_Product.Find(id);
                if (tb_Product == null)
                {
                    return HttpNotFound();
                }
                db.tb_Product.Remove(tb_Product);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                System.Diagnostics.Debug.WriteLine("Error deleting product: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError); // Trả về trạng thái lỗi 500
            }
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
