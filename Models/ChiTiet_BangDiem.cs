namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChiTiet_BangDiem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Ma_bang_diem { get; set; }

        public int Diem_so { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Ma_hoc_vien { get; set; }

        public virtual BangDiem BangDiem { get; set; }

        public virtual HocVien HocVien { get; set; }
    }
}
