using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MenuAPI.Models
{
    public partial class MenuDbContext : DbContext
    {
        public MenuDbContext()
        {
        }

        public MenuDbContext(DbContextOptions<MenuDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } =  null!;
        public virtual DbSet<Ban> Bans { get; set; } = null!;
        public virtual DbSet<ChiTietDatBan> ChiTietDatBans { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<LoaiMonAn> LoaiMonAns { get; set; } = null!;
        public virtual DbSet<LoaiNhanVien> LoaiNhanViens { get; set; } = null!;
        public virtual DbSet<MonAn> MonAns { get; set; } = null!;
        public virtual DbSet<NguyenLieu> NguyenLieus { get; set; } = null!;
        public virtual DbSet<NguyenLieuMonAn> NguyenLieuMonAns { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<NhapNguyenLieu> NhapNguyenLieus { get; set; } = null!;
        public virtual DbSet<ChietKhau> ChietKhaus { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=MenuDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasOne(d => d.NhanVien)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.NhanVienId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__account__nhan_vi__70DDC3D8");
            });

            modelBuilder.Entity<Ban>(entity =>
            {
                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasOne(d => d.HoaDon)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.HoaDonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__chi_tiet___hoa_d__7D439ABD");

                entity.HasOne(d => d.MonAn)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MonAnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__chi_tiet___mon_a__7E37BEF6");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Ban)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.BanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hoa_don__ban_id__797309D9");

                entity.HasOne(d => d.KhachHang)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.KhachHangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hoa_don__khach_h__787EE5A0");
            });

            modelBuilder.Entity<MonAn>(entity =>
            {
                entity.Property(e => e.TrangThaiMonAn).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.LoaiMonAn)
                    .WithMany(p => p.MonAns)
                    .HasForeignKey(d => d.LoaiMonAnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__mon_an__loai_mon__6A30C649");
            });

            modelBuilder.Entity<NguyenLieuMonAn>(entity =>
            {
                entity.HasOne(d => d.MonAn)
                    .WithMany(p => p.NguyenLieuMonAns)
                    .HasForeignKey(d => d.MonAnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nguyen_li__mon_a__160F4887");

                entity.HasOne(d => d.NguyenLieu)
                    .WithMany(p => p.NguyenLieuMonAns)
                    .HasForeignKey(d => d.NguyenLieuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nguyen_li__nguye__17036CC0");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasOne(d => d.LoaiNhanVien)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.LoaiNhanVienId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nhan_vien__loai___6E01572D");
            });

            modelBuilder.Entity<NhapNguyenLieu>(entity =>
            {
                entity.Property(e => e.NgayNhap).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NguyenLieu)
                    .WithMany(p => p.NhapNguyenLieus)
                    .HasForeignKey(d => d.NguyenLieuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nhap_nguy__nguye__1AD3FDA4");

                entity.HasOne(d => d.NhaCungCap)
                    .WithMany(p => p.NhapNguyenLieus)
                    .HasForeignKey(d => d.NhaCungCapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__nhap_nguy__nha_c__1CBC4616");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
