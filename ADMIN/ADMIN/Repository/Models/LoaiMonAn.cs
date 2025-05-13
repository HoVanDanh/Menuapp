using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("loai_mon_an")]
    public partial class LoaiMonAn
    {
        public LoaiMonAn()
        {
            MonAns = new HashSet<MonAn>();
        }

        [Key]
        [Column("loai_mon_an_id")]
        public int LoaiMonAnId { get; set; }
        [Column("ten_loai")]
        [StringLength(100)]
        public string TenLoai { get; set; } = null!;

        [InverseProperty("LoaiMonAn")]
        public virtual ICollection<MonAn> MonAns { get; set; }
    }
}
