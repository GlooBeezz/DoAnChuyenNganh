using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

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
                string query = "SELECT COUNT(*) FROM NguoiDung WHERE Ma = @Ma";
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
                string query = "SELECT COUNT(*) FROM LopHoc WHERE Ma_lop = @Ma";
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
                string query = "SELECT COUNT(*) FROM KhoaHoc WHERE Ma_khoa_hoc = @Ma";
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
                string query = "SELECT COUNT(*) FROM HoaDon WHERE Ma_hoa_don = @Ma";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma", ma);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool IsNameContainNumber(string ten)
        {
            if (ten.Any(char.IsDigit))
            { return false; }
            else { return true; }
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
    }
}