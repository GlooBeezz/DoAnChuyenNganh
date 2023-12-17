using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web2.Areas.Admin.Data;
using web2.Models;

namespace web2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage_User() {

            // Lấy danh sách người dùng 
            var dsNguoiDung = new DanhSachNguoiDung().listNguoiDung;
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand Cmd = new SqlCommand("SELECT NguoiDung.Ho_va_ten, " +
                " NguoiDung.Tai_khoan, " +
                "NguoiDung.Mat_khau, " +
                "NguoiDung.Phan_quyen from NguoiDung;", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                NguoiDung nguoiDung = new NguoiDung
                {
                    Ho_va_ten = dr["Ho_va_ten"].ToString(), 
                    Tai_khoan = dr["Tai_khoan"].ToString(),
                    Mat_khau = dr["Mat_khau"].ToString(),
                    Phan_quyen = (Boolean)dr["Phan_quyen"],
                  
                };//????? cái câu truy vấn bị sai
                dsNguoiDung.Add(nguoiDung);
            }
            return View(dsNguoiDung);

        }
        public ActionResult Check_Dangnhap(String user, String pass)// gọi đúng theo cái name bên .cshtml
        {
            var dsnguoidung = new DanhSachNguoiDung().listNguoiDung;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString)) 
            {
                conn.Open();

                // Thực hiện truy vấn kiểm tra mật khẩu, tài khoản và phân quyền
                using (SqlCommand cmd = new SqlCommand("SELECT Tai_khoan, Mat_khau, Phan_quyen FROM NguoiDung WHERE Tai_khoan = @Tai_khoan AND Mat_khau = @Mat_khau", conn))
                {
                    cmd.Parameters.AddWithValue("@Tai_khoan", user);
                    cmd.Parameters.AddWithValue("@Mat_khau", pass);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Tài khoản và mật khẩu đúng
                            bool phanQuyen = (bool)dr["Phan_quyen"];

                            // Thực hiện kiểm tra phân quyền và trả về giá trị tương ứng
                            if (phanQuyen)
                            {
                                // Phân quyền là true, đi tới học viên
                                return RedirectToAction("Index", "HomeAdmin");
                               
                            }
                            else
                            {
                                // Phân quyền là false đi tới giao viên
                                return Content("2"); 
                            }
                        }
                        else
                        {
                            // Tài khoản hoặc mật khẩu không đúng
                            
                            return Content("Sai Mat khau or tai khoan!!!!"); // Trả về giá trị 0 hoặc thực hiện xử lý khác tùy ý
                        }
                    }
                }
            }





                return View("Login");

        }



            public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Study()
        {
            ViewBag.Message = "Your  page.";

            return View();
        }
        public ActionResult Course_Piano()
        {
            return View();
        }
        public ActionResult Course_Guitar()
        {
            return View();
        }
        public ActionResult Course_Organ()
        {
            return View();
        }
        public ActionResult Course_Cajun()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }


        

    }
}