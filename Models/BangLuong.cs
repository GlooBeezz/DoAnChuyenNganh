namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BangLuong")]
    public partial class BangLuong
    {
        [Key]
        [StringLength(10)]
        public string Ma_bang_luong { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_giao_vien { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten_bang_luon { get; set; }

        [Required]
        public string Noi_dung { get; set; }

        [Column(TypeName = "money")]
        public decimal Luong_co_bang { get; set; }

        [Column(TypeName = "date")]
        public DateTime Tu_ngay { get; set; }

        [Column(TypeName = "date")]
        public DateTime Den_ngay { get; set; }

        public int So_tiet { get; set; }

        [Column(TypeName = "money")]
        public decimal Tong_nhan { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }
    }
}
