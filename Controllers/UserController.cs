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
    public class UserController : Controller
    {
        private dbThuongMaiDienTuEntities db = new dbThuongMaiDienTuEntities();

        // GET: User
        public ActionResult Index()
        {
            var tb_Customer = db.tb_Customer.Include(t => t.tb_Role);
            return View(tb_Customer.ToList());
        }

        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Cart()
        {
            return View();
        }
    }
}
