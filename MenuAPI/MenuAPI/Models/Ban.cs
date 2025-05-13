using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("ban")]
    public partial class Ban
    {
        public Ban()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        [Key]
        [Column("ban_id")]
        public int BanId { get; set; }
        [Column("ten_ban")]
        [StringLength(50)]
        public string TenBan { get; set; } = null!;
        [Column("trang_thai")]
        public bool TrangThai { get; set; }
        [Column("isDelete")]
        public bool? IsDelete { get; set; }

        [InverseProperty("Ban")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
