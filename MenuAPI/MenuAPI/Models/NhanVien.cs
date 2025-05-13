using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("nhan_vien")]
    public partial class NhanVien
    {
        public NhanVien()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [Column("nhan_vien_id")]
        public int NhanVienId { get; set; }
        [Column("ten_nhan_vien")]
        [StringLength(100)]
        public string TenNhanVien { get; set; } = null!;
        [Column("so_dien_thoai")]
        [StringLength(15)]
        public string? SoDienThoai { get; set; }
        [Column("dia_chi")]
        [StringLength(255)]
        public string? DiaChi { get; set; }
        [Column("loai_nhan_vien_id")]
        public int LoaiNhanVienId { get; set; }

        [ForeignKey("LoaiNhanVienId")]
        [InverseProperty("NhanViens")]
        public virtual LoaiNhanVien LoaiNhanVien { get; set; } = null!;
        [InverseProperty("NhanVien")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
