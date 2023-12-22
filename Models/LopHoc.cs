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
    
    public partial class LopHoc:ThoiGianBieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopHoc()
        {
            this.BaiTaps = new HashSet<BaiTap>();
            this.BangDiems = new HashSet<BangDiem>();
            this.ChiTiet_TaiLieu = new HashSet<ChiTiet_TaiLieu>();
            this.DangKyHocs = new HashSet<DangKyHoc>();
            this.GiaoViens = new HashSet<GiaoVien>();
            this.SoTietDays = new HashSet<SoTietDay>();
            this.HocViens = new HashSet<HocVien>();
        }
    
        public string Ma_lop { get; set; }
        public string Ma_khoa_hoc { get; set; }
        public string Phong_hoc { get; set; }
        public int So_luong_hoc_vien { get; set; }
        public string Ten_lop_hoc { get; set; }
        public string Mo_ta { get; set; }
        public string Ma_thoi_gian_bieu { get; set; }
        public System.DateTime Ngay_bat_dau { get; set; }
        public System.DateTime Ngay_ket_thuc { get; set; }
        public int So_tiet_hoc { get; set; }
        public string Ngay_bat_dau_formatted=> Ngay_bat_dau.ToString("yyyy-MM-dd");
        public string Ngay_ket_thuc_formatted => Ngay_ket_thuc.ToString("yyyy-MM-dd");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiTap> BaiTaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BangDiem> BangDiems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTiet_TaiLieu> ChiTiet_TaiLieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyHoc> DangKyHocs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiaoVien> GiaoViens { get; set; }
        public virtual KhoaHoc KhoaHoc { get; set; }
        public virtual ThoiGianBieu ThoiGianBieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoTietDay> SoTietDays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HocVien> HocViens { get; set; }
    }
}