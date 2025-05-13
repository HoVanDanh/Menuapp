using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("nhap_nguyen_lieu")]
    public partial class NhapNguyenLieu
    {
        [Key]
        [Column("nhap_nguyen_lieu_id")]
        public int NhapNguyenLieuId { get; set; }
        [Column("nguyen_lieu_id")]
        public int NguyenLieuId { get; set; }
        [Column("nha_cung_cap_id")]
        public int NhaCungCapId { get; set; }
        [Column("so_luong")]
        public int SoLuong { get; set; }
        [Column("ngay_nhap", TypeName = "datetime")]
        public DateTime NgayNhap { get; set; }

        [ForeignKey("NguyenLieuId")]
        [InverseProperty("NhapNguyenLieus")]
        public virtual NguyenLieu NguyenLieu { get; set; } = null!;
        [ForeignKey("NhaCungCapId")]
        [InverseProperty("NhapNguyenLieus")]
        public virtual NhaCungCap NhaCungCap { get; set; } = null!;
    }
}
