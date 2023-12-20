namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BangDiem")]
    public partial class BangDiem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BangDiem()
        {
            ChiTiet_BangDiem = new HashSet<ChiTiet_BangDiem>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_bang_diem { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_lop { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngay_lap_bang_diem { get; set; }

        public virtual LopHoc LopHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTiet_BangDiem> ChiTiet_BangDiem { get; set; }
    }
}
