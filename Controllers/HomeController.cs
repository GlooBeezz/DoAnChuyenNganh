using System;
using System.Collections.Generic;
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
            SqlCommand cmd = new SqlCommand("select lop.Ngay_bat_dau,lop.Ngay_ket_thuc,lop.Ten_lop_hoc,kh.Hoc_phi,tg.Thoi_gian_bat_dau,tg.Thoi_gian_ket_thuc,tg.Hoc_vao_thu" +
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

    }
}