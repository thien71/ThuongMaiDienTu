using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThuongMaiDienTu.Models
{
    public class ProductManage
    {
        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<tb_Product> Products { get; set; }
        public string SearchName { get; set; }
        public int? SelectedCategory { get; set; }
        public int? SelectedBrand { get; set; }
    }
}