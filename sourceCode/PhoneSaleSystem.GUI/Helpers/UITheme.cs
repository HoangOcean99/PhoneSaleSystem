using System.Drawing;
using System.Windows.Forms;

namespace PhoneSaleSystem.GUI.Helpers
{
    public static class UITheme
    {
        // ==========================================
        // BẢNG MÀU HIỆN ĐẠI (TAILWIND / MODERN SAAS)
        // ==========================================
        
        // Brand & Primary
        public static readonly Color Primary = Color.FromArgb(79, 70, 229);        // Indigo 600 #4F46E5
        public static readonly Color PrimaryHover = Color.FromArgb(67, 56, 202);   // Indigo 700 #4338CA
        public static readonly Color PrimaryLight = Color.FromArgb(238, 242, 255); // Indigo 50 #EEF2FF
        
        // Dark Sidebar
        public static readonly Color SidebarBg = Color.FromArgb(15, 23, 42);       // Slate 900 #0F172A
        public static readonly Color SidebarHover = Color.FromArgb(30, 41, 59);    // Slate 800 #1E293B
        public static readonly Color SidebarActive = Color.FromArgb(79, 70, 229);   // Indigo 600
        public static readonly Color Secondary = Color.FromArgb(15, 23, 42);
        
        // Accents
        public static readonly Color Success = Color.FromArgb(16, 185, 129);       // Emerald 500 #10B981
        public static readonly Color SuccessHover = Color.FromArgb(5, 150, 105);   // Emerald 600 #059669
        public static readonly Color SuccessLight = Color.FromArgb(236, 253, 245); // Emerald 50 #ECFDF5

        public static readonly Color Danger = Color.FromArgb(239, 68, 68);         // Red 500 #EF4444
        public static readonly Color DangerHover = Color.FromArgb(220, 38, 38);    // Red 600 #DC2626
        public static readonly Color DangerLight = Color.FromArgb(254, 242, 242);  // Red 50 #FEF2F2

        public static readonly Color Warning = Color.FromArgb(245, 158, 11);       // Amber 500 #F59E0B
        public static readonly Color WarningLight = Color.FromArgb(254, 243, 199); // Amber 50 #FEF3C7

        // Surfaces & Backgrounds
        public static readonly Color Background = Color.FromArgb(248, 250, 252);   // Slate 50 #F8FAFC
        public static readonly Color CardBg = Color.White;
        public static readonly Color CardHeader = Color.FromArgb(241, 245, 249);   // Slate 100 #F1F5F9
        public static readonly Color Border = Color.FromArgb(226, 232, 240);       // Slate 200 #E2E8F0
        public static readonly Color BorderLight = Color.FromArgb(241, 245, 249);  // Slate 100 #F1F5F9

        // Typography
        public static readonly Color TextMain = Color.FromArgb(15, 23, 42);        // Slate 900 #0F172A
        public static readonly Color TextSub = Color.FromArgb(71, 85, 105);        // Slate 600 #475569
        public static readonly Color TextMuted = Color.FromArgb(148, 163, 184);    // Slate 400 #94A3B8

        // ==========================================
        // STYLING CHO DATAGRIDVIEW PHONG CÁCH STRIPE
        // ==========================================
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(241, 245, 249);
            dgv.EnableHeadersVisualStyles = false;

            // Tiêu đề bảng phong cách Web hiện đại (nền xám nhẹ, chữ tối thanh lịch)
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249); // Slate 100
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);    // Slate 700
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.ColumnHeadersHeight = 44;

            // Dòng dữ liệu
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextMain;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255); // Indigo 50
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(67, 56, 202);   // Indigo 700
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // Dòng so le
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252); // Slate 50
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(67, 56, 202);

            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 42;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToResizeRows = false;
        }

        public static void StyleButton(Button btn, Color bg, Color fg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }
    }
}
