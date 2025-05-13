using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("chi_tiet_hoa_don")]
    public partial class ChiTietHoaDon
    {
        [Key]
        [Column("chi_tiet_hoa_don_id")]
        public int ChiTietHoaDonId { get; set; }
        [Column("hoa_don_id")]
        public int HoaDonId { get; set; }
        [Column("mon_an_id")]
        public int MonAnId { get; set; }
        [Column("so_luong")]
        public int SoLuong { get; set; }
        [Column("gia")]
        public int Gia { get; set; }

        [ForeignKey("HoaDonId")]
        [InverseProperty("ChiTietHoaDons")]
        public virtual HoaDon HoaDon { get; set; } = null!;
        [ForeignKey("MonAnId")]
        [InverseProperty("ChiTietHoaDons")]
        public virtual MonAn MonAn { get; set; } = null!;
    }
}
