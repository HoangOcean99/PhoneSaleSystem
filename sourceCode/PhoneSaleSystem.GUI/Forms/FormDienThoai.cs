using System;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.DTO;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormDienThoai : Form
    {
        private readonly DienThoaiBLL _dienThoaiBLL = new();
        private readonly HangBLL _hangBLL = new();

        private DataGridView dgvDienThoai = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboFilterHang = null!;
        private RoundedButton btnSearch = null!;
        private RoundedButton btnReload = null!;

        // Input controls
        private TextBox txtMaDT = null!;
        private TextBox txtTenDT = null!;
        private ComboBox cboHang = null!;
        private TextBox txtRAM = null!;
        private TextBox txtBoNho = null!;
        private TextBox txtMauSac = null!;
        private NumericUpDown numGiaNhap = null!;
        private NumericUpDown numGiaBan = null!;
        private NumericUpDown numSoLuong = null!;
        private NumericUpDown numBaoHanh = null!;

        private RoundedButton btnThem = null!;
        private RoundedButton btnSua = null!;
        private RoundedButton btnXoa = null!;
        private RoundedButton btnLamMoi = null!;

        public FormDienThoai()
        {
            InitializeCustomComponents();
            LoadHangData();
            LoadDienThoaiData();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Quản Lý Sản Phẩm Điện Thoại";
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

            // Left Input Card (RoundedPanel)
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
                Text = "📝  THÔNG TIN SẢN PHẨM",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Location = new Point(16, 12),
                AutoSize = true
            };
            pnlInputCard.Controls.Add(lblCardTitle);

            int y = 42;
            void AddField(string label, Control ctrl)
            {
                var lbl = new Label { Text = label, Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = UITheme.TextMain };
                ctrl.Location = new Point(16, y + 20);
                ctrl.Width = 320;
                pnlInputCard.Controls.Add(lbl);
                pnlInputCard.Controls.Add(ctrl);
                y += 50;
            }

            txtMaDT = new TextBox { ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249), Font = new Font("Segoe UI", 10F) };
            AddField("Mã sản phẩm (Tự động):", txtMaDT);

            txtTenDT = new TextBox { Font = new Font("Segoe UI", 10F) };
            AddField("Tên điện thoại (*):", txtTenDT);

            cboHang = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
            AddField("Hãng sản xuất (*):", cboHang);

            txtRAM = new TextBox { Text = "8GB", Font = new Font("Segoe UI", 10F) };
            AddField("RAM:", txtRAM);

            txtBoNho = new TextBox { Text = "128GB", Font = new Font("Segoe UI", 10F) };
            AddField("Bộ nhớ trong (ROM):", txtBoNho);

            txtMauSac = new TextBox { Font = new Font("Segoe UI", 10F) };
            AddField("Màu sắc:", txtMauSac);

            numGiaNhap = new NumericUpDown { Maximum = 1000000000, ThousandsSeparator = true, Value = 10000000, Font = new Font("Segoe UI", 10F) };
            AddField("Giá nhập (VNĐ):", numGiaNhap);

            numGiaBan = new NumericUpDown { Maximum = 1000000000, ThousandsSeparator = true, Value = 12000000, Font = new Font("Segoe UI", 10F) };
            AddField("Giá bán (VNĐ):", numGiaBan);

            numSoLuong = new NumericUpDown { Maximum = 10000, Value = 10, Font = new Font("Segoe UI", 10F) };
            AddField("Số lượng tồn kho:", numSoLuong);

            numBaoHanh = new NumericUpDown { Maximum = 60, Value = 12, Font = new Font("Segoe UI", 10F) };
            AddField("Bảo hành (tháng):", numBaoHanh);

            // Action Buttons
            btnThem = new RoundedButton
            {
                Text = "➕ Thêm",
                Location = new Point(16, y + 10),
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
                Location = new Point(98, y + 10),
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
                Location = new Point(180, y + 10),
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
                Location = new Point(262, y + 10),
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
            
            var lblSearch = new Label { Text = "Tìm:", Location = new Point(0, 10), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            txtSearch = new TextBox { Location = new Point(40, 8), Width = 180, Font = new Font("Segoe UI", 10F), PlaceholderText = "Tên máy..." };
            
            var lblFilter = new Label { Text = "Hãng:", Location = new Point(230, 10), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            cboFilterHang = new ComboBox { Location = new Point(280, 8), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
            
            btnSearch = new RoundedButton
            {
                Text = "🔍 Tìm",
                Location = new Point(430, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnSearch.Click += (s, e) => SearchDienThoai();

            btnReload = new RoundedButton
            {
                Text = "Tất cả",
                Location = new Point(512, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub
            };
            btnReload.Click += (s, e) => { txtSearch.Text = ""; cboFilterHang.SelectedIndex = 0; LoadDienThoaiData(); };

            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblFilter);
            pnlSearch.Controls.Add(cboFilterHang);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(btnReload);

            dgvDienThoai = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvDienThoai);
            dgvDienThoai.CellClick += DgvDienThoai_CellClick;

            pnlRightCard.Controls.Add(dgvDienThoai);
            pnlRightCard.Controls.Add(pnlSearch);

            this.Controls.Add(pnlRightWrapper);
            this.Controls.Add(pnlLeftWrapper);
            pnlLeftWrapper.SendToBack();
        }

        private void LoadHangData()
        {
            var hangs = _hangBLL.GetAll();
            cboHang.DataSource = new BindingSource(hangs, null);
            cboHang.DisplayMember = "TenHang";
            cboHang.ValueMember = "MaHang";

            var filterList = new System.Collections.Generic.List<HangDTO>
            {
                new HangDTO(0, "-- Tất cả hãng --")
            };
            filterList.AddRange(hangs);
            cboFilterHang.DataSource = new BindingSource(filterList, null);
            cboFilterHang.DisplayMember = "TenHang";
            cboFilterHang.ValueMember = "MaHang";
        }

        private void LoadDienThoaiData()
        {
            var list = _dienThoaiBLL.GetAll();
            dgvDienThoai.DataSource = list;
            FormatGridColumns();
        }

        private void SearchDienThoai()
        {
            int? maHang = null;
            if (cboFilterHang.SelectedValue is int hId && hId > 0)
            {
                maHang = hId;
            }
            var list = _dienThoaiBLL.Search(txtSearch.Text, maHang);
            dgvDienThoai.DataSource = list;
            FormatGridColumns();
        }

        private void FormatGridColumns()
        {
            if (dgvDienThoai.Columns["MaDT"] != null) dgvDienThoai.Columns["MaDT"].HeaderText = "Mã";
            if (dgvDienThoai.Columns["TenDT"] != null) dgvDienThoai.Columns["TenDT"].HeaderText = "Tên Sản Phẩm";
            if (dgvDienThoai.Columns["TenHang"] != null) dgvDienThoai.Columns["TenHang"].HeaderText = "Hãng";
            if (dgvDienThoai.Columns["RAM"] != null) dgvDienThoai.Columns["RAM"].HeaderText = "RAM";
            if (dgvDienThoai.Columns["BoNho"] != null) dgvDienThoai.Columns["BoNho"].HeaderText = "Bộ Nhớ";
            if (dgvDienThoai.Columns["MauSac"] != null) dgvDienThoai.Columns["MauSac"].HeaderText = "Màu";
            if (dgvDienThoai.Columns["GiaNhap"] != null)
            {
                dgvDienThoai.Columns["GiaNhap"].HeaderText = "Giá Nhập";
                dgvDienThoai.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            }
            if (dgvDienThoai.Columns["GiaBan"] != null)
            {
                dgvDienThoai.Columns["GiaBan"].HeaderText = "Giá Bán";
                dgvDienThoai.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            }
            if (dgvDienThoai.Columns["SoLuong"] != null) dgvDienThoai.Columns["SoLuong"].HeaderText = "Tồn";
            if (dgvDienThoai.Columns["BaoHanh"] != null) dgvDienThoai.Columns["BaoHanh"].HeaderText = "Bảo Hành (tháng)";
            if (dgvDienThoai.Columns["MaHang"] != null) dgvDienThoai.Columns["MaHang"].Visible = false;
        }

        private void DgvDienThoai_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDienThoai.Rows.Count) return;
            var row = dgvDienThoai.Rows[e.RowIndex];
            txtMaDT.Text = row.Cells["MaDT"].Value?.ToString();
            txtTenDT.Text = row.Cells["TenDT"].Value?.ToString();
            if (row.Cells["MaHang"].Value is int maHang)
            {
                cboHang.SelectedValue = maHang;
            }
            txtRAM.Text = row.Cells["RAM"].Value?.ToString();
            txtBoNho.Text = row.Cells["BoNho"].Value?.ToString();
            txtMauSac.Text = row.Cells["MauSac"].Value?.ToString();
            numGiaNhap.Value = Convert.ToDecimal(row.Cells["GiaNhap"].Value ?? 0);
            numGiaBan.Value = Convert.ToDecimal(row.Cells["GiaBan"].Value ?? 0);
            numSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuong"].Value ?? 0);
            numBaoHanh.Value = Convert.ToDecimal(row.Cells["BaoHanh"].Value ?? 12);
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var dt = GetModelFromInputs();
            if (_dienThoaiBLL.Add(dt, out string error))
            {
                MessageBox.Show("Thêm điện thoại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDienThoaiData();
                ClearInputs();
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaDT.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần sửa từ bảng danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dt = GetModelFromInputs();
            dt.MaDT = id;
            if (_dienThoaiBLL.Update(dt, out string error))
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDienThoaiData();
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaDT.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn điện thoại cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{txtTenDT.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_dienThoaiBLL.Delete(id, out string error))
                {
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDienThoaiData();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private DienThoaiDTO GetModelFromInputs()
        {
            return new DienThoaiDTO
            {
                TenDT = txtTenDT.Text.Trim(),
                MaHang = (int)(cboHang.SelectedValue ?? 1),
                RAM = txtRAM.Text.Trim(),
                BoNho = txtBoNho.Text.Trim(),
                MauSac = txtMauSac.Text.Trim(),
                GiaNhap = numGiaNhap.Value,
                GiaBan = numGiaBan.Value,
                SoLuong = (int)numSoLuong.Value,
                BaoHanh = (int)numBaoHanh.Value
            };
        }

        private void ClearInputs()
        {
            txtMaDT.Text = "";
            txtTenDT.Text = "";
            txtRAM.Text = "8GB";
            txtBoNho.Text = "128GB";
            txtMauSac.Text = "";
            numGiaNhap.Value = 10000000;
            numGiaBan.Value = 12000000;
            numSoLuong.Value = 10;
            numBaoHanh.Value = 12;
            txtTenDT.Focus();
        }
    }
}
