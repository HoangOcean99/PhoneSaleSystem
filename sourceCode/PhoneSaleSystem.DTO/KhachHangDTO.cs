namespace PhoneSaleSystem.DTO
{
    public class KhachHangDTO
    {
        public int MaKH { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public string? DiaChi { get; set; }

        public KhachHangDTO() { }

        public KhachHangDTO(int maKH, string hoTen, string sdt, string? diaChi)
        {
            MaKH = maKH;
            HoTen = hoTen;
            SDT = sdt;
            DiaChi = diaChi;
        }
    }
}
