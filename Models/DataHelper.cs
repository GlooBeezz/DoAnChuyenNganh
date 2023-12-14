using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                string query = "SELECT COUNT(*) FROM LopHoc WHERE Ma_lop_hoc = @Ma";
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

    }
}