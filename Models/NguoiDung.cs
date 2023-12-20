namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguoiDung")]
    public partial class NguoiDung
    {
        [Key]
        [StringLength(10)]
        public string Ma { get; set; }

        [Required]
        [StringLength(50)]
        public string Ho_va_ten { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_sinh { get; set; }

        [Required]
        [StringLength(10)]
        public string Sdt { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Dia_chi { get; set; }

        [Required]
        [StringLength(50)]
        public string Tai_khoan { get; set; }

        [Required]
        [StringLength(50)]
        public string Mat_khau { get; set; }

        public bool Phan_quyen { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual HocVien HocVien { get; set; }
    }
}
