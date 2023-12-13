using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web2.Models;

namespace web2.Areas.Admin.Data
{
    public class ThongBaoModel
    {
        public string NguoiNhan { get; set; }
        public string Tieude { get; set; }
        public string Noidung { get; set; }
        public string maKhoaInput { get; set; }
        public string maLopInput { get; set; }
        public bool NguoiNhan_GiaoVien { get; set; }
        public bool NguoiNhan_HocVien { get; set; }
        public List<KhoaHoc> KhoaHocs { get; set; }
        public List<LopHoc> LopHocs { get; set; }
    }
}