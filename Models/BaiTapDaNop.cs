//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BaiTapDaNop
    {
        public string Ma_bai_nop { get; set; }
        public string Ma_bai_tap { get; set; }
        public string Ma_hoc_vien { get; set; }
        public string File_bai_nop { get; set; }
        public string Nhan_xet { get; set; }
        public double Diem { get; set; }
    
        public virtual BaiTap BaiTap { get; set; }
        public virtual HocVien HocVien { get; set; }
    }
}
