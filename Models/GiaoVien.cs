namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiaoVien")]
    public partial class GiaoVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiaoVien()
        {
            BaiTaps = new HashSet<BaiTap>();
            BangLuongs = new HashSet<BangLuong>();
            SoTietDays = new HashSet<SoTietDay>();
            TaiLieux = new HashSet<TaiLieu>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_giao_vien { get; set; }

        [Required]
        [StringLength(50)]
        public string Chuyen_mon { get; set; }

        [Column(TypeName = "money")]
        public decimal Luong_co_ban { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_lop_giang_day { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_bang_luong { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_tai_lieu_tai_len { get; set; }

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
