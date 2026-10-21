using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.GUI.Helpers
{
    public static class SessionManager
    {
        public static TaiKhoanDTO? CurrentUser { get; set; }
        public static NhanVienDTO? CurrentNhanVien { get; set; }

        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsAdmin => CurrentUser != null && string.Equals(CurrentUser.Quyen, "Admin", System.StringComparison.OrdinalIgnoreCase);

        public static void Logout()
        {
            CurrentUser = null;
            CurrentNhanVien = null;
        }
    }
}
