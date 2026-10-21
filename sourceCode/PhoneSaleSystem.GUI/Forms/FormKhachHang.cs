using System;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.DTO;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormKhachHang : Form
    {
        private readonly KhachHangBLL _khachHangBLL = new();

        private DataGridView dgvKhachHang = null!;
        private TextBox txtSearch = null!;
        private RoundedButton btnSearch = null!;
        private RoundedButton btnReload = null!;

        private TextBox txtMaKH = null!;
        private TextBox txtHoTen = null!;
        private TextBox txtSDT = null!;
        private TextBox txtDiaChi = null!;

        private RoundedButton btnThem = null!;
        private RoundedButton btnSua = null!;
        private RoundedButton btnXoa = null!;
        private RoundedButton btnLamMoi = null!;

        public FormKhachHang()
        {
            InitializeCustomComponents();
            LoadData();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Quản Lý Khách Hàng";
            this.Size = new Size(1180, 720);
            this.BackColor = UITheme.Background;
            this.Padding = new Padding(12);

            // Left Input Wrapper
            var pnlLeftWrapper = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = UITheme.Background,
                Padding = new Padding(0, 0, 10, 0)
            };

            var pnlInputCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            pnlLeftWrapper.Controls.Add(pnlInputCard);

            var lblCardTitle = new Label
            {
                Text = "👥  THÔNG TIN KHÁCH HÀNG",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Location = new Point(16, 12),
                AutoSize = true
            };
            pnlInputCard.Controls.Add(lblCardTitle);

            int y = 45;
            void AddField(string label, Control ctrl)
            {
                var lbl = new Label { Text = label, Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = UITheme.TextMain };
                ctrl.Location = new Point(16, y + 20);
                ctrl.Width = 320;
                pnlInputCard.Controls.Add(lbl);
                pnlInputCard.Controls.Add(ctrl);
                y += 58;
            }

            txtMaKH = new TextBox { ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249), Font = new Font("Segoe UI", 10F) };
            AddField("Mã khách hàng (Tự động):", txtMaKH);

            txtHoTen = new TextBox { Font = new Font("Segoe UI", 10F) };
            AddField("Họ và tên (*):", txtHoTen);

            txtSDT = new TextBox { Font = new Font("Segoe UI", 10F) };
            AddField("Số điện thoại (*):", txtSDT);

            txtDiaChi = new TextBox { Multiline = true, Height = 75, Font = new Font("Segoe UI", 10F) };
            AddField("Địa chỉ cư trú:", txtDiaChi);
            y += 35;

            // Nút bấm bo góc
            btnThem = new RoundedButton
            {
                Text = "➕ Thêm",
                Location = new Point(16, y),
                Width = 75,
                Height = 36,
                BorderRadius = 6,
                BackColor = UITheme.Success,
                HoverColor = UITheme.SuccessHover,
                ForeColor = Color.White
            };
            btnThem.Click += BtnThem_Click;

            btnSua = new RoundedButton
            {
                Text = "✏️ Sửa",
                Location = new Point(98, y),
                Width = 75,
                Height = 36,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnSua.Click += BtnSua_Click;

            btnXoa = new RoundedButton
            {
                Text = "🗑 Xóa",
                Location = new Point(180, y),
                Width = 75,
                Height = 36,
                BorderRadius = 6,
                BackColor = UITheme.Danger,
                HoverColor = UITheme.DangerHover,
                ForeColor = Color.White
            };
            btnXoa.Click += BtnXoa_Click;

            btnLamMoi = new RoundedButton
            {
                Text = "🔄 Mới",
                Location = new Point(262, y),
                Width = 75,
                Height = 36,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub
            };
            btnLamMoi.Click += (s, e) => ClearInputs();

            pnlInputCard.Controls.Add(btnThem);
            pnlInputCard.Controls.Add(btnSua);
            pnlInputCard.Controls.Add(btnXoa);
            pnlInputCard.Controls.Add(btnLamMoi);

            // Right Grid Wrapper & Card
            var pnlRightWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background,
                Padding = new Padding(10, 0, 0, 0)
            };

            var pnlRightCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            pnlRightWrapper.Controls.Add(pnlRightCard);

            var pnlSearch = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(0, 4, 0, 8) };
            
            var lblSearch = new Label { Text = "Tìm kiếm:", Location = new Point(0, 10), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            txtSearch = new TextBox { Location = new Point(80, 8), Width = 300, Font = new Font("Segoe UI", 10F), PlaceholderText = "Tên khách hàng hoặc số điện thoại..." };
            
            btnSearch = new RoundedButton
            {
                Text = "🔍 Tìm",
                Location = new Point(390, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnSearch.Click += (s, e) =>
            {
                dgvKhachHang.DataSource = _khachHangBLL.Search(txtSearch.Text);
                FormatGrid();
            };

            btnReload = new RoundedButton
            {
                Text = "Tất cả",
                Location = new Point(472, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub
            };
            btnReload.Click += (s, e) => { txtSearch.Text = ""; LoadData(); };

            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(btnReload);

            dgvKhachHang = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvKhachHang);
            dgvKhachHang.CellClick += DgvKhachHang_CellClick;

            pnlRightCard.Controls.Add(dgvKhachHang);
            pnlRightCard.Controls.Add(pnlSearch);

            this.Controls.Add(pnlRightWrapper);
            this.Controls.Add(pnlLeftWrapper);
            pnlLeftWrapper.SendToBack();
        }

        private void LoadData()
        {
            dgvKhachHang.DataSource = _khachHangBLL.GetAll();
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvKhachHang.Columns["MaKH"] != null) dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
            if (dgvKhachHang.Columns["HoTen"] != null) dgvKhachHang.Columns["HoTen"].HeaderText = "Họ và Tên";
            if (dgvKhachHang.Columns["SDT"] != null) dgvKhachHang.Columns["SDT"].HeaderText = "Số Điện Thoại";
            if (dgvKhachHang.Columns["DiaChi"] != null) dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa Chỉ";
        }

        private void DgvKhachHang_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvKhachHang.Rows[e.RowIndex];
            txtMaKH.Text = row.Cells["MaKH"].Value?.ToString() ?? "";
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
            txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var kh = new KhachHangDTO
            {
                HoTen = txtHoTen.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            if (_khachHangBLL.Add(kh, out string error))
            {
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKH.Text, out int maKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa trong bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var kh = new KhachHangDTO
            {
                MaKH = maKH,
                HoTen = txtHoTen.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            if (_khachHangBLL.Update(kh, out string error))
            {
                MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKH.Text, out int maKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa trong bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{txtHoTen.Text}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_khachHangBLL.Delete(maKH, out string error))
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputs()
        {
            txtMaKH.Text = "";
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            txtHoTen.Focus();
        }
    }
}
