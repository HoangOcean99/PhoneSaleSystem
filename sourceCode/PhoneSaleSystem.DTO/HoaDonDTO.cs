using System;

namespace PhoneSaleSystem.DTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public int MaNV { get; set; }
        public string? TenNV { get; set; } // Hiển thị khi JOIN
        public int MaKH { get; set; }
        public string? TenKH { get; set; } // Hiển thị khi JOIN
        public string? SDTKH { get; set; } // Hiển thị khi JOIN
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; }

        public HoaDonDTO() { }

        public HoaDonDTO(int maHD, int maNV, int maKH, DateTime ngayLap, decimal tongTien)
        {
            MaHD = maHD;
            MaNV = maNV;
            MaKH = maKH;
            NgayLap = ngayLap;
            TongTien = tongTien;
        }
    }
}
