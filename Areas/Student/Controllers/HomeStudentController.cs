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
       
        public ActionResult settingAccount()
        {
            return View();
        }
        public ActionResult ScheduleStudent()
        {
            return View();
        }
        public ActionResult CourseStudent(string courseId, string register = "")
        {

            if (!string.IsNullOrEmpty(courseId))
            {
                KhoaHoc khoahoc = GetCourseInformation(courseId);

                if (khoahoc == null)
                {

                    return HttpNotFound();
                }
                return View("CourseStudent",khoahoc);
            }
            return RedirectToAction("Error");
        }
        private KhoaHoc GetCourseInformation(string courseId)
        {
            KhoaHoc khoahoc = null;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities3"].ConnectionString;

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

            return khoahoc;
        }
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
                                Hoc_phi = Convert.ToDecimal(rd["Hoc_Phi"]),
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

            return View("StudyStudent",dsKH);
        }
    }
}