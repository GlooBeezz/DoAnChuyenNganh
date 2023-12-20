namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKyHoc")]
    public partial class DangKyHoc
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Ma_dang_ky { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Ma_hoc_vien { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string Ma_hoa_don { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_dang_ky { get; set; }

        public virtual HoaDon HoaDon { get; set; }

        public virtual HocVien HocVien { get; set; }

        public virtual LopHoc LopHoc { get; set; }
    }
}
