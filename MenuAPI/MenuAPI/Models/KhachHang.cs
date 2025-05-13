using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("khach_hang")]
    public partial class KhachHang
    {
        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        [Key]
        [Column("khach_hang_id")]
        public int KhachHangId { get; set; }
        [Column("ten_khach_hang")]
        [StringLength(100)]
        public string TenKhachHang { get; set; } = null!;
        [Column("so_dien_thoai")]
        [StringLength(15)]
        public string? SoDienThoai { get; set; }
        [Column("dia_chi")]
        [StringLength(255)]
        public string? DiaChi { get; set; }

        [InverseProperty("KhachHang")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
