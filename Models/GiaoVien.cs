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
    
    public partial class GiaoVien:NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiaoVien()
        {
            this.BaiTaps = new HashSet<BaiTap>();
            this.BangLuongs = new HashSet<BangLuong>();
            this.SoTietDays = new HashSet<SoTietDay>();
            this.TaiLieux = new HashSet<TaiLieu>();
        }
    
        public string Ma_giao_vien { get; set; }
        public string Chuyen_mon { get; set; }
        public decimal Luong_co_ban { get; set; }
        public string Ma_lop_giang_day { get; set; }
        public string Ma_bang_luong { get; set; }
        public string Ma_tai_lieu_tai_len { get; set; }
        public string NgaySinhFormatted
        {
            get { return Ngay_sinh.ToString("dd/MM/yyyy"); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiTap> BaiTaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BangLuong> BangLuongs { get; set; }
        public virtual LopHoc LopHoc { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoTietDay> SoTietDays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiLieu> TaiLieux { get; set; }
    }
}
