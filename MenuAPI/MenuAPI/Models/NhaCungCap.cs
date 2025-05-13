using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
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
        [StringLength(100)]
        public string TenNhaCungCap { get; set; } = null!;
        [Column("dia_chi")]
        [StringLength(255)]
        public string DiaChi { get; set; } = null!;
        [Column("so_dien_thoai")]
        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [InverseProperty("NhaCungCap")]
        public virtual ICollection<NhapNguyenLieu> NhapNguyenLieus { get; set; }
    }
}
