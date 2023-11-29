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
    public class HocVienManage : IEnumerable<HocVien>
    {
        public List<HocVien> dsHV { get; set; }

        public HocVienManage()
        {
            dsHV = new List<HocVien>();
        }

        // Thêm học viên vào danh sách
        public void ThemHocVien(HocVien hocVien)
        {
            dsHV.Add(hocVien);
        }

        // Lấy danh sách học viên
        public List<HocVien> LayDanhSachHocVien()
        {
            return dsHV;
        }
        public IEnumerator<HocVien> GetEnumerator()
        {
            return dsHV.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}