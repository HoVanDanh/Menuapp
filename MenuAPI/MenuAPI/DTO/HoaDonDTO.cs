namespace MenuAPI.DTO
{
    public class HoaDonDTO
    {
        public int HoaDonId { get; set; }
        public int KhachHangId { get; set; }
        public string? TenKhachHanh { get; set; }
        public int BanId { get; set; }
        public DateTime NgayTao { get; set; }
        public int TongTien { get; set; }
        public List<ChiTietHoaDonDTO> ChiTietHoaDons { get; set; }

    }
}
