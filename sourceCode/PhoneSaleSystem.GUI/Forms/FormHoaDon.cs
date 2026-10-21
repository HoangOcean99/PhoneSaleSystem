using System;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.DTO;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormHoaDon : Form
    {
        private readonly HoaDonBLL _hoaDonBLL = new();

        private DataGridView dgvHoaDon = null!;
        private DataGridView dgvChiTiet = null!;
        private TextBox txtSearch = null!;
        private DateTimePicker dtpTuNgay = null!;
        private DateTimePicker dtpDenNgay = null!;
        private RoundedButton btnFilter = null!;
        private RoundedButton btnReload = null!;
        private Label lblTotalInvoiceCount = null!;
        private Label lblTotalRevenue = null!;

        public FormHoaDon()
        {
            InitializeCustomComponents();
            LoadHoaDon();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Quản Lý Hóa Đơn Bán Hàng";
            this.Size = new Size(1180, 720);
            this.BackColor = UITheme.Background;
            this.Padding = new Padding(12);

            // Filter Card (RoundedPanel)
            var pnlFilterCard = new RoundedPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };

            var lblKw = new Label { Text = "Tìm kiếm:", Location = new Point(14, 18), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            txtSearch = new TextBox { Location = new Point(85, 14), Width = 220, Font = new Font("Segoe UI", 10F), PlaceholderText = "Tên NV, khách hàng..." };

            var lblTu = new Label { Text = "Từ ngày:", Location = new Point(325, 18), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            dtpTuNgay = new DateTimePicker { Location = new Point(390, 14), Width = 120, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Font = new Font("Segoe UI", 10F) };

            var lblDen = new Label { Text = "Đến ngày:", Location = new Point(525, 18), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            dtpDenNgay = new DateTimePicker { Location = new Point(595, 14), Width = 120, Format = DateTimePickerFormat.Short, Value = DateTime.Today, Font = new Font("Segoe UI", 10F) };

            btnFilter = new RoundedButton
            {
                Text = "⚡ Lọc Dữ Liệu",
                Location = new Point(735, 12),
                Width = 110,
                Height = 34,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnFilter.Click += (s, e) => FilterHoaDon();

            btnReload = new RoundedButton
            {
                Text = "Tất cả",
                Location = new Point(855, 12),
                Width = 75,
                Height = 34,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub
            };
            btnReload.Click += (s, e) => { txtSearch.Text = ""; LoadHoaDon(); };

            pnlFilterCard.Controls.Add(lblKw);
            pnlFilterCard.Controls.Add(txtSearch);
            pnlFilterCard.Controls.Add(lblTu);
            pnlFilterCard.Controls.Add(dtpTuNgay);
            pnlFilterCard.Controls.Add(lblDen);
            pnlFilterCard.Controls.Add(dtpDenNgay);
            pnlFilterCard.Controls.Add(btnFilter);
            pnlFilterCard.Controls.Add(btnReload);

            this.Controls.Add(pnlFilterCard);

            // Body Area chia 2 card
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0),
                BackColor = UITheme.Background
            };

            // Bottom Detail Card
            var pnlDetailCard = new RoundedPanel
            {
                Dock = DockStyle.Bottom,
                Height = 250,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };

            var lblDetailTitle = new Label
            {
                Text = "📦  CHI TIẾT SẢN PHẨM TRONG HÓA ĐƠN ĐƯỢC CHỌN",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = 30
            };

            dgvChiTiet = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvChiTiet);

            pnlDetailCard.Controls.Add(dgvChiTiet);
            pnlDetailCard.Controls.Add(lblDetailTitle);

            // Top Invoice Card
            var pnlInvoiceCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };

            var pnlInvoiceHeader = new Panel { Dock = DockStyle.Top, Height = 35 };
            
            var lblInvoiceTitle = new Label
            {
                Text = "🧾  DANH SÁCH HÓA ĐƠN ĐÃ XUẤT",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Left,
                AutoSize = true
            };
            
            lblTotalInvoiceCount = new Label
            {
                Text = "Số hóa đơn: 0",
                Dock = DockStyle.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = UITheme.TextMain
            };

            lblTotalRevenue = new Label
            {
                Text = "Tổng doanh thu: 0 đ  |  ",
                Dock = DockStyle.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = UITheme.Danger
            };

            pnlInvoiceHeader.Controls.Add(lblInvoiceTitle);
            pnlInvoiceHeader.Controls.Add(lblTotalRevenue);
            pnlInvoiceHeader.Controls.Add(lblTotalInvoiceCount);

            dgvHoaDon = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvHoaDon);
            dgvHoaDon.CellClick += DgvHoaDon_CellClick;

            pnlInvoiceCard.Controls.Add(dgvHoaDon);
            pnlInvoiceCard.Controls.Add(pnlInvoiceHeader);

            // Spacing panel between top and bottom cards
            var pnlSpacing = new Panel { Dock = DockStyle.Bottom, Height = 10, BackColor = UITheme.Background };

            pnlBody.Controls.Add(pnlInvoiceCard);
            pnlBody.Controls.Add(pnlSpacing);
            pnlBody.Controls.Add(pnlDetailCard);

            this.Controls.Add(pnlBody);
            pnlFilterCard.SendToBack();
        }

        private void LoadHoaDon()
        {
            var list = _hoaDonBLL.GetAll();
            dgvHoaDon.DataSource = list;
            FormatHoaDonGrid();
            UpdateStats(list);
        }

        private void FilterHoaDon()
        {
            var list = _hoaDonBLL.Search(txtSearch.Text, dtpTuNgay.Value.Date, dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1));
            dgvHoaDon.DataSource = list;
            FormatHoaDonGrid();
            UpdateStats(list);
        }

        private void UpdateStats(System.Collections.Generic.List<HoaDonDTO> list)
        {
            decimal sum = 0;
            foreach (var h in list) sum += h.TongTien;
            lblTotalInvoiceCount.Text = $"Số hóa đơn: {list.Count}";
            lblTotalRevenue.Text = $"Tổng doanh thu: {sum:N0} đ  |  ";
        }

        private void FormatHoaDonGrid()
        {
            if (dgvHoaDon.Columns["MaHD"] != null) dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
            if (dgvHoaDon.Columns["TenNV"] != null) dgvHoaDon.Columns["TenNV"].HeaderText = "Nhân Viên Lập";
            if (dgvHoaDon.Columns["TenKH"] != null) dgvHoaDon.Columns["TenKH"].HeaderText = "Khách Hàng";
            if (dgvHoaDon.Columns["NgayLap"] != null)
            {
                dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dgvHoaDon.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            if (dgvHoaDon.Columns["TongTien"] != null)
            {
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền (VNĐ)";
                dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            }
            if (dgvHoaDon.Columns["MaNV"] != null) dgvHoaDon.Columns["MaNV"].Visible = false;
            if (dgvHoaDon.Columns["MaKH"] != null) dgvHoaDon.Columns["MaKH"].Visible = false;
        }

        private void DgvHoaDon_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvHoaDon.Rows[e.RowIndex];
            if (int.TryParse(row.Cells["MaHD"].Value?.ToString(), out int maHD))
            {
                var details = _hoaDonBLL.GetChiTietHoaDon(maHD);
                dgvChiTiet.DataSource = details;
                FormatDetailGrid();
            }
        }

        private void FormatDetailGrid()
        {
            if (dgvChiTiet.Columns["MaHD"] != null) dgvChiTiet.Columns["MaHD"].Visible = false;
            if (dgvChiTiet.Columns["MaDT"] != null) dgvChiTiet.Columns["MaDT"].HeaderText = "Mã ĐT";
            if (dgvChiTiet.Columns["TenDT"] != null) dgvChiTiet.Columns["TenDT"].HeaderText = "Tên Điện Thoại";
            if (dgvChiTiet.Columns["SoLuong"] != null) dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
            if (dgvChiTiet.Columns["DonGia"] != null)
            {
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá (VNĐ)";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvChiTiet.Columns["ThanhTien"] != null)
            {
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền (VNĐ)";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
        }
    }
}
