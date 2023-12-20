using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using web2.Areas.Student.Data;
using web2.Models;
namespace web2.Areas.Student.Controllers
{
    public class HomeStudentController : Controller
    {
        private quanLyTrungTamDayDanEntities db = new quanLyTrungTamDayDanEntities();
        // GET: Student/HomeStudent
        public ActionResult AboutStudent()
        {

            return View();
        }
        public ActionResult ContactStudent()
        {
            return View();
        }

        public ActionResult indexHomeStudent()
        {
            return View();
        }
        public ActionResult LoginStudent()
        {
            return View();
        }

        [HttpGet]
        public ActionResult settingAccount()
        {
            var username = User.Identity.Name;
            using (var db = new quanLyTrungTamDayDanEntities())
            {
                var nguoiDung = db.NguoiDungs.Find(username);

                // Truyền thông tin người dùng vào trang SettingAccount
                return View(nguoiDung);
            }
        }
        [HttpPost]
        public ActionResult settingAccount(string Ma, string Ho_va_ten, string Email, string Dia_chi, string Sdt, DateTime Ngay_sinh)
        {   
            try
            {
                using (var db = new quanLyTrungTamDayDanEntities())
                {
                    var nguoiDung = db.NguoiDungs.Find(Ma);

                    if (nguoiDung != null)
                    {

                        nguoiDung.Ho_va_ten = Ho_va_ten;
                        nguoiDung.Ngay_sinh = Ngay_sinh;
                        nguoiDung.Sdt = Sdt;
                        nguoiDung.Email = Email;
                        nguoiDung.Dia_chi = Dia_chi;
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Cập nhật thông tin người dùng thành công";
                        return RedirectToAction("sttingAccount");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                        return RedirectToAction("settingAccount");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("settingAccount");
            }
        }

        public ActionResult ScheduleStudent(String courseId)
        {
            /*if (!String.IsNullOrEmpty(courseId))
            {
                LopHoc lopHoc = GetCourseInformation1(courseId);
                if (lopHoc == null)
                {
                    return HttpNotFound();
                }
                ThoiGianBieu tkb = new ThoiGianBieu
                {
                    Ma_thoi_gian_bieu = lopHoc.Ma_thoi_gian_bieu,
                    Hoc_vao_thu = lopHoc.ThoiGianBieu.Hoc_vao_thu,
                    Thoi_gian_bat_dau = lopHoc.ThoiGianBieu.Thoi_gian_bat_dau,
                    Thoi_gian_ket_thuc = lopHoc.ThoiGianBieu.Thoi_gian_ket_thuc
                };
                return View("ScheduleStudent", lopHoc);
            }*/
            return View();
        }
        public ActionResult CourseStudent(string courseId)
        {
            List<LopHoc> danhLopHoc = new List<LopHoc>(); // Khai báo danh sách
            if (!string.IsNullOrEmpty(courseId))
            {
                KhoaHoc khoahocs = GetCourseInformation(courseId);
                if (khoahocs == null)
                {
                    return HttpNotFound();
                }
                try
                {
                    LopHoc lopHoc = GetCourseInformation1(courseId);
                    lopHoc = new LopHoc
                    {
                        Ma_khoa_hoc = khoahocs.Ma_khoa_hoc,
                        Ten_lop_hoc = khoahocs.Ten_khoa_hoc,
                        Mo_ta = khoahocs.Mo_ta,
                    };
                    danhLopHoc.Add(lopHoc);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View("CourseStudent", danhLopHoc);
        }
/*        [HttpPost]
        public ActionResult Register(string courseId)
        {
            var lopHoc = db.LopHocs.SingleOrDefault(lh => lh.Ma_khoa_hoc == courseId);
            if (lopHoc == null)
            {
                return RedirectToAction("StudyStudent");
            }
            string maHocVien = User.Identity.Name;
            DangKyHoc dangKyHoc = new DangKyHoc
            {
                Ma_hoc_vien = maHocVien,
                Ma_lop = courseId,
                Ngay_dang_ky = DateTime.Now
            };
            db.DangKyHocs.Add(dangKyHoc);
            db.SaveChanges();
            var hocVien = db.HocViens.SingleOrDefault(hv => hv.Ma_hoc_vien == maHocVien);

            // Cập nhật Lop_hoc_tham_gia của học viên
            hocVien.Lop_hoc_tham_gia = courseId;
            db.SaveChanges();
            return RedirectToAction("CourseStudent", new { courseId = courseId });
        }*/
        public void SaveCourseStudent(LopHoc lh)
        {
            try
            {
                using (var db = new quanLyTrungTamDayDanEntities())
                {

                    lh.KhoaHoc = db.KhoaHocs.SingleOrDefault(kh => kh.Ma_khoa_hoc == lh.Ma_khoa_hoc);
                    db.LopHocs.Add(lh);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Ghi log lỗi để theo dõi và phân tích nguyên nhân lỗi
            }
        }
        private KhoaHoc GetCourseInformation(string courseId)
        {

            KhoaHoc khoahoc = null;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities3"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT Ma_khoa_hoc, Ten_khoa_hoc, Mo_ta, Hoc_phi, Nguon_anh FROM KhoaHoc WHERE Ma_khoa_hoc = @CourseId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseId", courseId);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                khoahoc = new KhoaHoc
                                {
                                    Ma_khoa_hoc = rd["Ma_khoa_hoc"].ToString(),
                                    Ten_khoa_hoc = rd["Ten_khoa_hoc"].ToString(),
                                    Mo_ta = rd["Mo_ta"].ToString(),
                                    Hoc_phi = Convert.ToDecimal(rd["Hoc_phi"]),
                                    Nguon_anh = rd["Nguon_anh"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return khoahoc;
        }
        private LopHoc GetCourseInformation1(string courseId)
        {

            LopHoc lopHoc = null;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities3"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT Ma_khoa_hoc, Ten_khoa_hoc, Mo_ta, Phong_hoc, So_luong_hoc_vien, So_tiet_hoc, Ngay_bat_dau, Ngay_ket_thuc FROM KhoaHoc WHERE Ma_khoa_hoc = @CourseId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseId", courseId);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                lopHoc = new LopHoc
                                {
                                    Ma_lop = rd["Ma_lop"].ToString(),
                                    Ten_lop_hoc = rd["Ten_lop_hoc"].ToString(),
                                    Mo_ta = rd["Mo_ta"].ToString(),
                                    Phong_hoc = rd["Phong_hoc"].ToString(),
                                    So_luong_hoc_vien = Convert.ToInt32(rd["So_luong_hoc_vien"].ToString()),
                                    So_tiet_hoc = Convert.ToInt32(rd["So_tiet_hoc"].ToString()),
                                    Ngay_bat_dau = Convert.ToDateTime(rd["Ngay_bat_dau"]),
                                    Ngay_ket_thuc = Convert.ToDateTime(rd["Ngay_ket_thuc"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return lopHoc;
        }

        /* public ActionResult Register(string maKhoaHoc)
         {
             // Lưu trữ khóa học vào session
             Session["MaKhoaHoc"] = maKhoaHoc;

             // Trở về trang CourseStudent
             return RedirectToAction("CourseStudent");
         }*/
        //ham truy vấn dữ liệu KHóa học
        private List<KhoaHoc> GetCoursesList()
        {
            List<KhoaHoc> dsKH = new List<KhoaHoc>();

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities3"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Ma_khoa_hoc,Ten_khoa_hoc,Mo_ta,  Hoc_phi,  Nguon_anh  FROM KhoaHoc", conn))
                {
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            KhoaHoc kh = new KhoaHoc
                            {
                                Ma_khoa_hoc = rd["Ma_Khoa_Hoc"].ToString(),
                                Ten_khoa_hoc = rd["Ten_Khoa_Hoc"].ToString(),
                                Mo_ta = rd["Mo_Ta"].ToString(),
                                Hoc_phi = Convert.ToDecimal(rd["Hoc_phi"]),
                                Nguon_anh = rd["Nguon_Anh"].ToString()
                            };
                            dsKH.Add(kh);
                        }
                    }
                }
            }

            return dsKH;
        }
        public ActionResult StudyStudent()
        {
            List<KhoaHoc> dsKH = GetCoursesList(); // Lấy danh sách khóa học lên trang StudyStudent
            var registeredCourses = dsKH.Where(khoachoc => db.DangKyHocs.Any(dangKyHoc => dangKyHoc.LopHoc.Ma_khoa_hoc == khoachoc.Ma_khoa_hoc && dangKyHoc.Ma_hoc_vien == User.Identity.Name));
            return View("StudyStudent", dsKH);
        }
    }
}