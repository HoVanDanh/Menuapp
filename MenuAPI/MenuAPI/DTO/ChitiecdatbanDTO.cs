using MenuAPI.Models;

namespace MenuAPI.DTO
{
    public class ChitiecdatbanDTO
    {
        public int Id { get; set; }               // ID của chi tiết đặt bàn
        public int BanId { get; set; }            // ID của bàn đặt
        public int MonAnId { get; set; }          // ID của món ăn được đặt
        public string TenMonAn { get; set; }      // Tên của món ăn
        public int SoLuong { get; set; }          // Số lượng món ăn
        public int Gia { get; set; }
        public virtual MonAn MonAn { get; set; }
        public int TimeConLai { get; set; }
        public bool TrangThai { get; set; }
    }
}
