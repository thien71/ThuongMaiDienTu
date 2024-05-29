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

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        // POST: User/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(string name, string password, string repassword)
        {
            if (ModelState.IsValid)
            {
                if (db.tb_Customer.Any(u => u.Username == name))
                {
                    ModelState.AddModelError("name", "Tên đăng nhập đã tồn tại.");
                }
                else if (password != repassword)
                {
                    ModelState.AddModelError("repassword", "Mật khẩu xác nhận không khớp.");
                }
                else
                {
                    var newUser = new tb_Customer
                    {
                        Username = name,
                        Password = HashPassword(password), // Implement password hashing
                        Name = "", 
                        Email = "",
                        Phone = "", 
                        Gender = "", 
                        DayOfBirth = DateTime.Now, 
                        Address = "", 
                        Avatar = "avatar_default.png",
                        RoleID = 1
                    };

                    db.tb_Customer.Add(newUser);
                    db.SaveChanges();

                    // Optionally log the user in after registration
                    Session["CustomerID"] = newUser.CustomerID;
                    Session["Username"] = newUser.Username;
                    Session["Avatar"] = newUser.Avatar;

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name, string password)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(password);

                var user = db.tb_Customer.SingleOrDefault(u => u.Username == name && u.Password == hashedPassword);
                if (user != null)
                {
                    // Set user session or authentication ticket
                    Session["CustomerID"] = user.CustomerID;
                    Session["Username"] = user.Username;
                    Session["Avatar"] = user.Avatar;

                    // Redirect to a secure area
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
            }

            return View();
        }


        private string HashPassword(string password)
        {
            return password;
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cart()
        {
            return View();
        }
    }
}
