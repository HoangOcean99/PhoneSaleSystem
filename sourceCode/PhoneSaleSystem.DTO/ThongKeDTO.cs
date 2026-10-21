using System;

namespace PhoneSaleSystem.DTO
{
    public class DoanhThuDTO
    {
        public DateTime Ngay { get; set; }
        public int SoHoaDon { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class SanPhamBanChayDTO
    {
        public int MaDT { get; set; }
        public string TenDT { get; set; } = string.Empty;
        public string? TenHang { get; set; }
        public int TongDaBan { get; set; }
        public decimal TongDoanhThu { get; set; }
    }

    public class TonKhoDTO
    {
        public int MaDT { get; set; }
        public string TenDT { get; set; } = string.Empty;
        public string? TenHang { get; set; }
        public string? RAM { get; set; }
        public string? BoNho { get; set; }
        public string? MauSac { get; set; }
        public decimal GiaBan { get; set; }
        public int TonKho { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
