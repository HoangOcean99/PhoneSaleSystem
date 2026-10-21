namespace PhoneSaleSystem.DTO
{
    public class HangDTO
    {
        public int MaHang { get; set; }
        public string TenHang { get; set; } = string.Empty;

        public HangDTO() { }

        public HangDTO(int maHang, string tenHang)
        {
            MaHang = maHang;
            TenHang = tenHang;
        }

        public override string ToString()
        {
            return TenHang;
        }
    }
}
