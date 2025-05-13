using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("nguyen_lieu_mon_an")]
    public partial class NguyenLieuMonAn
    {
        [Key]
        [Column("nguyen_lieu_mon_an_id")]
        public int NguyenLieuMonAnId { get; set; }
        [Column("mon_an_id")]
        public int MonAnId { get; set; }
        [Column("nguyen_lieu_id")]
        public int NguyenLieuId { get; set; }
        [Column("so_luong")]
        public int SoLuong { get; set; }

        [ForeignKey("MonAnId")]
        [InverseProperty("NguyenLieuMonAns")]
        public virtual MonAn MonAn { get; set; } = null!;
        [ForeignKey("NguyenLieuId")]
        [InverseProperty("NguyenLieuMonAns")]
        public virtual NguyenLieu NguyenLieu { get; set; } = null!;
    }
}
