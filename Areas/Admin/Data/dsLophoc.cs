using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web2.Areas.Admin.Data
{
    public class dsLophoc
    {
        public dsLophoc() { }

        public DateTime Ngay_bat_dau { get; set; }
        public DateTime Ngay_ket_thuc { get; set; }
        public String Ten_Lop_hoc { get; set; }
        public Decimal Hoc_phi { get; set; }
        public TimeSpan Thoi_gian_bat_dau { get; set; }
        public TimeSpan Thoi_gian_ket_thuc { get; set; }
        public String Hoc_vao_thu { get; set; }



    }
}