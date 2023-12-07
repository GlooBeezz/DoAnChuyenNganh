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
    public class KhoaHocManage :IEnumerable<KhoaHoc>
    {
        public List<KhoaHoc> dsKH { get; set; }
        public KhoaHocManage()
        {
            dsKH = new List<KhoaHoc>();
        }
        public List<KhoaHoc> LayDanhSachKhoaHoc()
        {
            return dsKH;
        }
        public IEnumerator<KhoaHoc> GetEnumerator()
        {
            return dsKH.GetEnumerator();
        }
        public void ThemKhoaHoc(KhoaHoc khoaHoc)
        {
            dsKH.Add(khoaHoc);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}