namespace MenuAPI.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public int PhanQuyen { get; set; }
        public NhanVienDTO NhanVien { get; set; } = null!;

    }
}
