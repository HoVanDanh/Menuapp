using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("loai_nhan_vien")]
    public partial class LoaiNhanVien
    {
        public LoaiNhanVien()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        [Key]
        [Column("loai_nhan_vien_id")]
        public int LoaiNhanVienId { get; set; }
        [Column("ten_loai")]
        [StringLength(100)]
        public string TenLoai { get; set; } = null!;

        [InverseProperty("LoaiNhanVien")]
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
