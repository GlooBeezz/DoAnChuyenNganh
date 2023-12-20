using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace web2.Models
{
    public partial class DBHieu : DbContext
    {
        public DBHieu()
            : base("name=DBHieu")
        {
        }

        public virtual DbSet<BaiTap> BaiTaps { get; set; }
        public virtual DbSet<BaiTapDaNop> BaiTapDaNops { get; set; }
        public virtual DbSet<BangDiem> BangDiems { get; set; }
        public virtual DbSet<BangLuong> BangLuongs { get; set; }
        public virtual DbSet<ChiTiet_BangDiem> ChiTiet_BangDiem { get; set; }
        public virtual DbSet<ChiTiet_TaiLieu> ChiTiet_TaiLieu { get; set; }
        public virtual DbSet<DangKyHoc> DangKyHocs { get; set; }
        public virtual DbSet<GiaoVien> GiaoViens { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<HocVien> HocViens { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }
        public virtual DbSet<LopHoc> LopHocs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<SoTietDay> SoTietDays { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiLieu> TaiLieux { get; set; }
        public virtual DbSet<ThoiGianBieu> ThoiGianBieux { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiTap>()
                .Property(e => e.Ma_bai_tap)
                .IsFixedLength();

            modelBuilder.Entity<BaiTap>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<BaiTap>()
                .Property(e => e.Ma_giao_vien)
                .IsFixedLength();

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.BaiTapDaNops)
                .WithRequired(e => e.BaiTap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BaiTapDaNop>()
                .Property(e => e.Ma_bai_nop)
                .IsFixedLength();

            modelBuilder.Entity<BaiTapDaNop>()
                .Property(e => e.Ma_bai_tap)
                .IsFixedLength();

            modelBuilder.Entity<BaiTapDaNop>()
                .Property(e => e.Ma_hoc_vien)
                .IsFixedLength();

            modelBuilder.Entity<BangDiem>()
                .Property(e => e.Ma_bang_diem)
                .IsFixedLength();

            modelBuilder.Entity<BangDiem>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<BangDiem>()
                .HasMany(e => e.ChiTiet_BangDiem)
                .WithRequired(e => e.BangDiem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BangLuong>()
                .Property(e => e.Ma_bang_luong)
                .IsFixedLength();

            modelBuilder.Entity<BangLuong>()
                .Property(e => e.Ma_giao_vien)
                .IsFixedLength();

            modelBuilder.Entity<BangLuong>()
                .Property(e => e.Luong_co_bang)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BangLuong>()
                .Property(e => e.Tong_nhan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTiet_BangDiem>()
                .Property(e => e.Ma_bang_diem)
                .IsFixedLength();

            modelBuilder.Entity<ChiTiet_BangDiem>()
                .Property(e => e.Ma_hoc_vien)
                .IsFixedLength();

            modelBuilder.Entity<ChiTiet_TaiLieu>()
                .Property(e => e.Ma_tai_lieu_lop)
                .IsFixedLength();

            modelBuilder.Entity<ChiTiet_TaiLieu>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<ChiTiet_TaiLieu>()
                .Property(e => e.Ma_giao_vien)
                .IsFixedLength();

            modelBuilder.Entity<DangKyHoc>()
                .Property(e => e.Ma_dang_ky)
                .IsFixedLength();

            modelBuilder.Entity<DangKyHoc>()
                .Property(e => e.Ma_hoc_vien)
                .IsFixedLength();

            modelBuilder.Entity<DangKyHoc>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<DangKyHoc>()
                .Property(e => e.Ma_hoa_don)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.Ma_giao_vien)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.Luong_co_ban)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.Ma_lop_giang_day)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.Ma_bang_luong)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.Ma_tai_lieu_tai_len)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.BaiTaps)
                .WithRequired(e => e.GiaoVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.BangLuongs)
                .WithRequired(e => e.GiaoVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiaoVien>()
                .HasOptional(e => e.NguoiDung)
                .WithRequired(e => e.GiaoVien);

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.SoTietDays)
                .WithRequired(e => e.GiaoVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.TaiLieux)
                .WithRequired(e => e.GiaoVien)
                .HasForeignKey(e => e.Ma_giang_vien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.Ma_hoa_don)
                .IsFixedLength();

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.Ma_dang_ky)
                .IsFixedLength();

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.Tong_tien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HoaDon>()
                .HasMany(e => e.DangKyHocs)
                .WithRequired(e => e.HoaDon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HocVien>()
                .Property(e => e.Ma_hoc_vien)
                .IsFixedLength();

            modelBuilder.Entity<HocVien>()
                .Property(e => e.Lop_hoc_tham_gia)
                .IsFixedLength();

            modelBuilder.Entity<HocVien>()
                .HasMany(e => e.BaiTapDaNops)
                .WithRequired(e => e.HocVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HocVien>()
                .HasMany(e => e.ChiTiet_BangDiem)
                .WithRequired(e => e.HocVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HocVien>()
                .HasMany(e => e.DangKyHocs)
                .WithRequired(e => e.HocVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HocVien>()
                .HasOptional(e => e.NguoiDung)
                .WithRequired(e => e.HocVien);

            modelBuilder.Entity<HocVien>()
                .HasMany(e => e.TaiLieux)
                .WithOptional(e => e.HocVien)
                .HasForeignKey(e => e.Hoc_vien_duoc_xem);

            modelBuilder.Entity<HocVien>()
                .HasMany(e => e.LopHocs)
                .WithMany(e => e.HocViens)
                .Map(m => m.ToTable("DanhSachHocVien").MapLeftKey("Ma_hoc_vien").MapRightKey("Ma_lop"));

            modelBuilder.Entity<KhoaHoc>()
                .Property(e => e.Ma_khoa_hoc)
                .IsFixedLength();

            modelBuilder.Entity<KhoaHoc>()
                .Property(e => e.Hoc_phi)
                .HasPrecision(19, 4);

            modelBuilder.Entity<KhoaHoc>()
                .HasMany(e => e.LopHocs)
                .WithRequired(e => e.KhoaHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<LopHoc>()
                .Property(e => e.Ma_khoa_hoc)
                .IsFixedLength();

            modelBuilder.Entity<LopHoc>()
                .Property(e => e.Phong_hoc)
                .IsFixedLength();

            modelBuilder.Entity<LopHoc>()
                .Property(e => e.Ma_thoi_gian_bieu)
                .IsFixedLength();

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.BaiTaps)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.BangDiems)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.ChiTiet_TaiLieu)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.DangKyHocs)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.GiaoViens)
                .WithRequired(e => e.LopHoc)
                .HasForeignKey(e => e.Ma_lop_giang_day)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.SoTietDays)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.Ma)
                .IsFixedLength();

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.Sdt)
                .IsFixedLength();

            modelBuilder.Entity<SoTietDay>()
                .Property(e => e.Ma_so_tiet_day)
                .IsFixedLength();

            modelBuilder.Entity<SoTietDay>()
                .Property(e => e.Ma_giao_vien)
                .IsFixedLength();

            modelBuilder.Entity<SoTietDay>()
                .Property(e => e.Ma_lop)
                .IsFixedLength();

            modelBuilder.Entity<TaiLieu>()
                .Property(e => e.Ma_tai_lieu)
                .IsFixedLength();

            modelBuilder.Entity<TaiLieu>()
                .Property(e => e.Duong_dan)
                .IsUnicode(false);

            modelBuilder.Entity<TaiLieu>()
                .Property(e => e.Ma_giang_vien)
                .IsFixedLength();

            modelBuilder.Entity<TaiLieu>()
                .Property(e => e.Hoc_vien_duoc_xem)
                .IsFixedLength();

            modelBuilder.Entity<TaiLieu>()
                .HasMany(e => e.ChiTiet_TaiLieu)
                .WithRequired(e => e.TaiLieu)
                .HasForeignKey(e => e.Ma_tai_lieu_lop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ThoiGianBieu>()
                .Property(e => e.Ma_thoi_gian_bieu)
                .IsFixedLength();

            modelBuilder.Entity<ThoiGianBieu>()
                .HasMany(e => e.LopHocs)
                .WithRequired(e => e.ThoiGianBieu)
                .WillCascadeOnDelete(false);
        }
    }
}
