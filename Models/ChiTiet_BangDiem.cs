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
    
    public partial class ChiTiet_BangDiem
    {
        public string Ma_bang_diem { get; set; }
        public int Diem_so { get; set; }
        public string Ma_hoc_vien { get; set; }
    
        public virtual BangDiem BangDiem { get; set; }
        public virtual HocVien HocVien { get; set; }
    }
}
