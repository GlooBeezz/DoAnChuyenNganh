namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SoTietDay")]
    public partial class SoTietDay
    {
        [Key]
        [StringLength(10)]
        public string Ma_so_tiet_day { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_giao_vien { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        public int So_tiet { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual LopHoc LopHoc { get; set; }
    }
}
