using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("chi_tiet_dat_ban")]
    public partial class ChiTietDatBan
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("ban_id")]
        public int BanId { get; set; }
        [Column("mon_an_id")]
        public int MonAnId { get; set; }
        [Column("so_luong")]
        public int SoLuong { get; set; }
        [Column("gia")]
        public int Gia { get; set; }
        [Column("thoi_gian_tao")]
        public DateTime ThoiGianTao { get; set; }
        [Column("trang_thai")] 
        public bool TrangThai { get; set; }
    }
}
