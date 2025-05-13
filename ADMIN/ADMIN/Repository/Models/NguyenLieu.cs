using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
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
        [Required(ErrorMessage = "Tên nguyên liệu không được để trống.")]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = null!;
        [Column("so_luong")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Required(ErrorMessage = "Số lượng không được để trống.")]
        public int SoLuong { get; set; }
        [Column("don_vi")]
        [Required(ErrorMessage = "Đơn vị không được để trống.")]
        [StringLength(50)]
        public string? DonVi { get; set; }

        [InverseProperty("NguyenLieu")]
        public virtual ICollection<NguyenLieuMonAn> NguyenLieuMonAns { get; set; }
        [InverseProperty("NguyenLieu")]
         public ICollection<NhapNguyenLieu> NhapNguyenLieus { get; set; } = new HashSet<NhapNguyenLieu>();
    }
}
