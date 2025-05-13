using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("hoa_don")]
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        [Column("hoa_don_id")]
        public int HoaDonId { get; set; }
        [Column("khach_hang_id")]
        public int KhachHangId { get; set; }
        [Column("ban_id")]
        public int BanId { get; set; }
        [Column("ngay_tao", TypeName = "datetime")]
        public DateTime NgayTao { get; set; }
        [Column("tong_tien")]
        public int TongTien { get; set; }

        [ForeignKey("BanId")]
        [InverseProperty("HoaDons")]
        public virtual Ban Ban { get; set; } = null!;
        [ForeignKey("KhachHangId")]
        [InverseProperty("HoaDons")]
        public virtual KhachHang KhachHang { get; set; } = null!;
        [InverseProperty("HoaDon")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
