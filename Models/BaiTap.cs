namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiTap")]
    public partial class BaiTap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiTap()
        {
            BaiTapDaNops = new HashSet<BaiTapDaNop>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_bai_tap { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_bai_tap { get; set; }

        [Required]
        [StringLength(200)]
        public string Noi_dung { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_giao { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_het_han { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_giao_vien { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaiTapDaNop> BaiTapDaNops { get; set; }
    }
}
