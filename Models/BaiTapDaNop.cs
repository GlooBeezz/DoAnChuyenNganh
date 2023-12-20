namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaiTapDaNop")]
    public partial class BaiTapDaNop
    {
        [Key]
        [StringLength(10)]
        public string Ma_bai_nop { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_bai_tap { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_hoc_vien { get; set; }

        [Required]
        public string File_bai_nop { get; set; }

        [Required]
        public string Nhan_xet { get; set; }

        public double Diem { get; set; }

        public virtual BaiTap BaiTap { get; set; }

        public virtual HocVien HocVien { get; set; }
    }
}
