using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using web2.Areas.Admin.Data;
using web2.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Drawing;

namespace web2.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        public DataHelper dbHelper;

        //Index

        public ActionResult Index()
        {
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connStr);

            int studentCount = dataAccess.CountStudents();
            int teacherCount = dataAccess.CountTeachers();
            int courseCount = dataAccess.CountCourses();
            int classCount = dataAccess.CountClasses();
            ChartView chartView = new ChartView();
            chartView.NumberOfStudents = studentCount;
            chartView.NumberOfTeachers = teacherCount;
            chartView.NumberOfCourses = courseCount;
            chartView.NumberOfClasses = classCount;
            return View(chartView);
        }

        // Học viên
        public ActionResult Manage_Student()
        {
            // Lấy danh sách học viên
            var dsHocVien = new DanhSachHocViencs().listHocVien;
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand Cmd = new SqlCommand("SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen,\r\n       HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi\r\nFROM HocVien\r\nJOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma;", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                HocVien hv = new HocVien
                {
                    Ma_hoc_vien = dr["Ma_hoc_vien"].ToString(),
                    Ho_va_ten = dr["Ho_va_ten"].ToString(),
                    Ngay_sinh = (DateTime)dr["Ngay_sinh"],
                    Sdt = dr["Sdt"].ToString(),
                    Email = dr["Email"].ToString(),
                    Dia_chi = dr["Dia_chi"].ToString(),
                    Tai_khoan = dr["Tai_khoan"].ToString(),
                    Mat_khau = dr["Mat_khau"].ToString(),
                    Phan_quyen = (Boolean)dr["Phan_quyen"],
                    Lop_hoc_tham_gia = dr["Lop_hoc_tham_gia"].ToString(),
                    Trang_thai_hoc_phi = (Boolean)dr["Trang_thai_hoc_phi"]
                };
                dsHocVien.Add(hv);
            }
            var errorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.ErrorMessage = errorMessage;
            return View(dsHocVien);
        }

        public ActionResult Edit_Student_Infor(string maHocVien)
        {
            try
            {
                string query = "SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi FROM HocVien JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma WHERE Ma_hoc_vien = @Ma_hoc_vien";
                SqlParameter parameter = new SqlParameter("@Ma_hoc_vien", maHocVien);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    HocVien hocVien = new HocVien
                    {
                        Ma_hoc_vien = row["Ma_hoc_vien"].ToString(),
                        Ho_va_ten = row["Ho_va_ten"].ToString(),
                        Ngay_sinh = (DateTime)row["Ngay_sinh"],
                        Sdt = row["Sdt"].ToString(),
                        Email = row["Email"].ToString(),
                        Dia_chi = row["Dia_chi"].ToString(),
                        Tai_khoan = row["Tai_khoan"].ToString(),
                        Mat_khau = row["Mat_khau"].ToString(),
                        Phan_quyen = (bool)row["Phan_quyen"],
                        Lop_hoc_tham_gia = row["Lop_hoc_tham_gia"].ToString(),
                        Trang_thai_hoc_phi = (bool)row["Trang_thai_hoc_phi"]
                    };
                    return View("Edit_Student_Infor", hocVien);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy học viên";
                    return RedirectToAction("Manage_Student");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Student");
            }
        }

        [HttpPost]
        public ActionResult Submit_Edit_Student_Infor(string Ma, string Ten, DateTime NgaySinh,  string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop,  string HocPhi)
        {
            try
            {
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                dbHelper = new DataHelper(connStr);
                HocVien hv = new HocVien
                {
                    Ma_hoc_vien = Ma,Ho_va_ten=Ten,Ngay_sinh=NgaySinh,Sdt=Sdt,Email=Email,Dia_chi=DiaChi,Tai_khoan=TaiKhoan,Mat_khau=MatKhau,Phan_quyen=true,Lop_hoc_tham_gia=Lop,Trang_thai_hoc_phi=(HocPhi=="true"?true:false)
                };
                if (dbHelper.ContainsDigitOrSpecialChar(Ten))
                {
                    ViewBag.ErrorMessage = "Tên chứa kí tự đặc biệt, vui lòng nhập lại.";
                    return View("Edit_Student_Infor", hv);
                }
                if (!dbHelper.IsPhoneNumberIsValid(Sdt))
                {
                    ViewBag.ErrorMessage = "Số điện thoại không đúng, vui lòng nhập lại.";
                    return View("Edit_Student_Infor", hv);
                }
                if (!dbHelper.IsBirthDateBeforeToday(NgaySinh))
                {
                    ViewBag.ErrorMessage = "Ngày sinh không hợp lệ, vui lòng nhập lại.";
                    return View("Edit_Student_Infor", hv);
                }
                string updateNguoiDungQuery = "UPDATE NguoiDung SET Ho_va_ten = @Ho_va_ten, Ngay_sinh = @Ngay_sinh, Sdt = @Sdt, Email = @Email, Dia_chi = @Dia_chi, Tai_khoan = @Tai_khoan, Mat_khau = @Mat_khau, Phan_quyen = @Phan_quyen WHERE Ma = @Ma_hoc_vien";
                string updateHocVienQuery = "UPDATE HocVien SET Lop_hoc_tham_gia = @Lop_hoc_tham_gia, Trang_thai_hoc_phi = @Trang_thai_hoc_phi WHERE Ma_hoc_vien = @Ma_hoc_vien";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Ma_hoc_vien", Ma),
                    new SqlParameter("@Ho_va_ten", Ten),
                    new SqlParameter("@Ngay_sinh",NgaySinh),
                    new SqlParameter("@Sdt", Sdt),
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@Dia_chi", DiaChi),
                    new SqlParameter("@Tai_khoan", TaiKhoan),
                    new SqlParameter("@Mat_khau", MatKhau),
                    new SqlParameter("@Phan_quyen", PhanQuyen),
                    new SqlParameter("@Lop_hoc_tham_gia", Lop),
                    new SqlParameter("@Trang_thai_hoc_phi", HocPhi)
                };

                DataAccess dataAccess = new DataAccess(connStr);

                // Thực hiện UPDATE cho bảng NguoiDung
                int rowsAffectedNguoiDung = dataAccess.ExecuteNonQuery(updateNguoiDungQuery, parameters);

                // Thực hiện UPDATE cho bảng HocVien
                int rowsAffectedHocVien = dataAccess.ExecuteNonQuery(updateHocVienQuery, parameters);

                if (rowsAffectedNguoiDung > 0 && rowsAffectedHocVien > 0)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin học viên thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật thông tin học viên không thành công";
                }

                return RedirectToAction("Manage_Student");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Student");
            }
        }

        [HttpPost]
        public ActionResult Find_Student(string maHocVienInput)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("B1");
                System.Diagnostics.Debug.WriteLine(maHocVienInput);
                var dsHocVien = new DanhSachHocViencs().listHocVien;
                string query = "SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi FROM HocVien JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma WHERE Ma_hoc_vien = @Ma_hoc_vien";
                SqlParameter parameter = new SqlParameter("@Ma_hoc_vien", maHocVienInput);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                dbHelper = new DataHelper(connStr); System.Diagnostics.Debug.WriteLine("B2");
                if (dbHelper.IsMaExistsInNguoiDung(maHocVienInput))
                {
                    DataAccess dataAccess = new DataAccess(connStr); System.Diagnostics.Debug.WriteLine("B2.1");
                    DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                    System.Diagnostics.Debug.WriteLine("check B2");
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        System.Diagnostics.Debug.WriteLine("B2.1");
                        HocVien hocVien = new HocVien
                        {
                            Ma_hoc_vien = row["Ma_hoc_vien"].ToString(),
                            Ho_va_ten = row["Ho_va_ten"].ToString(),
                            Ngay_sinh = (DateTime)row["Ngay_sinh"],
                            Sdt = row["Sdt"].ToString(),
                            Email = row["Email"].ToString(),
                            Dia_chi = row["Dia_chi"].ToString(),
                            Tai_khoan = row["Tai_khoan"].ToString(),
                            Mat_khau = row["Mat_khau"].ToString(),
                            Phan_quyen = (bool)row["Phan_quyen"],
                            Lop_hoc_tham_gia = row["Lop_hoc_tham_gia"].ToString(),
                            Trang_thai_hoc_phi = (bool)row["Trang_thai_hoc_phi"]
                        };
                        System.Diagnostics.Debug.WriteLine("B2.1.1");
                        dsHocVien.Add(hocVien);
                        System.Diagnostics.Debug.WriteLine("B2.1.2");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B3");
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        System.Diagnostics.Debug.WriteLine("B3.1");
                        SqlCommand Cmd = new SqlCommand("SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen,    HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi FROM HocVien JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma", conn);
                        SqlDataReader dr = Cmd.ExecuteReader();
                        System.Diagnostics.Debug.WriteLine("B3.2");
                        while (dr.Read())
                        {
                            HocVien hv = new HocVien
                            {
                                Ma_hoc_vien = dr["Ma_hoc_vien"].ToString(),
                                Ho_va_ten = dr["Ho_va_ten"].ToString(),
                                Ngay_sinh = (DateTime)dr["Ngay_sinh"],
                                Sdt = dr["Sdt"].ToString(),
                                Email = dr["Email"].ToString(),
                                Dia_chi = dr["Dia_chi"].ToString(),
                                Tai_khoan = dr["Tai_khoan"].ToString(),
                                Mat_khau = dr["Mat_khau"].ToString(),
                                Phan_quyen = (Boolean)dr["Phan_quyen"],
                                Lop_hoc_tham_gia = dr["Lop_hoc_tham_gia"].ToString(),
                                Trang_thai_hoc_phi = (Boolean)dr["Trang_thai_hoc_phi"]
                            };
                            dsHocVien.Add(hv); System.Diagnostics.Debug.WriteLine("B3.2.1");
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("B3.3");
                    if (maHocVienInput != "")
                    {
                        System.Diagnostics.Debug.WriteLine("B3.3.1");
                        ViewBag.ErrorMessage = "Mã không tồn tại. Vui lòng nhập mã khác.";
                    }
                }
                System.Diagnostics.Debug.WriteLine("sc");
                return View("Manage_Student", dsHocVien);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("out");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Student");
            }
        }

        public ActionResult Add_Student()
        {
            return View();
        }

        public ActionResult Submit_Add_Student(string Ma, string Ten, DateTime NgaySinh, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ThemLop, string HocPhi)
        {
            
            bool parsePhanQuyen = bool.Parse(PhanQuyen);
            bool parseHocPhi = bool.Parse(HocPhi);
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            dbHelper = new DataHelper(connStr);
            if (!dbHelper.IsUserIdIsValid(Ma))
            {
                ViewBag.ErrorMessage = "Mã đã đã nhập không đúng(tối đã chỉ 10 kí tự và phải bắt đầu bằng hv), vui lòng nhập lại.";
                return View("Add_Student");
            }
            if (dbHelper.ContainsDigitOrSpecialChar(Ten))
            {
                ViewBag.ErrorMessage = "Tên chứa kí tự đặc biệt, vui lòng nhập lại.";
                return View("Add_Student");
            }
            if (!dbHelper.IsPhoneNumberIsValid(Sdt))
            {
                ViewBag.ErrorMessage = "Số điện thoại không đúng, vui lòng nhập lại.";
                return View("Add_Student");
            }
            if (!dbHelper.IsBirthDateBeforeToday(NgaySinh))
            {
                ViewBag.ErrorMessage = "Ngày sinh không hợp lệ, vui lòng nhập lại.";
                return View("Add_Student");
            }
            if (!dbHelper.IsMaExistsInNguoiDung(Ma))
            {
                SqlCommand insertCmd = new SqlCommand("INSERT INTO NguoiDung (Ma, Ho_va_ten, Ngay_sinh, Sdt, Email, Dia_chi, Tai_khoan, Mat_khau, Phan_quyen) " +
                                           "VALUES (@Ma, @Ho_va_ten, @Ngay_sinh, @Sdt, @Email, @Dia_chi, @Tai_khoan, @Mat_khau, @Phan_quyen); " +
                                           "INSERT INTO HocVien (Ma_hoc_vien, Lop_hoc_tham_gia, Trang_thai_hoc_phi) " +
                                           "VALUES (@Ma_hoc_vien, @Lop_hoc_tham_gia, @Trang_thai_hoc_phi);", conn);


                insertCmd.Parameters.AddWithValue("@Ma", Ma);
                insertCmd.Parameters.AddWithValue("@Ho_va_ten", Ten);
                insertCmd.Parameters.AddWithValue("@Ngay_sinh", NgaySinh);
                insertCmd.Parameters.AddWithValue("@Sdt", Sdt);
                insertCmd.Parameters.AddWithValue("@Email", Email);
                insertCmd.Parameters.AddWithValue("@Dia_chi", DiaChi);
                insertCmd.Parameters.AddWithValue("@Tai_khoan", TaiKhoan);
                insertCmd.Parameters.AddWithValue("@Mat_khau", MatKhau);
                insertCmd.Parameters.AddWithValue("@Phan_quyen", parsePhanQuyen);

                // Thêm các tham số cho bảng HocVien
                insertCmd.Parameters.AddWithValue("@Ma_hoc_vien", Ma);
                insertCmd.Parameters.AddWithValue("@Lop_hoc_tham_gia", ThemLop != null ? string.Concat(Lop, ThemLop, " ") : Lop);
                insertCmd.Parameters.AddWithValue("@Trang_thai_hoc_phi", parseHocPhi);

                // Thực hiện câu lệnh INSERT
                int rowsAffected = insertCmd.ExecuteNonQuery();

                // Kiểm tra xem có bao nhiêu dòng đã được thêm
                if (rowsAffected > 0)
                {
                    // Insert thành công
                    TempData["SuccessMessage"] = "Thêm học viên thành công";
                }
                else
                {
                    // Insert không thành công
                    TempData["ErrorMessage"] = "Thêm học viên không thành công";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Mã đã tồn tại. Vui lòng nhập mã khác.";
                return View("Add_Student");
            }
            conn.Close();
            return RedirectToAction("Manage_Student");
        }

        public ActionResult Delete_Student_Infor(string maHocVien)
        {
            maHocVien = maHocVien.Trim();
            try
            {
                if (string.IsNullOrEmpty(maHocVien))
                {
                    TempData["ErrorMessage"] = "Mã học viên không hợp lệ";
                    return RedirectToAction("Manage_Student");
                }
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    string sql = "DELETE FROM HocVien WHERE Ma_hoc_vien = @Ma_hoc_vien DELETE FROM NguoiDung WHERE Ma=@Ma_hoc_vien";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma_hoc_vien", maHocVien);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Xóa học viên thành công";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Không tìm thấy học viên để xóa";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa học viên: " + ex.Message;
            }

            return RedirectToAction("Manage_Student");
        }

        // Giáo viên

        public ActionResult Manage_Teacher()
        {
            var dsGv = new DanhSachGiaoVien().listGiaoVien;
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select Ma_giao_vien,Ho_va_ten,Ngay_sinh,Sdt,Email,Dia_chi,Chuyen_mon,Luong_co_ban,Ma_lop_giang_day,Ma_tai_lieu_tai_len,Ma_bang_luong,Tai_khoan,Mat_khau,Phan_quyen\r\nfrom GiaoVien join NguoiDung on GiaoVien.Ma_giao_vien = NguoiDung.Ma", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                GiaoVien gv = new GiaoVien
                {
                    Ma_giao_vien = dr["Ma_giao_vien"].ToString(),
                    Ho_va_ten = dr["Ho_va_ten"].ToString(),
                    Ngay_sinh = (DateTime)dr["Ngay_sinh"],
                    Sdt = dr["Sdt"].ToString(),
                    Email = dr["Email"].ToString(),
                    Dia_chi = dr["Dia_chi"].ToString(),
                    Tai_khoan = dr["Tai_khoan"].ToString(),
                    Mat_khau = dr["Mat_khau"].ToString(),
                    Phan_quyen = (Boolean)dr["Phan_quyen"],
                    Chuyen_mon = dr["Chuyen_mon"].ToString(),
                    Luong_co_ban = Convert.ToDecimal(dr["Luong_co_ban"]),
                    Ma_lop_giang_day = dr["Ma_lop_giang_day"].ToString(),
                    Ma_tai_lieu_tai_len = dr["Ma_tai_lieu_tai_len"].ToString(),
                    Ma_bang_luong = dr["Ma_bang_luong"].ToString(),
                };
                dsGv.Add(gv);
            }
            var errorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.ErrorMessage = errorMessage;
            return View(dsGv);
        }

        public ActionResult Add_Teacher()
        {
            return View();
        }

        public ActionResult Submit_Add_Teacher(string Ma, string Ten, DateTime NgaySinh, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ChuyenMon, string LuongCoBan, string MaBangLuong, string MaTaiLieuTaiLen)
        {
            
            MaTaiLieuTaiLen = "none";
            System.Diagnostics.Debug.WriteLine(Ma, MaBangLuong, Lop, MaTaiLieuTaiLen, ChuyenMon, LuongCoBan);
            bool parsePhanQuyen = bool.Parse(PhanQuyen);
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            dbHelper = new DataHelper(connStr);
            if (!dbHelper.IsUserIdIsValid(Ma))
            {
                ViewBag.ErrorMessage = "Mã đã đã nhập không đúng(tối đã chỉ 10 kí tự và phải bắt đầu bằng hv), vui lòng nhập lại.";
                return View("Add_Teacher");
            }
            if (dbHelper.ContainsDigitOrSpecialChar(Ten))
            {
                ViewBag.ErrorMessage = "Tên chứa kí tự đặc biệt, vui lòng nhập lại.";
                return View("Add_Teacher");
            }
            if (!dbHelper.IsPhoneNumberIsValid(Sdt))
            {
                ViewBag.ErrorMessage = "Số điện thoại không đúng, vui lòng nhập lại.";
                return View("Add_Teacher");
            }
            if (!dbHelper.IsBirthDateBeforeToday(NgaySinh))
            {
                ViewBag.ErrorMessage = "Ngày sinh không hợp lệ, vui lòng nhập lại.";
                return View("Add_Teacher");
            }
            if (!dbHelper.IsMaExistsInNguoiDung(Ma))
            {
                SqlCommand insertCmd = new SqlCommand("INSERT INTO NguoiDung (Ma, Ho_va_ten, Ngay_sinh, Sdt, Email, Dia_chi, Tai_khoan, Mat_khau, Phan_quyen)\r\nVALUES (@Ma, @Ho_va_ten, @Ngay_sinh, @Sdt, @Email, @Dia_chi, @Tai_khoan, @Mat_khau, @Phan_quyen);\r\n\r\nINSERT INTO GiaoVien (Ma_giao_vien, Ma_bang_luong, Ma_lop_giang_day, Ma_tai_lieu_tai_len, Luong_co_ban, Chuyen_mon)\r\nVALUES (@Ma_giao_vien, @Ma_bang_luong, @Ma_lop_giang_day, @Ma_tai_lieu_tai_len, @Luong_co_ban,@Chuyen_mon);", conn);
                insertCmd.Parameters.AddWithValue("@Ma", Ma);
                insertCmd.Parameters.AddWithValue("@Ho_va_ten", Ten);
                insertCmd.Parameters.AddWithValue("@Ngay_sinh", NgaySinh);
                insertCmd.Parameters.AddWithValue("@Sdt", Sdt);
                insertCmd.Parameters.AddWithValue("@Email", Email);
                insertCmd.Parameters.AddWithValue("@Dia_chi", DiaChi);
                insertCmd.Parameters.AddWithValue("@Tai_khoan", TaiKhoan);
                insertCmd.Parameters.AddWithValue("@Mat_khau", MatKhau);
                insertCmd.Parameters.AddWithValue("@Phan_quyen", parsePhanQuyen);

                // Thêm các tham số cho bảng GiaoVien
                insertCmd.Parameters.AddWithValue("@Ma_giao_vien", Ma);
                insertCmd.Parameters.AddWithValue("@Ma_bang_luong", MaBangLuong);
                insertCmd.Parameters.AddWithValue("@Ma_lop_giang_day", Lop);
                insertCmd.Parameters.AddWithValue("@Ma_tai_lieu_tai_len", MaTaiLieuTaiLen);
                insertCmd.Parameters.AddWithValue("@Luong_co_ban", LuongCoBan);
                insertCmd.Parameters.AddWithValue("@Chuyen_mon", ChuyenMon);
                System.Diagnostics.Debug.WriteLine(Ma, MaBangLuong, Lop, MaTaiLieuTaiLen, ChuyenMon, LuongCoBan);
                // Thực hiện câu lệnh INSERT
                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Insert thành công
                    TempData["SuccessMessage"] = "Thêm giáo viên thành công";
                }
                else
                {
                    // Insert không thành công
                    TempData["ErrorMessage"] = "Thêm giáo viên không thành công";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Mã đã tồn tại. Vui lòng nhập mã khác.";
                return View("Add_Teacher");
            }
            conn.Close();
            return RedirectToAction("Manage_Teacher");
        }
        public ActionResult Edit_Teacher_Infor(string maGiaoVien)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(maGiaoVien);
                string query = "SELECT GiaoVien.Ma_giao_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, GiaoVien.Chuyen_mon, GiaoVien.Luong_co_ban,GiaoVien.Ma_bang_luong,GiaoVien.Ma_tai_lieu_tai_len,GiaoVien.Ma_lop_giang_day FROM GiaoVien JOIN NguoiDung ON GiaoVien.Ma_giao_vien = NguoiDung.Ma WHERE Ma_giao_vien = @Ma_giao_vien";
                SqlParameter parameter = new SqlParameter("@Ma_giao_vien", maGiaoVien);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
                if (dataTable.Rows.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("B2.1");
                    DataRow row = dataTable.Rows[0];
                    GiaoVien giaoVien = new GiaoVien()
                    {
                        Ma_giao_vien = row["Ma_giao_vien"].ToString(),
                        Ho_va_ten = row["Ho_va_ten"].ToString(),
                        Ngay_sinh = (DateTime)row["Ngay_sinh"],
                        Sdt = row["Sdt"].ToString(),
                        Email = row["Email"].ToString(),
                        Dia_chi = row["Dia_chi"].ToString(),
                        Tai_khoan = row["Tai_khoan"].ToString(),
                        Mat_khau = row["Mat_khau"].ToString(),
                        Phan_quyen = (bool)row["Phan_quyen"],
                        Chuyen_mon = row["Chuyen_mon"].ToString(),
                        Luong_co_ban = row["Luong_co_ban"] != DBNull.Value ? Convert.ToDecimal(row["Luong_co_ban"]) : 0m,
                        Ma_bang_luong = row["Ma_bang_luong"].ToString(),
                        Ma_lop_giang_day = row["Ma_lop_giang_day"].ToString(),
                        Ma_tai_lieu_tai_len = row["Ma_tai_lieu_tai_len"].ToString()

                    };
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    System.Diagnostics.Debug.WriteLine(giaoVien);
                    return View("Edit_Teacher_Infor", giaoVien);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B3");
                    TempData["ErrorMessage"] = "Không tìm thấy giáo viên";
                    return RedirectToAction("Manage_Teacher");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Teacher");
            }
        }

        [HttpPost]
        public ActionResult Submit_Edit_Teacher(string Ma, string Ten, DateTime NgaySinh, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ChuyenMon, string LuongCoBan, string MaBangLuong, string MaTaiLieuTaiLen)
        {
            try
            {
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                dbHelper = new DataHelper(connStr);
                bool Lcb;
                Decimal LcbDecimal;
                Lcb = Decimal.TryParse(LuongCoBan, out LcbDecimal);
                GiaoVien gv = new GiaoVien
                {
                    Ma_giao_vien = Ma,
                    Ho_va_ten = Ten,
                    Ngay_sinh = NgaySinh,
                    Sdt = Sdt,
                    Email = Email,
                    Dia_chi = DiaChi,
                    Tai_khoan = TaiKhoan,
                    Mat_khau = MatKhau,
                    Phan_quyen = false,
                    Ma_lop_giang_day = Lop,
                    Chuyen_mon = ChuyenMon,
                    Luong_co_ban = LcbDecimal,
                    Ma_bang_luong = MaBangLuong,
                    Ma_tai_lieu_tai_len = MaTaiLieuTaiLen
                };
                if (dbHelper.ContainsDigitOrSpecialChar(Ten))
                {
                    ViewBag.ErrorMessage = "Tên chứa kí tự đặc biệt, vui lòng nhập lại.";
                    ViewBag.Peak();
                    return View("Edit_Teacher_Infor",gv);
                }
                if (!dbHelper.IsPhoneNumberIsValid(Sdt))
                {
                    ViewBag.ErrorMessage = "Số điện thoại không đúng, vui lòng nhập lại.";
                    ViewBag.Peak();
                    return View("Edit_Teacher_Infor", gv);
                }
                if (!dbHelper.IsBirthDateBeforeToday(NgaySinh))
                {
                    ViewBag.ErrorMessage = "Ngày sinh không hợp lệ, vui lòng nhập lại.";
                    ViewBag.Peak();
                    return View("Edit_Teacher_Infor", gv);
                }
                if (!dbHelper.ContainsDigitOrSpecialChar(Ten) && dbHelper.IsPhoneNumberIsValid(Sdt) && dbHelper.IsBirthDateBeforeToday(NgaySinh))
                {
                    System.Diagnostics.Debug.WriteLine("B1");

                    // Update thông tin trong bảng NguoiDung
                    string updateNguoiDungQuery = "UPDATE NguoiDung SET Ho_va_ten = @Ho_va_ten, Ngay_sinh = @Ngay_sinh, Sdt = @Sdt, Email = @Email, Dia_chi = @Dia_chi, Tai_khoan = @Tai_khoan, Mat_khau = @Mat_khau, Phan_quyen = @Phan_quyen WHERE Ma = @Ma_giao_vien";
                    SqlParameter[] parametersNguoiDung =
                    {
                    new SqlParameter("@Ma_giao_vien", Ma),
                    new SqlParameter("@Ho_va_ten", Ten),
                    new SqlParameter("@Ngay_sinh", NgaySinh),
                    new SqlParameter("@Sdt", Sdt),
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@Dia_chi", DiaChi),
                    new SqlParameter("@Tai_khoan", TaiKhoan),
                    new SqlParameter("@Mat_khau", MatKhau),
                    new SqlParameter("@Phan_quyen", PhanQuyen)
                    };
                    DataAccess dataAccessNguoiDung = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString);
                    int rowsAffectedNguoiDung = dataAccessNguoiDung.ExecuteNonQuery(updateNguoiDungQuery, parametersNguoiDung);

                    // Update thông tin trong bảng GiaoVien
                    string updateGiaoVienQuery = "UPDATE GiaoVien SET Chuyen_mon = @Chuyen_mon, Luong_co_ban = @Luong_co_ban, Ma_lop_giang_day = @Ma_lop_giang_day, Ma_bang_luong = @Ma_bang_luong, Ma_tai_lieu_tai_len = @Ma_tai_lieu_tai_len WHERE Ma_giao_vien = @Ma_giao_vien";
                    SqlParameter[] parametersGiaoVien =
                    {
                    new SqlParameter("@Ma_giao_vien", Ma),
                    new SqlParameter("@Chuyen_mon", ChuyenMon),
                    new SqlParameter("@Luong_co_ban", LuongCoBan),
                    new SqlParameter("@Ma_lop_giang_day", Lop),
                    new SqlParameter("@Ma_bang_luong", MaBangLuong),
                    new SqlParameter("@Ma_tai_lieu_tai_len", MaTaiLieuTaiLen)
                    };
                    DataAccess dataAccessGiaoVien = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString);
                    int rowsAffectedGiaoVien = dataAccessGiaoVien.ExecuteNonQuery(updateGiaoVienQuery, parametersGiaoVien);
                    System.Diagnostics.Debug.WriteLine("B2");
                    if (rowsAffectedNguoiDung > 0 && rowsAffectedGiaoVien > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("B6.1");
                        TempData["SuccessMessage"] = "Cập nhật thông tin giáo viên thành công";
                        TempData.Peek("ErrorMessage");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("B6.2");
                        TempData["ErrorMessage"] = "Cập nhật thông tin giáo viên không thành công";
                        TempData.Peek("ErrorMessage");
                    }
                    System.Diagnostics.Debug.WriteLine("B7");
                    return RedirectToAction("Manage_Teacher");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Out");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                TempData.Peek("ErrorMessage");
                return RedirectToAction("Manage_Teacher");
            }
            return RedirectToAction("Manage_Teacher");
        }

        [HttpPost]
        public ActionResult Delete_Teacher_Infor(string maGiaoVien)
        {
            maGiaoVien = maGiaoVien.Trim();
            try
            {
                if (string.IsNullOrEmpty(maGiaoVien))
                {
                    TempData["ErrorMessage"] = "Mã giáo viên không hợp lệ";
                    return RedirectToAction("Manage_Teacher");
                }

                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string sql = "DELETE FROM GiaoVien WHERE Ma_giao_vien = @Ma_giao_vien DELETE FROM NguoiDung WHERE Ma=@Ma_giao_vien";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma_giao_vien", maGiaoVien);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Xóa giáo viên thành công";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Không tìm thấy giáo viên để xóa";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa giáo viên: " + ex.Message;
            }

            return RedirectToAction("Manage_Teacher");
        }

        [HttpPost]
        public ActionResult Find_Teacher(string maGiaoVienInput)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("B1");
                System.Diagnostics.Debug.WriteLine(maGiaoVienInput);
                var dsGiaoVien = new DanhSachGiaoVien().listGiaoVien;
                string query = "SELECT GiaoVien.Ma_giao_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, GiaoVien.Chuyen_mon, GiaoVien.Luong_co_ban, GiaoVien.Ma_lop_giang_day, GiaoVien.Ma_bang_luong, GiaoVien.Ma_tai_lieu_tai_len FROM GiaoVien JOIN NguoiDung ON GiaoVien.Ma_giao_vien = NguoiDung.Ma WHERE Ma_giao_vien = @Ma_giao_vien";
                SqlParameter parameter = new SqlParameter("@Ma_giao_vien", maGiaoVienInput);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    System.Diagnostics.Debug.WriteLine("B2.1");
                    GiaoVien giaoVien = new GiaoVien
                    {
                        Ma_giao_vien = row["Ma_giao_vien"].ToString(),
                        Ho_va_ten = row["Ho_va_ten"].ToString(),
                        Ngay_sinh = (DateTime)row["Ngay_sinh"],
                        Sdt = row["Sdt"].ToString(),
                        Email = row["Email"].ToString(),
                        Dia_chi = row["Dia_chi"].ToString(),
                        Tai_khoan = row["Tai_khoan"].ToString(),
                        Mat_khau = row["Mat_khau"].ToString(),
                        Phan_quyen = (bool)row["Phan_quyen"],
                        Chuyen_mon = row["Chuyen_mon"].ToString(),
                        Luong_co_ban = (decimal)row["Luong_co_ban"],
                        Ma_lop_giang_day = row["Ma_lop_giang_day"].ToString(),
                        Ma_bang_luong = row["Ma_bang_luong"].ToString(),
                        Ma_tai_lieu_tai_len = row["Ma_tai_lieu_tai_len"].ToString()
                    };
                    dsGiaoVien.Add(giaoVien);
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    return View("Manage_Teacher", dsGiaoVien);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var ds = new List<GiaoVien>();
                        SqlCommand Cmd = new SqlCommand("SELECT GiaoVien.Ma_giao_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, GiaoVien.Chuyen_mon, GiaoVien.Luong_co_ban, GiaoVien.Ma_lop_giang_day, GiaoVien.Ma_bang_luong, GiaoVien.Ma_tai_lieu_tai_len FROM GiaoVien JOIN NguoiDung ON GiaoVien.Ma_giao_vien = NguoiDung.Ma ", conn);
                        SqlDataReader dr = Cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            GiaoVien gv = new GiaoVien
                            {
                                Ma_giao_vien = dr["Ma_giao_vien"].ToString(),
                                Ho_va_ten = dr["Ho_va_ten"].ToString(),
                                Ngay_sinh = (DateTime)dr["Ngay_sinh"],
                                Sdt = dr["Sdt"].ToString(),
                                Email = dr["Email"].ToString(),
                                Dia_chi = dr["Dia_chi"].ToString(),
                                Tai_khoan = dr["Tai_khoan"].ToString(),
                                Mat_khau = dr["Mat_khau"].ToString(),
                                Phan_quyen = (Boolean)dr["Phan_quyen"],
                                Chuyen_mon = dr["Chuyen_mon"].ToString(),
                                Luong_co_ban = Convert.ToDecimal(dr["Luong_co_ban"]),
                                Ma_lop_giang_day = dr["Ma_lop_giang_day"].ToString(),
                                Ma_tai_lieu_tai_len = dr["Ma_tai_lieu_tai_len"].ToString(),
                                Ma_bang_luong = dr["Ma_bang_luong"].ToString(),
                            };
                            ds.Add(gv);
                        }
                        if (maGiaoVienInput != "")
                        {
                            ViewBag.ErrorMessage = "Mã không tồn tại. Vui lòng nhập mã khác.";
                        }
                        return View("Manage_Teacher", ds);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Teacher");
            }
        }

        //Lớp học

        public ActionResult Manage_Classes()
        {
            var dsLopHoc = new List<LopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand Cmd = new SqlCommand("SELECT\r\n    L.Ma_lop,\r\n    L.Ma_khoa_hoc,\r\n    L.Phong_hoc,\r\n    COUNT(H.Lop_hoc_tham_gia) AS So_luong_hoc_vien,\r\n    L.Ten_lop_hoc,\r\n    L.Mo_ta,\r\n    L.Ma_thoi_gian_bieu,\r\n    L.Ngay_bat_dau,\r\n    L.Ngay_ket_thuc,\r\n    L.So_tiet_hoc\r\nFROM\r\n    LopHoc L\r\nLEFT JOIN\r\n    HocVien H ON L.Ma_lop = H.Lop_hoc_tham_gia\r\nGROUP BY\r\n    L.Ma_lop,\r\n    L.Ma_khoa_hoc,\r\n    L.Phong_hoc,\r\n    L.Ten_lop_hoc,\r\n    L.Mo_ta,\r\n    L.Ma_thoi_gian_bieu,\r\n    L.Ngay_bat_dau,\r\n    L.Ngay_ket_thuc,\r\n    L.So_tiet_hoc;", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                LopHoc lop = new LopHoc
                {
                    Ma_lop = dr["Ma_lop"].ToString(),
                    Ma_khoa_hoc = dr["Ma_khoa_hoc"].ToString(),
                    Phong_hoc = dr["Phong_hoc"].ToString(),
                    So_luong_hoc_vien = Convert.ToInt32(dr["So_luong_hoc_vien"]),
                    Ten_lop_hoc = dr["Ten_lop_hoc"].ToString(),
                    Mo_ta = dr["Mo_ta"].ToString(),
                    Ma_thoi_gian_bieu = dr["Ma_thoi_gian_bieu"].ToString(),
                    Ngay_bat_dau = (DateTime)dr["Ngay_bat_dau"],
                    Ngay_ket_thuc = (DateTime)dr["Ngay_ket_thuc"],
                    So_tiet_hoc = Convert.ToInt32(dr["So_tiet_hoc"])
                };
                dsLopHoc.Add(lop);
            }
            var errorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.ErrorMessage = errorMessage;
            return View(dsLopHoc);
        }

        [HttpPost]
        public ActionResult Delete_Class_Infor(string maLop)
        {
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            dbHelper = new DataHelper(connStr);
            maLop = maLop.Trim();
            try
            {
                if (string.IsNullOrEmpty(maLop) )
                {
                    TempData["ErrorMessage"] = "Mã lớp không hợp lệ";
                    TempData.Peek("ErrorMessage");
                    return RedirectToAction("Manage_Classes");
                }
                if(dbHelper.KiemTraLopHocTonTai(maLop))
                {
                    TempData["ErrorMessage"] = "Lớp còn học viên hoặc giáo viên nên không thể xoá";
                    TempData.Peek("ErrorMessage");
                    return RedirectToAction("Manage_Classes");
                }
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string sql = "DELETE FROM LopHoc WHERE Ma_lop = @Ma_lop";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma_lop", maLop);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Xóa lớp thành công";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Không tìm thấy lớp để xóa";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa lớp: " + ex.Message;
            }

            return RedirectToAction("Manage_Classes");
        }

        public ActionResult Add_Class()
        {
            return View();
        }

        public ActionResult Submit_Add_Class(string Ma, string Ten, string MoTa, string Phong, string MaTKB, DateTime NgayBD, DateTime NgayKT, int SoTiet, string KhoaHoc, int? SoLuongHocVien,TimeSpan ThoiGianBatDau,TimeSpan ThoiGianKetThuc,string NgayHoc)
        {
            
            SoLuongHocVien = 0;
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            dbHelper = new DataHelper(connStr);
            try {
                System.Diagnostics.Debug.WriteLine("B1");
                if (NgayBD > NgayKT)
                {
                    ViewBag.ErrorMessage = "Ngày bắt đầu và Ngày kết thúc không hợp lệ. Vui lòng nhập lại.";
                    return View("Add_Class");
                }
                System.Diagnostics.Debug.WriteLine("B2");
                if (ThoiGianBatDau > ThoiGianKetThuc)
                {
                    ViewBag.ErrorMessage = "Thời gian bắt đầu và Thời gian kết thúc khôgn hợp lệ. Vui lòng nhập lại.";
                    return View("Add_Class");
                }
                System.Diagnostics.Debug.WriteLine("B3");
                if (!dbHelper.IsMaExistsInLopHoc(Ma))
                {
                    System.Diagnostics.Debug.WriteLine("B3.1");
                    SqlCommand insertCmd = new SqlCommand(" INSERT INTO LopHoc (Ma_lop, Ma_khoa_hoc, Phong_hoc, So_luong_hoc_vien, Ten_lop_hoc, Mo_ta, Ma_thoi_gian_bieu, Ngay_bat_dau, Ngay_ket_thuc, So_tiet_hoc)\r\n        VALUES (@Ma_lop, @Ma_khoa_hoc, @Phong_hoc, @So_luong_hoc_vien, @Ten_lop_hoc, @Mo_ta, @Ma_thoi_gian_bieu, @Ngay_bat_dau, @Ngay_ket_thuc, @So_tiet_hoc); Insert Into ThoiGianBieu (Ma_thoi_gian_bieu,Thoi_gian_bat_dau,Thoi_gian_ket_thuc,Hoc_vao_thu) values (@Ma_thoi_gian_bieu,@Thoi_gian_bat_dau,@Thoi_gian_ket_thuc,@Hoc_vao_thu); ", conn);
                    insertCmd.Parameters.AddWithValue("@Ma_lop", Ma);
                    insertCmd.Parameters.AddWithValue("@Ma_khoa_hoc", KhoaHoc);
                    insertCmd.Parameters.AddWithValue("@Phong_hoc", Phong);
                    insertCmd.Parameters.AddWithValue("@So_luong_hoc_vien", SoLuongHocVien);
                    insertCmd.Parameters.AddWithValue("Ten_lop_hoc", Ten);
                    insertCmd.Parameters.AddWithValue("@Mo_ta", MoTa);
                    insertCmd.Parameters.AddWithValue("@Ma_thoi_gian_bieu", MaTKB);
                    insertCmd.Parameters.AddWithValue("@Ngay_bat_dau", NgayBD);
                    insertCmd.Parameters.AddWithValue("@Ngay_ket_thuc", NgayKT);
                    insertCmd.Parameters.AddWithValue("@So_tiet_hoc", SoTiet);
                    insertCmd.Parameters.AddWithValue("@Thoi_gian_bat_dau", ThoiGianBatDau);
                    insertCmd.Parameters.AddWithValue("Thoi_gian_ket_thuc", ThoiGianKetThuc);
                    insertCmd.Parameters.AddWithValue("Hoc_vao_thu", NgayHoc);
                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    // Kiểm tra xem có bao nhiêu dòng đã được thêm
                    if (rowsAffected > 0)
                    {
                        // Insert thành công
                        TempData["SuccessMessage"] = "Thêm lớp thành công";
                    }
                    else
                    {
                        // Insert không thành công
                        TempData["ErrorMessage"] = "Thêm lớp không thành công";
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B3.2");
                    ViewBag.ErrorMessage = "Mã đã tồn tại. Vui lòng nhập mã khác.";
                    return View("Add_Class");
                }
                conn.Close();
                return RedirectToAction("Manage_Classes");
            }
            catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("Out");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "Gặp lỗi: " + ex;
                return View("Add_Class");
            }
            
        }

        public ActionResult Edit_Class_Infor(string maLop)
        {
            try
            {
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
                    return View("Edit_Class_Infor", lopHoc);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy lớp học";
                    return RedirectToAction("Manage_Classes");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Classes");
            }
        }

        [HttpPost]
        public ActionResult Submit_Edit_Class_Infor(string Ma, string Ten, string Phong, string MoTa, string MaTKB, DateTime NgayBD,  DateTime NgayKT, int SoTiet, string KhoaHoc, TimeSpan ThoiGianBatDau, TimeSpan ThoiGianKetThuc, string NgayHoc)
        {
            try
            {
                if(NgayBD>NgayKT&&ThoiGianBatDau>ThoiGianKetThuc)
                    return RedirectToAction("Manage_Classes");
                string updateLopHocQuery = "UPDATE LopHoc SET Ma_khoa_hoc = @Ma_khoa_hoc, Phong_hoc = @Phong_hoc, Ten_lop_hoc = @Ten_lop_hoc, Mo_ta = @Mo_ta, Ma_thoi_gian_bieu = @Ma_thoi_gian_bieu, Ngay_bat_dau = @Ngay_bat_dau, Ngay_ket_thuc = @Ngay_ket_thuc, So_tiet_hoc = @So_tiet_hoc WHERE Ma_lop = @Ma_lop; update  ThoiGianBieu set  Thoi_gian_bat_dau=@Thoi_gian_bat_dau, Thoi_gian_ket_thuc=@Thoi_gian_ket_thuc,Hoc_vao_thu=@Hoc_vao_thu where Ma_thoi_gian_bieu=@Ma_thoi_gian_bieu;";
                System.Diagnostics.Debug.WriteLine("B1");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Ma_lop", Ma),
                    new SqlParameter("@Ma_khoa_hoc", KhoaHoc),
                    new SqlParameter("@Phong_hoc", Phong),
                    new SqlParameter("@Ten_lop_hoc", Ten),
                    new SqlParameter("@Mo_ta", MoTa),
                    new SqlParameter("@Ma_thoi_gian_bieu", MaTKB),
                    new SqlParameter("@Ngay_bat_dau",  NgayBD),
                    new SqlParameter("@Ngay_ket_thuc",  NgayKT),
                    new SqlParameter("@Thoi_gian_bat_dau", ThoiGianBatDau),
                    new SqlParameter("@Thoi_gian_ket_thuc", ThoiGianKetThuc),
                    new SqlParameter("@Hoc_vao_thu", NgayHoc),
                    //new SqlParameter("@Ngay_bat_dau", NgayBDNew != null ? NgayBDNew : DateTime.ParseExact(NgayBD, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    //new SqlParameter("@Ngay_ket_thuc", NgayKTNew != null ? NgayKTNew : DateTime.ParseExact(NgayKT, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@So_tiet_hoc", SoTiet)
                };
                System.Diagnostics.Debug.WriteLine("B2");
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                System.Diagnostics.Debug.WriteLine("B3");
                // Thực hiện UPDATE cho bảng LopHoc
                int rowsAffected = dataAccess.ExecuteNonQuery(updateLopHocQuery, parameters);
                System.Diagnostics.Debug.WriteLine("B4");
                if (rowsAffected > 0)
                {
                    System.Diagnostics.Debug.WriteLine("B4.1");
                    TempData["SuccessMessage"] = "Cập nhật thông tin lớp học thành công";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B4.2");
                    TempData["ErrorMessage"] = "Cập nhật thông tin lớp học không thành công";
                }
                System.Diagnostics.Debug.WriteLine("B5");
                return RedirectToAction("Manage_Classes");
            }
            catch (SqlException sqlEx)
            {
                System.Diagnostics.Debug.WriteLine("SQL Exception: " + sqlEx.Message);
                foreach (SqlError error in sqlEx.Errors)
                {
                    System.Diagnostics.Debug.WriteLine("Error " + error.Number + ": " + error.Message);
                }

                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + sqlEx.Message;
                return RedirectToAction("Manage_Classes");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("out");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Classes");
            }
        }

        public ActionResult Find_Class(string maLopInput)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("B1");
                System.Diagnostics.Debug.WriteLine(maLopInput);
                var dsLopHoc = new List<LopHoc>();
                string query = "SELECT LopHoc.Ma_lop, LopHoc.Ma_khoa_hoc, LopHoc.Phong_hoc, LopHoc.So_luong_hoc_vien, LopHoc.Ten_lop_hoc, LopHoc.Mo_ta, LopHoc.Ma_thoi_gian_bieu, LopHoc.Ngay_bat_dau, LopHoc.Ngay_ket_thuc, LopHoc.So_tiet_hoc FROM LopHoc WHERE Ma_lop = @Ma_lop";
                SqlParameter parameter = new SqlParameter("@Ma_lop", maLopInput);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    System.Diagnostics.Debug.WriteLine("B2.1");
                    LopHoc lopHoc = new LopHoc
                    {
                        Ma_lop = row["Ma_lop"].ToString(),
                        Ma_khoa_hoc = row["Ma_khoa_hoc"].ToString(),
                        Phong_hoc = row["Phong_hoc"].ToString(),
                        So_luong_hoc_vien = (int)row["So_luong_hoc_vien"],
                        Ten_lop_hoc = row["Ten_lop_hoc"].ToString(),
                        Mo_ta = row["Mo_ta"].ToString(),
                        Ma_thoi_gian_bieu = row["Ma_thoi_gian_bieu"].ToString(),
                        Ngay_bat_dau = (DateTime)row["Ngay_bat_dau"],
                        Ngay_ket_thuc = (DateTime)row["Ngay_ket_thuc"],
                        So_tiet_hoc = (int)row["So_tiet_hoc"]
                    };
                    dsLopHoc.Add(lopHoc);
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    return View("Manage_Classes", dsLopHoc);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var ds = new List<LopHoc>();
                        SqlCommand Cmd = new SqlCommand("SELECT * FROM   LopHoc ", conn);
                        SqlDataReader dr = Cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            LopHoc lop = new LopHoc
                            {
                                Ma_lop = dr["Ma_lop"].ToString(),
                                Ma_khoa_hoc = dr["Ma_khoa_hoc"].ToString(),
                                Phong_hoc = dr["Phong_hoc"].ToString(),
                                So_luong_hoc_vien = Convert.ToInt32(dr["So_luong_hoc_vien"]),
                                Ten_lop_hoc = dr["Ten_lop_hoc"].ToString(),
                                Mo_ta = dr["Mo_ta"].ToString(),
                                Ma_thoi_gian_bieu = dr["Ma_thoi_gian_bieu"].ToString(),
                                Ngay_bat_dau = (DateTime)dr["Ngay_bat_dau"],
                                Ngay_ket_thuc = (DateTime)dr["Ngay_ket_thuc"],
                                So_tiet_hoc = Convert.ToInt32(dr["So_tiet_hoc"])
                            };
                            ds.Add(lop);
                        }
                        if (maLopInput != "")
                        {
                            ViewBag.ErrorMessage = "Mã không tồn tại. Vui lòng nhập mã khác.";
                        }
                        return View("Manage_Classes", ds);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Classes");
            }
        }

        [HttpPost]
        public ActionResult SelKhoaHoc(string maKhoaInput)
        {
            try
            {
                var dsLopHoc = new List<LopHoc>();
                System.Diagnostics.Debug.WriteLine("B1");
                System.Diagnostics.Debug.WriteLine(maKhoaInput);
                string query = "SELECT LopHoc.Ma_lop, LopHoc.Ma_khoa_hoc, LopHoc.Phong_hoc, LopHoc.So_luong_hoc_vien, LopHoc.Ten_lop_hoc, LopHoc.Mo_ta, LopHoc.Ma_thoi_gian_bieu, LopHoc.Ngay_bat_dau, LopHoc.Ngay_ket_thuc, LopHoc.So_tiet_hoc FROM LopHoc WHERE Ma_khoa_hoc = @Ma_khoa_hoc";
                SqlParameter parameter = new SqlParameter("@Ma_khoa_hoc", maKhoaInput);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    System.Diagnostics.Debug.WriteLine("B2.1");
                    LopHoc lopHoc = new LopHoc
                    {
                        Ma_lop = row["Ma_lop"].ToString(),
                        Ma_khoa_hoc = row["Ma_khoa_hoc"].ToString(),
                        Phong_hoc = row["Phong_hoc"].ToString(),
                        So_luong_hoc_vien = (int)row["So_luong_hoc_vien"],
                        Ten_lop_hoc = row["Ten_lop_hoc"].ToString(),
                        Mo_ta = row["Mo_ta"].ToString(),
                        Ma_thoi_gian_bieu = row["Ma_thoi_gian_bieu"].ToString(),
                        Ngay_bat_dau = (DateTime)row["Ngay_bat_dau"],
                        Ngay_ket_thuc = (DateTime)row["Ngay_ket_thuc"],
                        So_tiet_hoc = (int)row["So_tiet_hoc"]
                    };
                    dsLopHoc.Add(lopHoc);
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    return View("Manage_Classes", dsLopHoc);
                }
                return RedirectToAction("Manage_Classes");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Classes");
            }
        }

        //Khoá học

        public ActionResult Manage_Course()
        {
            System.Diagnostics.Debug.WriteLine("B1");
            var dsKhoaHoc = new List<KhoaHoc>();
            SqlConnection conn = null;
            try
            {
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                conn = new SqlConnection(connStr);
                conn.Open();
                System.Diagnostics.Debug.WriteLine("B2");
                SqlCommand Cmd = new SqlCommand("SELECT * FROM   KhoaHoc ", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("B3");

                while (dr.Read())
                {
                    System.Diagnostics.Debug.WriteLine("B4");
                    KhoaHoc khoaHoc = new KhoaHoc
                    {
                        Ma_khoa_hoc = dr["Ma_khoa_hoc"].ToString(),
                        Ten_khoa_hoc = dr["Ten_khoa_hoc"].ToString(),
                        Mo_ta = dr["Mo_ta"].ToString(),
                        Hoc_phi = Convert.ToDecimal(dr["Hoc_phi"]),
                        Nguon_anh = dr["Nguon_anh"].ToString()
                    };
                    //System.Diagnostics.Debug.WriteLine($"Ma_khoa_hoc: {khoaHoc.Ma_khoa_hoc}, Ten_khoa_hoc: {khoaHoc.Ten_khoa_hoc}, Mo_ta: {khoaHoc.Mo_ta}, Hoc_phi: {khoaHoc.Hoc_phi}, Nguon_anh: {khoaHoc.Nguon_anh}");
                    dsKhoaHoc.Add(khoaHoc);
                }
                System.Diagnostics.Debug.WriteLine("B5");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            var errorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.ErrorMessage = errorMessage;
            return View(dsKhoaHoc);
        }

        [HttpPost]
        public ActionResult Delete_Course_Infor(string maKhoaHoc)
        {
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            maKhoaHoc = maKhoaHoc.Trim();
            dbHelper=new DataHelper(connStr);
            try
            {
                if (string.IsNullOrEmpty(maKhoaHoc) )
                {
                    TempData["ErrorMessage"] = "Mã khóa học không hợp lệ";
                    return RedirectToAction("Manage_Course");
                }
                if (dbHelper.CheckKhoaHocCoLopCon(maKhoaHoc))
                {
                    {
                        TempData["ErrorMessage"] = "Khoá học vẫn còn lớp bên trong";
                        return RedirectToAction("Manage_Course");
                    }
                }
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string sql = "DELETE FROM KhoaHoc WHERE Ma_khoa_hoc = @Ma_khoa_hoc";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma_khoa_hoc", maKhoaHoc);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Xóa khóa học thành công";
                            return RedirectToAction("Manage_Course");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Không tìm thấy khóa học để xóa";
                            return RedirectToAction("Manage_Course");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa khóa học: " + ex.Message;
                return RedirectToAction("Manage_Course");
            }

            return RedirectToAction("Manage_Course");
        }

        public ActionResult Add_Course()
        {
            return View();
        }

        public ActionResult Submit_Add_Course(string Ma, string Ten, string MoTa, decimal HocPhi, string NguonAnh)
        {
            
            System.Diagnostics.Debug.WriteLine("B1");
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            System.Diagnostics.Debug.WriteLine(Ma);
            dbHelper = new DataHelper(connStr);
            if (!dbHelper.IsMaExistsInKhoaHoc(Ma))
            {
                System.Diagnostics.Debug.WriteLine("B2");
                SqlCommand insertCmd = new SqlCommand("INSERT INTO KhoaHoc (Ma_khoa_hoc, Ten_khoa_hoc, Mo_ta, Hoc_phi, Nguon_anh)\r\nVALUES (@Ma_khoa_hoc, @Ten_khoa_hoc, @Mo_ta, @Hoc_phi, @Nguon_anh)", conn);
                insertCmd.Parameters.AddWithValue("@Ma_khoa_hoc", Ma);
                insertCmd.Parameters.AddWithValue("@Ten_khoa_hoc", Ten);
                insertCmd.Parameters.AddWithValue("@Mo_ta", MoTa);
                insertCmd.Parameters.AddWithValue("@Hoc_phi", HocPhi);
                insertCmd.Parameters.AddWithValue("@Nguon_anh", NguonAnh);
                int rowsAffected = insertCmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("B3");
                if (rowsAffected > 0)
                {
                    TempData["SuccessMessage"] = "Thêm khóa học thành công"; System.Diagnostics.Debug.WriteLine("B3.1");
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm khóa học không thành công"; System.Diagnostics.Debug.WriteLine("B3.2");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Mã đã tồn tại. Vui lòng nhập mã khác.";
                return View("Add_Course");
            }
            conn.Close();
            System.Diagnostics.Debug.WriteLine("B4");
            return RedirectToAction("Manage_Course");

        }

        public ActionResult Edit_Course_Infor(string maKhoaHoc)
        {
            try
            {
                string query = "SELECT KhoaHoc.Ma_khoa_hoc, KhoaHoc.Ten_khoa_hoc, KhoaHoc.Mo_ta, KhoaHoc.Hoc_phi, KhoaHoc.Nguon_anh FROM KhoaHoc WHERE Ma_khoa_hoc = @Ma_khoa_hoc";
                SqlParameter parameter = new SqlParameter("@Ma_khoa_hoc", maKhoaHoc);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    KhoaHoc khoaHoc = new KhoaHoc
                    {
                        Ma_khoa_hoc = row["Ma_khoa_hoc"].ToString(),
                        Ten_khoa_hoc = row["Ten_khoa_hoc"].ToString(),
                        Mo_ta = row["Mo_ta"].ToString(),
                        Hoc_phi = Convert.ToDecimal(row["Hoc_phi"]),
                        Nguon_anh = row["Nguon_anh"].ToString()
                    };
                    return View("Edit_Course_Infor", khoaHoc);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khóa học";
                    return RedirectToAction("Manage_Course");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Course");
            }
        }

        [HttpPost]
        public ActionResult Submit_Edit_Course_Infor(string Ma, string Ten, string MoTa, decimal HocPhi, string NguonAnh, string NguonAnhNew)
        {
            System.Diagnostics.Debug.WriteLine(Ma);
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            dbHelper=new DataHelper(connStr);
            if (!dbHelper.checkHocPhi(HocPhi))
            {
                ViewBag.ErrorMessage = "Học phí bé hơn 0.";
                return RedirectToAction("Edit_Course_Infor");
            }
            try
            {
                string updateKhoaHocQuery = "UPDATE KhoaHoc SET Ten_khoa_hoc = @Ten_khoa_hoc, Mo_ta = @Mo_ta, Hoc_phi = @Hoc_phi, Nguon_anh = @Nguon_anh WHERE Ma_khoa_hoc = @Ma_khoa_hoc";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Ma_khoa_hoc", Ma),
                    new SqlParameter("@Ten_khoa_hoc", Ten),
                    new SqlParameter("@Mo_ta", MoTa),
                    new SqlParameter("@Hoc_phi", HocPhi),
                    new SqlParameter("@Nguon_anh", NguonAnhNew != null?NguonAnhNew:NguonAnh)
                };
                DataAccess dataAccess = new DataAccess(connStr);
                int rowsAffected = dataAccess.ExecuteNonQuery(updateKhoaHocQuery, parameters);

                if (rowsAffected > 0)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin khóa học thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật thông tin khóa học không thành công";
                }
                return RedirectToAction("Manage_Course");
            }
            catch (SqlException sqlEx)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + sqlEx.Message;
                return RedirectToAction("Manage_Course");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Course");
            }
        }

        public ActionResult Find_Course(string maKhoaInput)
        {
            try
            {
                var dsKhoaHoc = new List<KhoaHoc>();
                string query = "SELECT KhoaHoc.Ma_khoa_hoc, KhoaHoc.Ten_khoa_hoc, KhoaHoc.Mo_ta, KhoaHoc.Hoc_phi, KhoaHoc.Nguon_anh FROM KhoaHoc WHERE Ma_khoa_hoc = @Ma_khoa_hoc";
                SqlParameter parameter = new SqlParameter("@Ma_khoa_hoc", maKhoaInput);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    KhoaHoc khoaHoc = new KhoaHoc
                    {
                        Ma_khoa_hoc = row["Ma_khoa_hoc"].ToString(),
                        Ten_khoa_hoc = row["Ten_khoa_hoc"].ToString(),
                        Mo_ta = row["Mo_ta"].ToString(),
                        Hoc_phi = Convert.ToDecimal(row["Hoc_phi"]),
                        Nguon_anh = row["Nguon_anh"].ToString()
                    };
                    dsKhoaHoc.Add(khoaHoc);
                    return View("Manage_Course", dsKhoaHoc);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var ds = new List<KhoaHoc>();
                        SqlCommand Cmd = new SqlCommand("SELECT * FROM   KhoaHoc ", conn);
                        SqlDataReader dr = Cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            KhoaHoc khoaHoc = new KhoaHoc
                            {
                                Ma_khoa_hoc = dr["Ma_khoa_hoc"].ToString(),
                                Ten_khoa_hoc = dr["Ten_khoa_hoc"].ToString(),
                                Mo_ta = dr["Mo_ta"].ToString(),
                                Hoc_phi = Convert.ToDecimal(dr["Hoc_phi"]),
                                Nguon_anh = dr["Nguon_anh"].ToString()
                            };
                            ds.Add(khoaHoc);
                        }
                        if (maKhoaInput != "")
                        {
                            ViewBag.ErrorMessage = "Mã không tồn tại. Vui lòng nhập mã khác.";
                        }
                        return View("Manage_Course", ds);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Course");
            }
        }

        //Gửi thông báo qua mail

        public ActionResult Send_Notification()
        {
            web2.Models.quanLyTrungTamDayDanEntities db = new web2.Models.quanLyTrungTamDayDanEntities();
            var model = new ThongBaoModel(); // Tạo đối tượng model để truyền vào view
            model.KhoaHocs = db.KhoaHocs.ToList();
            model.LopHocs = db.LopHocs.ToList();
            return View(model);

        }
        public ActionResult Submit_Send_Notification(ThongBaoModel model)
        {
            System.Diagnostics.Debug.WriteLine("********************************************************************************************************Gửi Mail*******************************************************************************************");
            System.Diagnostics.Debug.WriteLine(model.Tieude);
            System.Diagnostics.Debug.WriteLine(model.NguoiNhan);
            System.Diagnostics.Debug.WriteLine(model.Noidung);
            System.Diagnostics.Debug.WriteLine(model.NguoiNhan_GiaoVien);
            System.Diagnostics.Debug.WriteLine(model.NguoiNhan_HocVien);
            System.Diagnostics.Debug.WriteLine(model.maLopInput);
            System.Diagnostics.Debug.WriteLine(model.maKhoaInput);
            web2.Models.quanLyTrungTamDayDanEntities db = new web2.Models.quanLyTrungTamDayDanEntities();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            dbHelper = new DataHelper(connStr);
            List<String> list = new List<String>();
            try
            {
                // Thực hiện kiểm tra và xử lý dữ liệu
                System.Diagnostics.Debug.WriteLine("B2");
                if (model.NguoiNhan_GiaoVien == true && (model.maKhoaInput != null || model.maLopInput != null))
                {
                    System.Diagnostics.Debug.WriteLine("RDB chọn giáo viên");
                    if (model.maKhoaInput != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Vào hàm gửi theo khoá");
                        string querySendTeacherInCourse = "select NguoiDung.Email from GiaoVien join LopHoc on Ma_lop=Ma_lop_giang_day join NguoiDung on Ma=Ma_giao_vien where Ma_khoa_hoc=@Ma_khoa_hoc";
                        SqlCommand cmd = new SqlCommand(querySendTeacherInCourse, conn);
                        cmd.Parameters.AddWithValue("@Ma_khoa_hoc", model.maKhoaInput);
                        System.Diagnostics.Debug.WriteLine("Excute");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string email = reader["Email"].ToString();
                            list.Add(email);
                        }
                        foreach (string item in list)
                        {
                            SendEmail(item, model.Tieude, model.Noidung);
                            System.Diagnostics.Debug.WriteLine("Gửi mail thành công cho " + item);
                        }
                    }
                    else if (model.maLopInput != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Vào hàm gửi theo lớp");
                        string querySendTeacherInClass = "select NguoiDung.Email from GiaoVien join NguoiDung on Ma=Ma_giao_vien where Ma_lop_giang_day=@Ma_lop_hoc";
                        SqlCommand cmd = new SqlCommand(querySendTeacherInClass, conn);
                        cmd.Parameters.AddWithValue("@Ma_lop_hoc", model.maLopInput);
                        System.Diagnostics.Debug.WriteLine("Excute");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string email = reader["Email"].ToString();
                            list.Add(email);
                        }
                        foreach (string item in list)
                        {
                            SendEmail(item, model.Tieude, model.Noidung);
                            System.Diagnostics.Debug.WriteLine("Gửi mail thành công cho " + item);
                        }
                    }
                }
                else if (model.NguoiNhan_HocVien == true && (model.maKhoaInput != null || model.maLopInput != null))
                {
                    System.Diagnostics.Debug.WriteLine("RDB chọn học viên");
                    if (model.maKhoaInput != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Vào hàm gửi theo khoá");
                        string querySendStudentInCourse = "select NguoiDung.Email from HocVien join LopHoc on Ma_lop=Lop_hoc_tham_gia join NguoiDung on Ma=Ma_hoc_vien where Ma_khoa_hoc=@Ma_khoa_hoc";
                        SqlCommand cmd = new SqlCommand(querySendStudentInCourse, conn);
                        cmd.Parameters.AddWithValue("@Ma_khoa_hoc", model.maKhoaInput);
                        System.Diagnostics.Debug.WriteLine("Excute");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string email = reader["Email"].ToString();
                            list.Add(email);
                        }
                        foreach (string item in list)
                        {
                            SendEmail(item, model.Tieude, model.Noidung);
                            System.Diagnostics.Debug.WriteLine("Gửi mail thành công cho " + item);
                        }
                    }
                    else if (model.maLopInput != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Vào hàm gửi theo lớp");
                        string querySendStudentInClass = "select NguoiDung.Email from HocVien join NguoiDung on Ma=Ma_hoc_vien where Lop_hoc_tham_gia=@Lop_hoc_tham_gia";
                        SqlCommand cmd = new SqlCommand(querySendStudentInClass, conn);
                        cmd.Parameters.AddWithValue("@Lop_hoc_tham_gia", model.maLopInput);
                        System.Diagnostics.Debug.WriteLine("Excute");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string email = reader["Email"].ToString();
                            list.Add(email);
                        }
                        foreach (string item in list)
                        {
                            SendEmail(item, model.Tieude, model.Noidung);
                            System.Diagnostics.Debug.WriteLine("Gửi mail thành công cho " + item);
                        }
                    }
                }
                else if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("B2.1");
                    SendEmail(model.NguoiNhan, model.Tieude, model.Noidung);
                    ViewBag.Message = "Gửi mail thành công!";
                    return RedirectToAction("Send_Notification");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B3");
                ViewBag.ErrorMessage = "Đã xảy ra lỗi: " + ex.Message;
            }
            System.Diagnostics.Debug.WriteLine("B4");
            list.Clear();
            var model1 = new ThongBaoModel();
            model1.KhoaHocs = db.KhoaHocs.ToList();
            model1.LopHocs = db.LopHocs.ToList();
            return View("Send_Notification", model1);
        }
        public void SendEmail(string toAddress, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "phothimusicofficial@gmail.com";
            string smtpPassword = "nhph srib xees iiou";


            string fromAddress = "phothimusicofficial@gmail.com";

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mailMessage);
                System.Diagnostics.Debug.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        // Hoá đơn

        public ActionResult Manage_Invoice()
        {
            List<HoaDon> ds = new List<HoaDon>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand("select * from HoaDon", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                while (dr.Read())
                {
                    HoaDon invoce = new HoaDon
                    {
                        Ma_hoa_don = dr["Ma_hoa_don"].ToString(),
                        Ma_dang_ky = dr["Ma_dang_ky"].ToString(),
                        Ngay_tao = (DateTime)dr["Ngay_tao"],
                        Tong_tien = (decimal)dr["Tong_tien"],
                        Trang_thai_thanh_toan = Convert.ToBoolean(dr["Trang_thai_thanh_toan"])
                    };
                    ds.Add(invoce);
                }
                return View(ds);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                return View(ds);
            }
        }
        public ActionResult Add_Invoice(DangKyHoc hoaDon)
        {
            String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connectionString);

            try
            {
                dbHelper = new DataHelper(connectionString);
                System.Diagnostics.Debug.WriteLine(hoaDon.Ma_hoa_don);
                /*if (!dbHelper.IsMaExistsInHoaDon(hoaDon.Ma_hoa_don))
                {*/
                    // Thêm hóa đơn
                    string hoaDonQuery = "INSERT INTO HoaDon (Ma_hoa_don, Ma_dang_ky, Ngay_tao, Tong_tien, Trang_thai_thanh_toan) " +
                                        "VALUES (@Ma_hoa_don_hoa_don, @Ma_dang_ky_hoa_don, @Ngay_tao_hoa_don, @Tong_tien_hoa_don, @Trang_thai_thanh_toan_hoa_don)";
                    System.Diagnostics.Debug.WriteLine("B1");
                    SqlParameter[] hoaDonParameters =
                    {
                        new SqlParameter("@Ma_hoa_don_hoa_don", hoaDon.Ma_hoa_don),
                        new SqlParameter("@Ma_dang_ky_hoa_don", hoaDon.Ma_dang_ky),
                        new SqlParameter("@Ngay_tao_hoa_don", hoaDon.Ngay_tao),
                        new SqlParameter("@Tong_tien_hoa_don", hoaDon.Tong_tien),
                        new SqlParameter("@Trang_thai_thanh_toan_hoa_don", hoaDon.Trang_thai_thanh_toan)
                    };
                    int rowsAffectedHoaDon = dataAccess.ExecuteNonQuery(hoaDonQuery, hoaDonParameters);
                    System.Diagnostics.Debug.WriteLine("B2");

                // Thêm đăng ký học
                string dangKyHocQuery = "INSERT INTO DangKyHoc (Ma_dang_ky, Ma_hoc_vien, Ma_lop, Ma_hoa_don, Ngay_dang_ky) " +
                    "VALUES (@Ma_dang_ky, @Ma_hoc_vien, @Ma_lop, @Ma_hoa_don, @Ngay_dang_ky)";
                    SqlParameter[] dangKyHocParameters =
                    {
                        new SqlParameter("@Ma_dang_ky", hoaDon.Ma_dang_ky),
                        new SqlParameter("@Ma_hoc_vien", hoaDon.Ma_hoc_vien),
                        new SqlParameter("@Ma_lop", hoaDon.Ma_lop),
                        new SqlParameter("@Ma_hoa_don", hoaDon.Ma_hoa_don),
                        new SqlParameter("@Ngay_dang_ky", hoaDon.Ngay_tao)
                    };


                int rowsAffectedDangKyHoc = dataAccess.ExecuteNonQuery(dangKyHocQuery, dangKyHocParameters);
                    System.Diagnostics.Debug.WriteLine("B3");
                    if (rowsAffectedHoaDon > 0 && rowsAffectedDangKyHoc > 0)
                    {
                        return RedirectToAction("Manage_Invoice");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thêm hóa đơn hoặc đăng ký học.";
                        return View();
                    }
                /*}
                else
                {
                    System.Diagnostics.Debug.WriteLine("B4");
                    System.Diagnostics.Debug.WriteLine("trùng mã");
                    ViewBag.ErrorMessage = "Mã hóa đơn đã tồn tại.";
                    return View();
                }*/
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // Log thông điệp lỗi và mã lỗi chi tiết
                System.Diagnostics.Debug.WriteLine($"SQL Error {ex.Number}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("trùng mã do: ");
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thêm hóa đơn: " + ex.Message;
                return View();
            }
        }
        public ActionResult Edit_Invoice_Infor(string maHoaDon)
        {
            string query = "SELECT * FROM HoaDon WHERE Ma_hoa_don = @Ma_hoa_don";
            SqlParameter parameter = new SqlParameter("@Ma_hoa_don", maHoaDon);
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connStr);
            DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    HoaDon invoce = new HoaDon
                    {
                        Ma_hoa_don = row["Ma_hoa_don"].ToString(),
                        Ma_dang_ky = row["Ma_dang_ky"].ToString(),
                        Ngay_tao = (DateTime)row["Ngay_tao"],
                        Tong_tien = (decimal)row["Tong_tien"],
                        Trang_thai_thanh_toan = Convert.ToBoolean(row["Trang_thai_thanh_toan"])
                    };
                    return View(invoce);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy hóa đơn có mã: " + maHoaDon;
                    return RedirectToAction("Manage_Invoice");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Invoice");
            }
        }
        [HttpPost]
        public ActionResult Submit_Edit_Invoice_Infor(HoaDon hoaDon)
        {
            String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connectionString);
            try
            {
                string query = "UPDATE HoaDon " +
                               "SET Ma_dang_ky = @Ma_dang_ky, Ngay_tao = @Ngay_tao, " +
                               "Tong_tien = @Tong_tien, Trang_thai_thanh_toan = @Trang_thai_thanh_toan " +
                               "WHERE Ma_hoa_don = @Ma_hoa_don";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Ma_hoa_don", hoaDon.Ma_hoa_don),
                    new SqlParameter("@Ma_dang_ky", hoaDon.Ma_dang_ky),
                    new SqlParameter("@Ngay_tao", hoaDon.Ngay_tao),
                    new SqlParameter("@Tong_tien", hoaDon.Tong_tien),
                    new SqlParameter("@Trang_thai_thanh_toan", hoaDon.Trang_thai_thanh_toan)
                };

                int rowsAffected = dataAccess.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    return RedirectToAction("Manage_Invoice");
                }
                else
                {
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thêm hóa đơn. ";
                    return View(hoaDon);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thêm hóa đơn: " + ex.Message;
                return View(hoaDon);
            }
        }
        public ActionResult Delete_Invoice(string maHoaDon)
        {
            System.Diagnostics.Debug.WriteLine("B1");
            String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connectionString);
            dbHelper = new DataHelper(connectionString);
            try
            {
                System.Diagnostics.Debug.WriteLine("B2");

                if (dbHelper.IsMaExistsInHoaDon(maHoaDon))
                {
                    System.Diagnostics.Debug.WriteLine("B3");
                    string deleteQuery = "DELETE FROM HoaDon WHERE Ma_hoa_don = @Ma_hoa_don"+ "DELETE FROM DangKyHoc WHERE Ma_hoa_don = @Ma_hoa_don";
                    SqlParameter parameter = new SqlParameter("@Ma_hoa_don", maHoaDon);

                    int rowsAffected = dataAccess.ExecuteNonQuery(deleteQuery, new SqlParameter[] { parameter });

                    if (rowsAffected > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("B4");

                        return RedirectToAction("Manage_Invoice");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi");

                        ViewBag.ErrorMessage = "Xoá hoá đơn không thành công.";
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("không có mã");

                    ViewBag.ErrorMessage = "Hoá đơn không tồn tại.";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("out");

                ViewBag.ErrorMessage = "Đã xảy ra lỗi: " + ex.Message;
            }
            return RedirectToAction("Manage_Invoice");
        }
        public ActionResult ExportToTxt_Invoice(string maHoaDon)
        {
            HoaDon invoice = GetInvoiceInfo(maHoaDon);

            if (invoice != null)
            {
                string status = invoice.Trang_thai_thanh_toan ? "Đã đóng tiền" : "Chưa đóng tiền";
                string formattedTongTien = invoice.Tong_tien.ToString() + " VND";
                string formattedNgayTao = invoice.Ngay_tao.ToString("dd/MM/yyyy");
                decimal totalAmount = Convert.ToDecimal(invoice.Tong_tien) + invoice.Hoc_phi;
                string content = $"Mã hóa đơn: {invoice.Ma_hoa_don}\r\n" +
                                 $"Mã đăng ký: {invoice.Ma_dang_ky}\r\n" +
                                 $"Mã lớp: {invoice.Ma_lop}\r\n" +
                                 $"Mã học viên: {invoice.Ma_hoc_vien}\r\n" +
                                 $"Tên học viên: {invoice.Ho_va_ten}\r\n" +
                                 $"Ngày sinh: {invoice.Ngay_sinh.ToString("dd/MM/yyyy")}\r\n" +
                                 $"Số điện thoại: {invoice.Sdt}\r\n" +
                                 $"Email: {invoice.Email}\r\n" +
                                 $"Địa chỉ: {invoice.Dia_chi}\r\n" +
                                 $"Ngày tạo: {formattedNgayTao}\r\n" +
                                 $"Học phí: {invoice.Hoc_phi} VND\r\n"+ 
                                 $"Tổng tiền: {totalAmount} VND\r\n" +
                                 $"Trạng thái thanh toán: {status}\r\n";

                byte[] contentBytes = Encoding.UTF8.GetBytes(content);

                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", $"attachment;filename=HoaDon_{invoice.Ma_hoa_don.Trim()}_Info.txt");

                Response.BinaryWrite(contentBytes);
                Response.Flush();
                Response.End();

                return new EmptyResult();
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin hoá đơn.";
                return RedirectToAction("Manage_Invoice");
            }
        }

        private HoaDon GetInvoiceInfo(string maHoaDon)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            DataAccess dataAccess = new DataAccess(connStr);

            string query = "SELECT HoaDon.*, DangKyHoc.Ma_dang_ky, DangKyHoc.Ma_lop, HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, KhoaHoc.Hoc_phi " +
                           "FROM HoaDon " +
                           "JOIN DangKyHoc ON HoaDon.Ma_dang_ky = DangKyHoc.Ma_dang_ky " +
                           "JOIN HocVien ON DangKyHoc.Ma_hoc_vien = HocVien.Ma_hoc_vien " +
                           "JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma " +
                           "JOIN LopHoc ON DangKyHoc.Ma_lop = LopHoc.Ma_lop " +
                           "JOIN KhoaHoc ON LopHoc.Ma_khoa_hoc = KhoaHoc.Ma_khoa_hoc " +
                           "WHERE HoaDon.Ma_hoa_don = @Ma_hoa_don";

            SqlParameter parameter = new SqlParameter("@Ma_hoa_don", maHoaDon);

            DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                HoaDon invoiceInfo = new HoaDon
                {
                    Ma_hoc_vien = row["Ma_hoc_vien"].ToString(),
                    Ma_hoa_don = row["Ma_hoa_don"].ToString(),
                    Ho_va_ten = row["Ho_va_ten"].ToString(),
                    Ngay_sinh = (DateTime)row["Ngay_sinh"],
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString(),
                    Dia_chi = row["Dia_chi"].ToString(),
                    Ma_dang_ky = row["Ma_dang_ky"].ToString(),
                    Ma_lop = row["Ma_lop"].ToString(), // Include Ma_lop
                    Ngay_tao = (DateTime)row["Ngay_tao"],
                    Trang_thai_thanh_toan = Convert.ToBoolean(row["Trang_thai_thanh_toan"]),
                    Hoc_phi = Convert.ToDecimal(row["Hoc_phi"]) // Include Hoc_phi
                };

                return invoiceInfo;
            }
            else
            {
                return null;
            }
        }



        // Thời khoá biểu

        public ActionResult Manage_Schedule()
        {
            List<LopHoc> ds = new List<LopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand("select Ten_lop_hoc,Hoc_vao_thu, Thoi_gian_bat_dau,Thoi_gian_ket_thuc,Phong_hoc,Ma_lop from LopHoc join ThoiGianBieu on LopHoc.Ma_thoi_gian_bieu=ThoiGianBieu.Ma_thoi_gian_bieu", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                while (dr.Read())
                {
                    LopHoc lop = new LopHoc
                    {
                        Ma_lop = dr["Ma_lop"].ToString(),
                        Ten_lop_hoc = dr["Ten_lop_hoc"].ToString(),
                        Hoc_vao_thu = dr["Hoc_vao_thu"].ToString(),
                        Thoi_gian_bat_dau = (TimeSpan)dr["Thoi_gian_bat_dau"],
                        Thoi_gian_ket_thuc = (TimeSpan)dr["Thoi_gian_ket_thuc"],
                        Phong_hoc = dr["Phong_hoc"].ToString()
                    };
                    ds.Add(lop);
                }
                return View(ds);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                return View("Index");
            }
        }
    }
}