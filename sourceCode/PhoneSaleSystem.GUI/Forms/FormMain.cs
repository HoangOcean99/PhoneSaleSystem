using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormMain : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlTopBar = null!;
        private Panel pnlContent = null!;
        private Label lblCurrentPage = null!;
        private Label lblUserInfo = null!;

        // Navigation buttons
        private RoundedButton btnNavBanHang = null!;
        private RoundedButton btnNavDienThoai = null!;
        private RoundedButton btnNavKhachHang = null!;
        private RoundedButton btnNavNhanVien = null!;
        private RoundedButton btnNavHoaDon = null!;
        private RoundedButton btnNavThongKe = null!;
        private RoundedButton btnNavLogout = null!;

        private readonly List<RoundedButton> _navButtons = new();
        private Form? _activeForm;

        public FormMain()
        {
            InitializeCustomComponents();
            ApplyRolePermissions();
            // Default view: Bán Hàng
            OpenChildForm(new FormBanHang(), "BÁN HÀNG & LẬP HÓA ĐƠN", btnNavBanHang);
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Hệ Thống Quản Lý Bán Điện Thoại - PhoneSale Pro";
            this.Size = new Size(1320, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1150, 700);
            this.BackColor = UITheme.Background;

            // 1. Sidebar (Trái - Slate 900)
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = UITheme.SidebarBg
            };

            // Sidebar Logo & Brand Header
            var pnlBrand = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(10, 15, 30),
                Padding = new Padding(16, 16, 16, 10)
            };

            var lblBrand = new Label
            {
                Text = "⚡ PHONE STORE",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblSub = new Label
            {
                Text = "Hệ Thống Quản Lý Bán Lẻ",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlBrand.Controls.Add(lblSub);
            pnlBrand.Controls.Add(lblBrand);
            pnlSidebar.Controls.Add(pnlBrand);

            // Sidebar Navigation Menu Items
            int navY = 100;
            RoundedButton CreateNavButton(string text, Action<RoundedButton> onSelect)
            {
                var btn = new RoundedButton
                {
                    Text = "  " + text,
                    Location = new Point(12, navY),
                    Size = new Size(216, 44),
                    BorderRadius = 8,
                    BackColor = UITheme.SidebarBg,
                    HoverColor = UITheme.SidebarHover,
                    ForeColor = Color.FromArgb(203, 213, 225), // Slate 300
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 0, 0, 0),
                    Cursor = Cursors.Hand
                };

                btn.Click += (s, e) => onSelect(btn);
                navY += 50;
                pnlSidebar.Controls.Add(btn);
                _navButtons.Add(btn);
                return btn;
            }

            btnNavBanHang = CreateNavButton("🛒  Bán Hàng", b => OpenChildForm(new FormBanHang(), "BÁN HÀNG & LẬP HÓA ĐƠN", b));
            btnNavDienThoai = CreateNavButton("📱  Quản Lý Điện Thoại", b => OpenChildForm(new FormDienThoai(), "QUẢN LÝ ĐIỆN THOẠI & TỒN KHO", b));
            btnNavKhachHang = CreateNavButton("👥  Quản Lý Khách Hàng", b => OpenChildForm(new FormKhachHang(), "QUẢN LÝ THÔNG TIN KHÁCH HÀNG", b));
            btnNavNhanVien = CreateNavButton("👨‍💼  Quản Lý Nhân Viên", b => OpenChildForm(new FormNhanVien(), "QUẢN LÝ NHÂN VIÊN & PHÂN QUYỀN", b));
            btnNavHoaDon = CreateNavButton("🧾  Lịch Sử Hóa Đơn", b => OpenChildForm(new FormHoaDon(), "LỊCH SỬ HÓA ĐƠN & CHI TIẾT", b));
            btnNavThongKe = CreateNavButton("📊  Báo Cáo Thống Kê", b => OpenChildForm(new FormThongKe(), "BÁO CÁO THỐNG KÊ DOANH THU", b));

            // User Profile Card & Logout at Sidebar Bottom
            var pnlUserCard = new RoundedPanel
            {
                Dock = DockStyle.Bottom,
                Height = 105,
                BorderRadius = 10,
                BorderColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(12)
            };

            lblUserInfo = new Label
            {
                Text = "👤 Quản Trị Viên\n[Admin]",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 42
            };

            btnNavLogout = new RoundedButton
            {
                Text = "ĐĂNG XUẤT",
                Dock = DockStyle.Bottom,
                Height = 32,
                BorderRadius = 6,
                BackColor = Color.FromArgb(239, 68, 68),
                HoverColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
            btnNavLogout.Click += (s, e) => Logout();

            pnlUserCard.Controls.Add(btnNavLogout);
            pnlUserCard.Controls.Add(lblUserInfo);

            var pnlUserWrapper = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 125,
                BackColor = UITheme.SidebarBg,
                Padding = new Padding(12, 10, 12, 12)
            };
            pnlUserWrapper.Controls.Add(pnlUserCard);
            pnlSidebar.Controls.Add(pnlUserWrapper);

            // 2. TopBar (Nền trắng, viền mỏng thanh lịch)
            pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 24, 12)
            };

            lblCurrentPage = new Label
            {
                Text = "BÁN HÀNG & LẬP HÓA ĐƠN",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Left,
                AutoSize = true
            };

            var lblDateTime = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd/MM/yyyy"),
                Font = new Font("Segoe UI", 10F),
                ForeColor = UITheme.TextSub,
                Dock = DockStyle.Right,
                AutoSize = true
            };

            pnlTopBar.Controls.Add(lblDateTime);
            pnlTopBar.Controls.Add(lblCurrentPage);

            // Viền dưới của Topbar
            var pnlTopBorder = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = UITheme.Border
            };
            pnlTopBar.Controls.Add(pnlTopBorder);

            // 3. Content Panel (Nền xám nhạt Slate 50)
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background,
                Padding = new Padding(16)
            };

            // Thứ tự Docking chuẩn WinForms
            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlSidebar);

            pnlSidebar.SendToBack();
            pnlTopBar.SendToBack();
        }

        private void SetActiveButton(RoundedButton activeBtn)
        {
            foreach (var btn in _navButtons)
            {
                btn.BackColor = UITheme.SidebarBg;
                btn.ForeColor = Color.FromArgb(203, 213, 225);
            }
            activeBtn.BackColor = UITheme.SidebarActive;
            activeBtn.ForeColor = Color.White;
        }

        private void OpenChildForm(Form childForm, string pageTitle, RoundedButton? navBtn = null)
        {
            _activeForm?.Close();
            _activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            lblCurrentPage.Text = pageTitle;
            if (navBtn != null)
            {
                SetActiveButton(navBtn);
            }
        }

        private void ApplyRolePermissions()
        {
            var user = SessionManager.CurrentUser;
            string role = user?.Quyen ?? "NhanVien";
            string name = SessionManager.CurrentNhanVien?.HoTen ?? user?.TenDangNhap ?? "User";

            lblUserInfo.Text = $"👤 {name}\nVai trò: [{role}]";

            // Phân quyền theo chuẩn báo cáo:
            // Admin: Có toàn quyền mọi mục
            // NhanVien: Chỉ bán hàng, quản lý điện thoại (xem), khách hàng, hóa đơn
            if (role != "Admin")
            {
                btnNavNhanVien.Visible = false;
                btnNavThongKe.Visible = false;
            }
        }

        private void Logout()
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionManager.Logout();
                this.Hide();
                using var login = new FormLogin();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    ApplyRolePermissions();
                    this.Show();
                    OpenChildForm(new FormBanHang(), "BÁN HÀNG & LẬP HÓA ĐƠN", btnNavBanHang);
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
