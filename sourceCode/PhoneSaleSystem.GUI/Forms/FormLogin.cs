using System;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public partial class FormLogin : Form
    {
        private readonly TaiKhoanBLL _taiKhoanBLL = new();
        private readonly NhanVienBLL _nhanVienBLL = new();

        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private RoundedButton btnLogin = null!;
        private RoundedButton btnExit = null!;
        private Label lblError = null!;
        private CheckBox chkShowPassword = null!;

        public FormLogin()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "PhoneSale Pro - Đăng Nhập Hệ Thống";
            this.Size = new Size(480, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // 1. Panel Header (Indigo 600)
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 115,
                BackColor = UITheme.Primary,
                Padding = new Padding(20, 16, 20, 16)
            };

            var lblTitle = new Label
            {
                Text = "⚡ PHONE SALE PRO",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblSub = new Label
            {
                Text = "Hệ Thống Quản Lý Bán Điện Thoại & Doanh Thu",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(224, 231, 255), // Indigo 100
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(lblTitle);

            // 2. Container Panel
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(45, 20, 45, 20)
            };

            var lblUser = new Label
            {
                Text = "Tên đăng nhập:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = UITheme.TextMain,
                Location = new Point(45, 20),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(45, 48),
                Width = 370,
                Text = "admin"
            };

            var lblPass = new Label
            {
                Text = "Mật khẩu:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = UITheme.TextMain,
                Location = new Point(45, 100),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(45, 128),
                Width = 370,
                UseSystemPasswordChar = true,
                Text = "123456"
            };

            chkShowPassword = new CheckBox
            {
                Text = "Hiển thị mật khẩu",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = UITheme.TextSub,
                Location = new Point(45, 172),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = UITheme.Danger,
                Location = new Point(45, 200),
                Width = 370,
                Height = 35
            };

            btnLogin = new RoundedButton
            {
                Text = "ĐĂNG NHẬP HỆ THỐNG",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(45, 245),
                Width = 370,
                Height = 46,
                BorderRadius = 8,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnLogin.Click += BtnLogin_Click;

            btnExit = new RoundedButton
            {
                Text = "Thoát Ứng Dụng",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(45, 302),
                Width = 370,
                Height = 40,
                BorderRadius = 8,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            btnExit.Click += (s, e) => Application.Exit();

            pnlBody.Controls.Add(lblUser);
            pnlBody.Controls.Add(txtUsername);
            pnlBody.Controls.Add(lblPass);
            pnlBody.Controls.Add(txtPassword);
            pnlBody.Controls.Add(chkShowPassword);
            pnlBody.Controls.Add(lblError);
            pnlBody.Controls.Add(btnLogin);
            pnlBody.Controls.Add(btnExit);

            // Docking order
            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlHeader);
            pnlHeader.SendToBack();

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                var user = _taiKhoanBLL.Login(username, password, out string errorMessage);
                if (user == null)
                {
                    lblError.Text = string.IsNullOrEmpty(errorMessage) 
                        ? "Tên đăng nhập hoặc mật khẩu không chính xác!" 
                        : errorMessage;
                    txtPassword.Focus();
                    return;
                }

                // Lưu phiên đăng nhập
                SessionManager.CurrentUser = user;
                SessionManager.CurrentNhanVien = _nhanVienBLL.GetByMaTK(user.MaTK) ?? new DTO.NhanVienDTO
                {
                    MaNV = 1,
                    HoTen = user.TenDangNhap,
                    ChucVu = user.Quyen
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "Lỗi kết nối CSDL: " + ex.Message;
                MessageBox.Show(
                    "Không thể kết nối đến máy chủ SQL Server!\n\n" +
                    $"Chi tiết lỗi: {ex.Message}\n\n" +
                    "Gợi ý xử lý:\n" +
                    "1. Kiểm tra chuỗi kết nối trong file .env (Server=...).\n" +
                    "2. Đảm bảo dịch vụ SQL Server (SQLEXPRESS) trên máy bạn đang chạy.",
                    "Lỗi Kết Nối CSDL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}
