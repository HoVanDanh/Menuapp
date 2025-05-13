using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MenuAPI.Models
{
    [Table("nguyen_lieu")]
    public partial class NguyenLieu
    {
        public NguyenLieu()
        {
            NguyenLieuMonAns = new HashSet<NguyenLieuMonAn>();
            NhapNguyenLieus = new HashSet<NhapNguyenLieu>();
        }

        [Key]
        [Column("nguyen_lieu_id")]
        public int NguyenLieuId { get; set; }
        [Column("ten_nguyen_lieu")]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = null!;
        [Column("so_luong")]
        public int SoLuong { get; set; }
        [Column("don_vi")]
        [StringLength(50)]
        public string? DonVi { get; set; }

        [InverseProperty("NguyenLieu")]
        public virtual ICollection<NguyenLieuMonAn> NguyenLieuMonAns { get; set; }
        [InverseProperty("NguyenLieu")]
        public virtual ICollection<NhapNguyenLieu> NhapNguyenLieus { get; set; }
    }
}
