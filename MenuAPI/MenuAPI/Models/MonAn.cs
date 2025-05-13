using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("mon_an")]
    public partial class MonAn
    {
        public MonAn()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
            NguyenLieuMonAns = new HashSet<NguyenLieuMonAn>();
        }

        [Key]
        [Column("mon_an_id")]
        public int MonAnId { get; set; }
        [Column("ten_mon")]
        [StringLength(100)]
        public string TenMon { get; set; } = null!;
        [Column("loai_mon_an_id")]
        public int LoaiMonAnId { get; set; }
        [Column("gia")]
        public int Gia { get; set; }
        [Column("img")]
        [StringLength(255)]
        public string? Img { get; set; }
        [Required]
        [Column("trang_thai_mon_an")]
        public bool? TrangThaiMonAn { get; set; }

        [ForeignKey("LoaiMonAnId")]
        [InverseProperty("MonAns")]
        public virtual LoaiMonAn LoaiMonAn { get; set; } = null!;
        [InverseProperty("MonAn")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        [InverseProperty("MonAn")]
        public virtual ICollection<NguyenLieuMonAn> NguyenLieuMonAns { get; set; }
    }
}
