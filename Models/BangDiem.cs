//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BangDiem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BangDiem()
        {
            this.ChiTiet_BangDiem = new HashSet<ChiTiet_BangDiem>();
        }
    
        public string Ma_bang_diem { get; set; }
        public string Ma_lop { get; set; }
        public System.DateTime Ngay_lap_bang_diem { get; set; }
    
        public virtual LopHoc LopHoc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTiet_BangDiem> ChiTiet_BangDiem { get; set; }
    }
}
