using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADMIN.Repository.Models
{
    [Table("ChietKhau")]
    public partial class ChietKhau
    {
        [Key]
        [Column("ChietKhauId")]
        public int ChietKhauId { get; set; }

        [Column("MaChietKhau")]
        [Required(ErrorMessage = "Mã chiết khấu không được để trống.")]
        [StringLength(50)]
        public string MaChietKhau { get; set; } = null!;

        [Column("GiaTriChietKhau")]
        [Required(ErrorMessage = "Giá trị chiết khấu không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị chiết khấu phải lớn hơn 0.")]
        public int GiaTriChietKhau { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column("NgayBatDau", TypeName = "date")]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        public DateTime NgayBatDau { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column("NgayKetThuc", TypeName = "date")]
        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]

        public DateTime NgayKetThuc { get; set; }

        [Column("SoLuong")]
        [Required(ErrorMessage = "Số lượng chiết khấu không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ.")]
        public int Soluong { get; set; }
    }
}
