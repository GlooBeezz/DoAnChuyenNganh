using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web2.Areas.Admin.Data;
using web2.Models;

namespace web2.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage_Student()
        {
            HocVien hv= new HocVien();
            HocVienManage hocVienManage = new HocVienManage();
            hocVienManage.ThemHocVien(new HocVien { Ma_hoc_vien = "HV001", Lop_hoc_tham_gia = "LopA", Trang_thai_hoc_phi = true });

            // Lấy danh sách học viên
            List<HocVien> danhSachHocVien = hocVienManage.LayDanhSachHocVien();
            return View(danhSachHocVien);
        }
        public ActionResult Manage_Teacher()
        {
            return View();
        }
    }
}