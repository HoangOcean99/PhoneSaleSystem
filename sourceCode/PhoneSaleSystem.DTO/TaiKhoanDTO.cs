namespace PhoneSaleSystem.DTO
{
    public class TaiKhoanDTO
    {
        public int MaTK { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string Quyen { get; set; } = "NhanVien"; // "Admin" or "NhanVien"

        public TaiKhoanDTO() { }

        public TaiKhoanDTO(int maTK, string tenDangNhap, string matKhau, string quyen)
        {
            MaTK = maTK;
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            Quyen = quyen;
        }
    }
}
