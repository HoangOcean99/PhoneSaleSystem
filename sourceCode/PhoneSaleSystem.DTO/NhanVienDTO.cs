namespace PhoneSaleSystem.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public string? ChucVu { get; set; }
        public int? MaTK { get; set; }

        public NhanVienDTO() { }

        public NhanVienDTO(int maNV, string hoTen, string? sdt, string? diaChi, string? chucVu, int? maTK = null)
        {
            MaNV = maNV;
            HoTen = hoTen;
            SDT = sdt;
            DiaChi = diaChi;
            ChucVu = chucVu;
            MaTK = maTK;
        }
    }
}
