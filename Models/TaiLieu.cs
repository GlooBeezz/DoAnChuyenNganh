namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiLieu")]
    public partial class TaiLieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiLieu()
        {
            ChiTiet_TaiLieu = new HashSet<ChiTiet_TaiLieu>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_tai_lieu { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_tai_lieu { get; set; }

        [Required]
        public string Duong_dan { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_upload { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_giang_vien { get; set; }

        [StringLength(10)]
        public string Hoc_vien_duoc_xem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTiet_TaiLieu> ChiTiet_TaiLieu { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual HocVien HocVien { get; set; }
    }
}
