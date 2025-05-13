using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
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
        [Required(ErrorMessage = "Tên nhân viên không được để trống.")]
        [Column("ten_nhan_vien")]
        [StringLength(100)]
        public string TenNhanVien { get; set; } = null!;
        [Column("so_dien_thoai")]
        [StringLength(15)]
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa ký tự số.")]

        public string SoDienThoai { get; set; } = string.Empty;
        [Column("dia_chi")]
        [StringLength(255)]
        public string? DiaChi { get; set; }
        [Required(ErrorMessage = "Phải chọn loại nhân viên.")]
        [Column("loai_nhan_vien_id")]
        public int LoaiNhanVienId { get; set; }

        [ForeignKey("LoaiNhanVienId")]
        [InverseProperty("NhanViens")]

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;
        public virtual LoaiNhanVien LoaiNhanVien { get; set; } = null!;
        [InverseProperty("NhanVien")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
