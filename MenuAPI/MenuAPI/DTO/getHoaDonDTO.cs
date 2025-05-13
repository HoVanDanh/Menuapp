namespace MenuAPI.DTO
{
    public class getHoaDonDTO
    {
        public int HoaDonId { get; set; }
        public int KhachHangId { get; set; }
        public string TenKhachHang { get; set; }     
        public int BanId { get; set; }                
        public DateTime NgayTao { get; set; }        
        public int TongTien { get; set; }             
        public List<ChiTietHoaDonDTO> ChiTietHoaDons { get; set; } 

        public class ChiTietHoaDonDTO
        {
            public int ChiTietHoaDonId { get; set; }
            public int MonAnId { get; set; }
            public string TenMonAn { get; set; }      
            public int SoLuong { get; set; }          
            public int Gia { get; set; }               
        }
    }
}
