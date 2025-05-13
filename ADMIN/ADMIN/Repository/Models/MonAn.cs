using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Repository.Models
{
    [Table("mon_an")]
    public partial class MonAn
    {
        public MonAn()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        [Column("mon_an_id")]
        public int MonAnId { get; set; }

        [Column("ten_mon")]
        [StringLength(100)]
        [Required(ErrorMessage = "Tên món không được để trống.")]
        public string TenMon { get; set; } = null!;

        [Column("loai_mon_an_id")]
        [Required(ErrorMessage = "Loại món ăn không được để trống.")]
        public int LoaiMonAnId { get; set; }

        [Column("gia")]
        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá không hợp lệ.")]
        public int Gia { get; set; }

        [Column("img")]
        [StringLength(255)]
        public string Img { get; set; } = null!;

        [Required(ErrorMessage = "Trạng thái món ăn không được để trống.")]
        [Column("trang_thai_mon_an")]
        public bool? TrangThaiMonAn { get; set; }

        [ForeignKey("LoaiMonAnId")]
        [InverseProperty("MonAns")]
        public virtual LoaiMonAn LoaiMonAn { get; set; } = null!;

        [InverseProperty("MonAn")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        [NotMapped]
        public IFormFile imgFile { get; set; }

        public virtual List<NguyenLieuMonAn> NguyenLieuMonAns { get; set; }
    }
}
