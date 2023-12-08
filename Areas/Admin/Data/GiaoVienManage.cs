using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using web2.Models;

namespace web2.Areas.Admin.Data
{
    public class GiaoVienManage : IEnumerable<GiaoVien>
    {
        public List<GiaoVien> dsGV { get; set; }

        public GiaoVienManage()
        {
            dsGV = new List<GiaoVien>();
        }

        // Thêm học viên vào danh sách
        public void ThemHocVien(GiaoVien giaoVien)
        {
            dsGV.Add(giaoVien);
        }

        // Lấy danh sách học viên
        public List<GiaoVien> LayDanhSachHocVien()
        {
            return dsGV;
        }
        public IEnumerator<GiaoVien> GetEnumerator()
        {
            return dsGV.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}