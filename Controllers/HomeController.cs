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
    public class HomeController : Controller
    {
        private dbThuongMaiDienTuEntities db = new dbThuongMaiDienTuEntities();
        public ActionResult Index()
        {
            ViewBag.IsFixedHeader = true;
            ViewBag.Products = db.tb_Product.Include(t => t.tb_Brand).Include(t => t.tb_ProductCategory).Include(t => t.tb_Shop).Include(t => t.tb_Supplier).ToList();
            ViewBag.Categories = db.tb_ProductCategory.ToList();
            return View();
        }
    }
}