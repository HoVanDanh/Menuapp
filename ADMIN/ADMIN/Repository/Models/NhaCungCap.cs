using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("nha_cung_cap")]
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            NhapNguyenLieus = new HashSet<NhapNguyenLieu>();
        }

        [Key]
        [Column("nha_cung_cap_id")]
        public int NhaCungCapId { get; set; }

        [Column("ten_nha_cung_cap")]
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
        [StringLength(100)]
        public string TenNhaCungCap { get; set; } = null!;

        [Column("dia_chi")]
        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [StringLength(255)]

        public string DiaChi { get; set; } = null!;

        [Column("so_dien_thoai")]
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15)]

        public string? SoDienThoai { get; set; }

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        [InverseProperty("NhaCungCap")]
        public virtual ICollection<NhapNguyenLieu> NhapNguyenLieus { get; set; }
    }
}
