using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web2.Models;

namespace web2.Areas.Admin.Data
{
    public class NguoiDungManage:IEnumerable<NguoiDung>
    {
        public List<NguoiDung> dsnd {  get; set; }

        public NguoiDungManage() 
        { 
        dsnd = new List<NguoiDung>();
        }

        public List<NguoiDung> LayDanhSachHocVien()
        {
            return dsnd;
        }

        public IEnumerator<NguoiDung> GetEnumerator()
        {
            return dsnd.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }


}