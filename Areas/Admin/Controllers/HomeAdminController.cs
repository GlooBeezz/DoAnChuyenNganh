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
            // Lấy danh sách học viên
            var dsHocVien= new DanhSachHocViencs().listHocVien;
            //   String connStr = "Data Source=GLOOMYBEEZZ\\MSSQLSERVER01;Initial Catalog=quanLyTrungTamDayDan;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0";
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                //"Data Source=GLOOMYBEEZZ\\MSSQLSERVER01;Initial Catalog=quanLyTrungTamDayDan;Persist Security Info=True;User ID=DoAnChuyenNgang;";
            
            SqlConnection conn=new SqlConnection(connStr);
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
            return View(dsHocVien);
        }
        public ActionResult Edit_Student_Infor(string maHocVien)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(maHocVien);
                System.Diagnostics.Debug.WriteLine("B1");
                string query = "SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi FROM HocVien JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma WHERE Ma_hoc_vien = @Ma_hoc_vien";
                SqlParameter parameter = new SqlParameter("@Ma_hoc_vien", maHocVien);
                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
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
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    return View("Edit_Student_Infor", hocVien);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B3");
                    TempData["ErrorMessage"] = "Không tìm thấy học viên";
                    return RedirectToAction("Manage_Student");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Student");
            }
        }
        [HttpPost]
        public ActionResult Submit_Edit_Student_Infor(string Ma, string Ten, DateTime NgaySinh,DateTime? NgaySinhNew, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ThemLop, string HocPhi)
        {
            try
            {
                string updateNguoiDungQuery = "UPDATE NguoiDung SET Ho_va_ten = @Ho_va_ten, Ngay_sinh = @Ngay_sinh, Sdt = @Sdt, Email = @Email, Dia_chi = @Dia_chi, Tai_khoan = @Tai_khoan, Mat_khau = @Mat_khau, Phan_quyen = @Phan_quyen WHERE Ma = @Ma_hoc_vien";
                string updateHocVienQuery = "UPDATE HocVien SET Lop_hoc_tham_gia = @Lop_hoc_tham_gia, Trang_thai_hoc_phi = @Trang_thai_hoc_phi WHERE Ma_hoc_vien = @Ma_hoc_vien";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@Ma_hoc_vien", Ma),
                    new SqlParameter("@Ho_va_ten", Ten),
                    new SqlParameter("@Ngay_sinh", NgaySinhNew != null ? NgaySinhNew:NgaySinh),
                    new SqlParameter("@Sdt", Sdt),
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@Dia_chi", DiaChi),
                    new SqlParameter("@Tai_khoan", TaiKhoan),
                    new SqlParameter("@Mat_khau", MatKhau),
                    new SqlParameter("@Phan_quyen", PhanQuyen),
                    new SqlParameter("@Lop_hoc_tham_gia", ThemLop != null ? string.Concat(Lop, ThemLop, " ") : Lop),
                    new SqlParameter("@Trang_thai_hoc_phi", HocPhi)
                };

                String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
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
        //public ActionResult Find_Student() {
        //    try
        //    {
        //        System.Diagnostics.Debug.WriteLine("B1");
        //        string maHocVien = Request.Form["maHocVienInput"];
        //        System.Diagnostics.Debug.WriteLine(maHocVien);
        //        var dsHocVien = new DanhSachHocViencs().listHocVien;
        //        string query = "SELECT HocVien.Ma_hoc_vien, NguoiDung.Ho_va_ten, NguoiDung.Ngay_sinh, NguoiDung.Sdt, NguoiDung.Email, NguoiDung.Dia_chi, NguoiDung.Tai_khoan, NguoiDung.Mat_khau, NguoiDung.Phan_quyen, HocVien.Lop_hoc_tham_gia, HocVien.Trang_thai_hoc_phi FROM HocVien JOIN NguoiDung ON HocVien.Ma_hoc_vien = NguoiDung.Ma WHERE Ma_hoc_vien = @Ma_hoc_vien";
        //        SqlParameter parameter = new SqlParameter("@Ma_hoc_vien", maHocVien);
        //        String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
        //        DataAccess dataAccess = new DataAccess(connStr);
        //        DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
        //        System.Diagnostics.Debug.WriteLine("B2");
        //        if (dataTable.Rows.Count > 0)
        //        {
        //            DataRow row = dataTable.Rows[0];
        //            System.Diagnostics.Debug.WriteLine("B2.1");
        //            HocVien hocVien = new HocVien
        //            {
        //                Ma_hoc_vien = row["Ma_hoc_vien"].ToString(),
        //                Ho_va_ten = row["Ho_va_ten"].ToString(),
        //                Ngay_sinh = (DateTime)row["Ngay_sinh"],
        //                Sdt = row["Sdt"].ToString(),
        //                Email = row["Email"].ToString(),
        //                Dia_chi = row["Dia_chi"].ToString(),
        //                Tai_khoan = row["Tai_khoan"].ToString(),
        //                Mat_khau = row["Mat_khau"].ToString(),
        //                Phan_quyen = (bool)row["Phan_quyen"],
        //                Lop_hoc_tham_gia = row["Lop_hoc_tham_gia"].ToString(),
        //                Trang_thai_hoc_phi = (bool)row["Trang_thai_hoc_phi"]
        //            };
        //            dsHocVien.Add(hocVien);
        //            System.Diagnostics.Debug.WriteLine("B2.2");
        //            return View("Manage_Student", hocVien);
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("B3");
        //            TempData["ErrorMessage"] = "Không tìm thấy học viên";
        //            return RedirectToAction("Manage_Student");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("B4");
        //        TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
        //        return RedirectToAction("Manage_Student");
        //    }

        //}
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
                DataAccess dataAccess = new DataAccess(connStr);
                DataTable dataTable = dataAccess.ExecuteQuery(query, new SqlParameter[] { parameter });
                System.Diagnostics.Debug.WriteLine("B2");
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
                    dsHocVien.Add(hocVien);
                    System.Diagnostics.Debug.WriteLine("B2.2");
                    return View("Manage_Student", dsHocVien);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("B3");
                    TempData["ErrorMessage"] = "Không tìm thấy học viên";
                    return RedirectToAction("Manage_Student");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("B4");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Manage_Student");
            }
        }


        public ActionResult Manage_Teacher()
        {
            return View();
        }
        public ActionResult Add_Student()
        {
            return View();
        }
        //public ActionResult Submit_Add_Student(string Ma, string Ten, DateTime NgaySinh, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ThemLop, string HocPhi)
        //{
        //    bool parsePhanQuyen = bool.Parse(PhanQuyen);
        //    bool parseHocPhi = bool.Parse(HocPhi);

        //    String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        System.Diagnostics.Debug.WriteLine("B1");
        //        conn.Open();
        //        // Bắt đầu giao dịch
        //        System.Diagnostics.Debug.WriteLine("B2");
        //        SqlTransaction transaction = conn.BeginTransaction();
        //        try
        //        {
        //            System.Diagnostics.Debug.WriteLine("B2.1");
        //            // Thêm vào bảng NguoiDung
        //            using (SqlCommand insertCmdNguoiDung = new SqlCommand("INSERT INTO NguoiDung (Ma, Ho_va_ten, Ngay_sinh, Sdt, Email, Dia_chi, Tai_khoan, Mat_khau, Phan_quyen) " +
        //                            "VALUES (@Ma, @Ho_va_ten, @Ngay_sinh, @Sdt, @Email, @Dia_chi, @Tai_khoan, @Mat_khau, @Phan_quyen);", conn, transaction))
        //            {
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Ma", Ma);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Ho_va_ten", Ten);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Ngay_sinh", NgaySinh);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Sdt", Sdt);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Email", Email);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Dia_chi", DiaChi);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Tai_khoan", TaiKhoan);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Mat_khau", MatKhau);
        //                insertCmdNguoiDung.Parameters.AddWithValue("@Phan_quyen", parsePhanQuyen);
        //                insertCmdNguoiDung.ExecuteNonQuery();
        //            }
        //            System.Diagnostics.Debug.WriteLine("B2.2");
        //            // Thêm vào bảng HocVien
        //            using (SqlCommand insertCmdHocVien = new SqlCommand("INSERT INTO HocVien (Ma_hoc_vien, Lop_hoc_tham_gia, Trang_thai_hoc_phi) " +
        //                            "VALUES (@Ma_hoc_vien, @Lop_hoc_tham_gia, @Trang_thai_hoc_phi);", conn, transaction))
        //            {
        //                insertCmdHocVien.Parameters.AddWithValue("@Ma_hoc_vien", Ma);
        //                insertCmdHocVien.Parameters.AddWithValue("@Lop_hoc_tham_gia", ThemLop != null ? string.Concat(Lop, ThemLop, " ") : Lop);
        //                insertCmdHocVien.Parameters.AddWithValue("@Trang_thai_hoc_phi", parseHocPhi);
        //                insertCmdHocVien.ExecuteNonQuery();
        //            }
        //            System.Diagnostics.Debug.WriteLine("B3");
        //            transaction.Commit();
        //            TempData["SuccessMessage"] = "Thêm học viên thành công";
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("B4");
        //            transaction.Rollback();
        //            TempData["ErrorMessage"] = "Thêm học viên không thành công. Lỗi: " + ex.Message;
        //        }
        //        finally
        //        {
        //            System.Diagnostics.Debug.WriteLine("B5");
        //            conn.Close();
        //        }
        //    }
        //    System.Diagnostics.Debug.WriteLine("B6");
        //    return RedirectToAction("Manage_Student");
        //}
        public ActionResult Submit_Add_Student(string Ma, string Ten, DateTime NgaySinh, string Sdt, string Email, string DiaChi, string TaiKhoan, string MatKhau, string PhanQuyen, string Lop, string ThemLop, string HocPhi)
        {
            //Thêm vào db

            bool parsePhanQuyen = bool.Parse(PhanQuyen);
            bool parseHocPhi = bool.Parse(HocPhi);
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
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
                    string sql = "DELETE FROM HocVien WHERE Ma_hoc_vien = @Ma_hoc_vien";
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

    }
}