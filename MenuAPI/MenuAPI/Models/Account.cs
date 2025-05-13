using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("account")]
    public partial class Account
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }
        [Column("nhan_vien_id")]
        public int NhanVienId { get; set; }
        [Column("ten_dang_nhap")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = null!;
        [Column("mat_khau")]
        [StringLength(255)]
        public string MatKhau { get; set; } = null!;
        [Column("phan_quyen")]
        public int PhanQuyen { get; set; }

        [ForeignKey("NhanVienId")]
        [InverseProperty("Accounts")]
        public virtual NhanVien NhanVien { get; set; } = null!;
    }
}
