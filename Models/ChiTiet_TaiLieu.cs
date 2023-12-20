namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChiTiet_TaiLieu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Ma_tai_lieu_lop { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Ma_giao_vien { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        public virtual TaiLieu TaiLieu { get; set; }
    }
}
