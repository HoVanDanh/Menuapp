namespace MenuAPI.DTO
{
    public class NhanVienDTO
    {
        public int NhanVienId { get; set; } 
        public string TenNhanVien { get; set; } = null!; 
        public string? SoDienThoai { get; set; } 
        public string? DiaChi { get; set; } 
        public int LoaiNhanVienId { get; set; } 

    }
}
