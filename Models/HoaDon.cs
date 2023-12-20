namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            DangKyHocs = new HashSet<DangKyHoc>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_hoa_don { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_dang_ky { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_tao { get; set; }

        [Column(TypeName = "money")]
        public decimal Tong_tien { get; set; }

        public bool Trang_thai_thanh_toan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKyHoc> DangKyHocs { get; set; }
    }
}
