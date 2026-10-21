namespace PhoneSaleSystem.DTO
{
    public class ChiTietHoaDonDTO
    {
        public int MaHD { get; set; }
        public int MaDT { get; set; }
        public string? TenDT { get; set; } // Hiển thị khi JOIN
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public ChiTietHoaDonDTO() { }

        public ChiTietHoaDonDTO(int maHD, int maDT, int soLuong, decimal donGia, decimal thanhTien)
        {
            MaHD = maHD;
            MaDT = maDT;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = thanhTien;
        }

        public ChiTietHoaDonDTO(int maDT, string tenDT, int soLuong, decimal donGia)
        {
            MaDT = maDT;
            TenDT = tenDT;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = soLuong * donGia;
        }
    }
}
