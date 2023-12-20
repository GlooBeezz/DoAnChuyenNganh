namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LopHoc")]
    public partial class LopHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopHoc()
        {
            BaiTaps = new HashSet<BaiTap>();
            BangDiems = new HashSet<BangDiem>();
            ChiTiet_TaiLieu = new HashSet<ChiTiet_TaiLieu>();
            DangKyHocs = new HashSet<DangKyHoc>();
            GiaoViens = new HashSet<GiaoVien>();
            SoTietDays = new HashSet<SoTietDay>();
            HocViens = new HashSet<HocVien>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_khoa_hoc { get; set; }

        [Required]
        [StringLength(10)]
        public string Phong_hoc { get; set; }

        public int So_luong_hoc_vien { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_lop_hoc { get; set; }

        [StringLength(100)]
        public string Mo_ta { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_thoi_gian_bieu { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_bat_dau { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_ket_thuc { get; set; }

        public int So_tiet_hoc { get; set; }

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
