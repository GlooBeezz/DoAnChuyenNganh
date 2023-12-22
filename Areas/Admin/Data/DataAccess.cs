using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace web2.Areas.Admin.Data
{
    public class DataAccess
    {
        private readonly string connectionString;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        public int CountStudents()
        {
            string query = "SELECT COUNT(*) FROM HocVien";
            return ExecuteScalar(query);
        }

        public int CountTeachers()
        {
            string query = "SELECT COUNT(*) FROM GiaoVien";
            return ExecuteScalar(query);
        }

        public int CountCourses()
        {
            string query = "SELECT COUNT(*) FROM KhoaHoc";
            return ExecuteScalar(query);
        }

        public int CountClasses()
        {
            string query = "SELECT COUNT(*) FROM LopHoc";
            return ExecuteScalar(query);
        }

        public int ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // ExecuteScalar trả về giá trị đầu tiên từ kết quả truy vấn
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}