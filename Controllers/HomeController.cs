﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                "NguoiDung.Phan_quyen;", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                HocVien hv = new HocVien
                {
                    Ho_va_ten = dr["Ho_va_ten"].ToString(), 
                    Tai_khoan = dr["Tai_khoan"].ToString(),
                    Mat_khau = dr["Mat_khau"].ToString(),
                    Phan_quyen = (Boolean)dr["Phan_quyen"],
                  
                };
                dsNguoiDung.Add(hv);
            }
            return View(dsNguoiDung);

        }
        [HttpPost]
        public ActionResult Check_Dangnhap(String user, String pass)
        {
            System.Diagnostics.Debug.WriteLine(user);
            System.Diagnostics.Debug.WriteLine(pass);
            var dsnguoidung = new DanhSachNguoiDung().listNguoiDung;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString)) 
            {
                conn.Open();
                try
                {
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
                                    return View("Index");

                                }
                                else
                                {
                                    // Phân quyền là false đi tới giao viên
                                    return RedirectToAction("Index","HomeTeacher", new { area = "Teacher" });
                                }
                            }
                            else
                            {
                                if(user=="admin" && pass == "admin")
                                {
                                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                                }
                                return Content("Sai Mat khau or tai khoan!!!!");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return View("Login");
                }
                
            }

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
            var lophoc=DanhSach_lopPiano();
             return View(lophoc);
        }
        public ActionResult Course_Guitar()
        {
            var lophoc=DanhSach_lopGuitar();
            return View(lophoc);
        }
        public ActionResult Course_Organ()
        {
            var lophoc= DanhSach_lopOrgan();
            return View(lophoc);
        }
        public ActionResult Course_Cajun()
        {
            return View();
        }
        public List<DanhSachLopHoc> DanhSach_lopPiano()
        {
            List<DanhSachLopHoc> dslophoc = new List<DanhSachLopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select lop.Ma_lop, lop.Ngay_bat_dau,lop.Ngay_ket_thuc,lop.Ten_lop_hoc,kh.Hoc_phi,tg.Thoi_gian_bat_dau,tg.Thoi_gian_ket_thuc,tg.Hoc_vao_thu" +
                "\r\nfrom LopHoc lop, KhoaHoc kh, ThoiGianBieu tg" +
                "\r\nwhere lop.Ma_khoa_hoc=kh.Ma_khoa_hoc" +
                " \r\nand lop.Ma_thoi_gian_bieu= tg.Ma_thoi_gian_bieu" +
                "\r\nand lop.Ma_khoa_hoc like 'p%'", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                DanhSachLopHoc dsLophoc= new DanhSachLopHoc
                {
                    Ngay_bat_dau = (DateTime)dr["Ngay_bat_dau"],
                    Ngay_ket_thuc = (DateTime)dr["Ngay_ket_thuc"],
                    Ten_Lop_hoc = dr["Ten_lop_hoc"].ToString(),
                    Hoc_phi = (Decimal)dr["Hoc_phi"],
                    Thoi_gian_bat_dau = (TimeSpan)dr["Thoi_gian_bat_dau"],
                    Thoi_gian_ket_thuc = (TimeSpan)dr["Thoi_gian_ket_thuc"],
                    Hoc_vao_thu = dr["Hoc_vao_thu"].ToString(),
                    Ma_lop = dr["Ma_lop"].ToString()
                };
                dslophoc.Add(dsLophoc);
            }


            return dslophoc;
        }
        public List<DanhSachLopHoc> DanhSach_lopGuitar()
        {
            List<DanhSachLopHoc> dslophoc = new List<DanhSachLopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select lop.Ngay_bat_dau,lop.Ngay_ket_thuc,lop.Ten_lop_hoc,kh.Hoc_phi,tg.Thoi_gian_bat_dau,tg.Thoi_gian_ket_thuc,tg.Hoc_vao_thu" +
                "\r\nfrom LopHoc lop, KhoaHoc kh, ThoiGianBieu tg" +
                "\r\nwhere lop.Ma_khoa_hoc=kh.Ma_khoa_hoc" +
                " \r\nand lop.Ma_thoi_gian_bieu= tg.Ma_thoi_gian_bieu" +
                "\r\nand lop.Ma_khoa_hoc like 'g%'", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                DanhSachLopHoc dsLophoc = new DanhSachLopHoc
                {
                    Ngay_bat_dau = (DateTime)dr["Ngay_bat_dau"],
                    Ngay_ket_thuc = (DateTime)dr["Ngay_ket_thuc"],
                    Ten_Lop_hoc = dr["Ten_lop_hoc"].ToString(),
                    Hoc_phi = (Decimal)dr["Hoc_phi"],
                    Thoi_gian_bat_dau = (TimeSpan)dr["Thoi_gian_bat_dau"],
                    Thoi_gian_ket_thuc = (TimeSpan)dr["Thoi_gian_ket_thuc"],
                    Hoc_vao_thu = dr["Hoc_vao_thu"].ToString(),
                };
                dslophoc.Add(dsLophoc);
            }


            return dslophoc;
        }
        public List<DanhSachLopHoc> DanhSach_lopOrgan()
        {
            List<DanhSachLopHoc> dslophoc = new List<DanhSachLopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select lop.Ngay_bat_dau,lop.Ngay_ket_thuc,lop.Ten_lop_hoc,kh.Hoc_phi,tg.Thoi_gian_bat_dau,tg.Thoi_gian_ket_thuc,tg.Hoc_vao_thu" +
                "\r\nfrom LopHoc lop, KhoaHoc kh, ThoiGianBieu tg" +
                "\r\nwhere lop.Ma_khoa_hoc=kh.Ma_khoa_hoc" +
                " \r\nand lop.Ma_thoi_gian_bieu= tg.Ma_thoi_gian_bieu" +
                "\r\nand lop.Ma_khoa_hoc like 'o%'", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                DanhSachLopHoc dsLophoc = new DanhSachLopHoc
                {
                    Ngay_bat_dau = (DateTime)dr["Ngay_bat_dau"],
                    Ngay_ket_thuc = (DateTime)dr["Ngay_ket_thuc"],
                    Ten_Lop_hoc = dr["Ten_lop_hoc"].ToString(),
                    Hoc_phi = (Decimal)dr["Hoc_phi"],
                    Thoi_gian_bat_dau = (TimeSpan)dr["Thoi_gian_bat_dau"],
                    Thoi_gian_ket_thuc = (TimeSpan)dr["Thoi_gian_ket_thuc"],
                    Hoc_vao_thu = dr["Hoc_vao_thu"].ToString(),
                };
                dslophoc.Add(dsLophoc);
            }


            return dslophoc;
        }


        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult detail(LopHoc lopHoc) {
        return View(lopHoc);
        }
        public ActionResult ChiTiet(string maLop) {

            try
            {
                System.Diagnostics.Debug.WriteLine(maLop);
                maLop = maLop.Trim();
                string query = "SELECT LopHoc.Ma_lop, LopHoc.Ma_khoa_hoc, LopHoc.Phong_hoc, LopHoc.So_luong_hoc_vien, LopHoc.Ten_lop_hoc, LopHoc.Mo_ta, LopHoc.Ma_thoi_gian_bieu, LopHoc.Ngay_bat_dau, LopHoc.Ngay_ket_thuc, LopHoc.So_tiet_hoc,ThoiGianBieu.Thoi_gian_bat_dau, ThoiGianBieu.Thoi_gian_ket_thuc, ThoiGianBieu.Hoc_vao_thu FROM LopHoc join ThoiGianBieu on LopHoc.Ma_thoi_gian_bieu=ThoiGianBieu.Ma_thoi_gian_bieu WHERE Ma_lop = @Ma_lop";
                SqlParameter parameter = new SqlParameter("@Ma_lop", maLop);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    LopHoc lopHoc = new LopHoc
                    {
                        Ma_lop = row["Ma_lop"].ToString(),
                        Ma_khoa_hoc = row["Ma_khoa_hoc"].ToString(),
                        Phong_hoc = row["Phong_hoc"].ToString(),
                        So_luong_hoc_vien = Convert.ToInt32(row["So_luong_hoc_vien"]),
                        Ten_lop_hoc = row["Ten_lop_hoc"].ToString(),
                        Mo_ta = row["Mo_ta"].ToString(),
                        Ma_thoi_gian_bieu = row["Ma_thoi_gian_bieu"].ToString(),
                        Ngay_bat_dau = (DateTime)row["Ngay_bat_dau"],
                        Ngay_ket_thuc = (DateTime)row["Ngay_ket_thuc"],
                        So_tiet_hoc = Convert.ToInt32(row["So_tiet_hoc"]),
                        Thoi_gian_bat_dau = (TimeSpan)row["Thoi_gian_bat_dau"],
                        Thoi_gian_ket_thuc = (TimeSpan)row["Thoi_gian_ket_thuc"],
                        Hoc_vao_thu = row["Hoc_vao_thu"].ToString()
                    };
                    return View("detail", lopHoc);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy lớp học";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Course_Piano");
            }
            

        }


        

    }
}