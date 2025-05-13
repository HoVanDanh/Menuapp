using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADMIN.Repository.Models
{
    [Table("account")]
    public partial class Account
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }

        [Column("nhan_vien_id")]
        public int NhanVienId { get; set; }

        [Column("ten_dang_nhap")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = null!;

        [Column("mat_khau")]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(255)]
        public string MatKhau { get; set; } = null!;

        [Column("phan_quyen")]
        public int PhanQuyen { get; set; }

        [ForeignKey("NhanVienId")]
        public virtual NhanVien NhanVien { get; set; } = null!;
    }
}
