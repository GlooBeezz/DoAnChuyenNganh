using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using web2.Areas.Admin.Data;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace web2.Models
{
    public class DataHelper
    {
        private string connectionString = "data source=GLOOMYBEEZZ\\MSSQLSERVER01;initial catalog=quanLyTrungTamDayDan;user id=DoAnChuyenNgang;password=Admin;MultipleActiveResultSets=True;";

        public DataHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Kiểm tra sự tồn tại của mã trong bảng NguoiDung
        public bool IsMaExistsInNguoiDung(string ma)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(Ma) FROM NguoiDung WHERE Ma = @Ma";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma", ma);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Kiểm tra sự tồn tại của mã trong bảng LopHoc
        public bool IsMaExistsInLopHoc(string ma)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(Ma_lop) FROM LopHoc WHERE Ma_lop = @Ma";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma", ma);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Kiểm tra sự tồn tại của mã trong bảng KhoaHoc
        public bool IsMaExistsInKhoaHoc(string ma)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(Ma_khoa_hoc) FROM KhoaHoc WHERE Ma_khoa_hoc = @Ma";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma", ma);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool IsMaExistsInHoaDon(string ma)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(Ma_hoa_don) FROM HoaDon WHERE Ma_hoa_don = @Ma";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Đảm bảo giá trị cho tham số @Ma
                    command.Parameters.AddWithValue("@Ma", ma);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool ContainsDigitOrSpecialChar(string input)
        {
            string pattern = @"[\d]";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public bool IsPhoneNumberIsValid(string sdt)
        {
            string pattern = @"^(?:\+84|0)[1-9]\d{8}$|^0[1-9]\d{8}$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(sdt))
            { return true; }
            else { return false; }
        }
        public bool IsUserIdIsValid(string ma)
        {
            if((ma.StartsWith("hv")||ma.StartsWith("gv"))&& ma.Length <= 10)
            {
                return true;
            }
            else { return false; }
        }
        public bool IsBirthDateBeforeToday(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (birthDate.Date < currentDate)
            {
                return true;
            }
            else {
                return false;
            }
                
        }
        public bool KiemTraLopHocTonTai(string maLop)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT
                    COUNT(*) AS TotalCount
                FROM
                    LopHoc
                LEFT JOIN
                    GiaoVien ON LopHoc.Ma_lop = GiaoVien.Ma_lop_giang_day
                LEFT JOIN
                    DangKyHoc ON LopHoc.Ma_lop = DangKyHoc.Ma_lop
                LEFT JOIN
                    HocVien ON DangKyHoc.Ma_hoc_vien = HocVien.Ma_hoc_vien
                WHERE
                    LopHoc.Ma_lop = @Ma_lop
                    AND (GiaoVien.Ma_giao_vien IS NOT NULL OR HocVien.Ma_hoc_vien IS NOT NULL);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma_lop", maLop);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int totalCount = Convert.ToInt32(reader["TotalCount"]);
                            return totalCount > 0;
                        }
                    }
                }
            }

            return false;
        }
        public bool CheckKhoaHocCoLopCon(string maKhoaHoc)
        {
            try
            {
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
                DataAccess dataAccess = new DataAccess(connStr);

                string query = "IF EXISTS ( " +
                               "    SELECT 1 " +
                               "    FROM LopHoc " +
                               "    WHERE Ma_khoa_hoc = @Ma_khoa_hoc " +
                               ") " +
                               "    SELECT 1 " +
                               "ELSE " +
                               "    SELECT 0;";

                SqlParameter parameter = new SqlParameter("@Ma_khoa_hoc", maKhoaHoc);

                int result = Convert.ToInt32(dataAccess.ExecuteScalar(query, new SqlParameter[] { parameter }));
                return result == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi xử lý truy vấn: {ex.Message}");
                return false;
            }
        }

    }
}