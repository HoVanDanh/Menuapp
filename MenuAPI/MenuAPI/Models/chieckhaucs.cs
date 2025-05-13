using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPI.Models
{
    [Table("ChietKhau")]
    public partial class ChietKhau
    {
        [Key]
        [Column("ChietKhauId")]
        public int ChietKhauId { get; set; }

        [Column("MaChietKhau")]
        [StringLength(50)] 
        public string MaChietKhau { get; set; } = null!;

        [Column("GiaTriChietKhau")]
        public int GiaTriChietKhau { get; set; }

        [Column("NgayBatDau")]
        public DateTime? NgayBatDau { get; set; }

        [Column("NgayKetThuc")]
        public DateTime? NgayKetThuc { get; set; }
        [Column("SoLuong")]
        public int SoLuong { get; set; }

    }
}