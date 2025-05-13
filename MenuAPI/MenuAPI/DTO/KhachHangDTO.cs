namespace MenuAPI.DTO
{
    public class KhachHangDTO
    {
        public int KhachHangId { get; set; }
        public string TenKhachHang { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }
}
