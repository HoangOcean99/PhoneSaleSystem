namespace PhoneSaleSystem.DTO
{
    public class DienThoaiDTO
    {
        public int MaDT { get; set; }
        public string TenDT { get; set; } = string.Empty;
        public int MaHang { get; set; }
        public string? TenHang { get; set; } // Hiển thị tên hãng khi JOIN
        public string? RAM { get; set; }
        public string? BoNho { get; set; }
        public string? MauSac { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public int BaoHanh { get; set; } = 12;

        public DienThoaiDTO() { }

        public DienThoaiDTO(int maDT, string tenDT, int maHang, string? ram, string? boNho, 
            string? mauSac, decimal giaNhap, decimal giaBan, int soLuong, int baoHanh = 12)
        {
            MaDT = maDT;
            TenDT = tenDT;
            MaHang = maHang;
            RAM = ram;
            BoNho = boNho;
            MauSac = mauSac;
            GiaNhap = giaNhap;
            GiaBan = giaBan;
            SoLuong = soLuong;
            BaoHanh = baoHanh;
        }

        public override string ToString()
        {
            return $"{TenDT} ({RAM}/{BoNho} - {MauSac}) - {GiaBan:N0} đ [Còn: {SoLuong}]";
        }
    }
}
