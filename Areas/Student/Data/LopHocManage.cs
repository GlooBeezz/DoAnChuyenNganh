using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using web2.Models;
namespace web2.Areas.Student.Data
{
    public class LopHocManage : IEnumerable<LopHoc>
    {
        public List<LopHoc> dsLH { get; set; }
        public LopHocManage()
        {
            dsLH = new List<LopHoc>();
        }
        public List<LopHoc> LayDanhSachKhoaHoc()
        {
            return dsLH;
        }
        public IEnumerator<LopHoc> GetEnumerator()
        {
            return dsLH.GetEnumerator();
        }
        public void ThemKhoaHoc(LopHoc lopHoc)
        {
            dsLH.Add(lopHoc);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}