using System;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormThongKe : Form
    {
        private readonly ThongKeBLL _thongKeBLL = new();

        private DateTimePicker dtpTuNgay = null!;
        private DateTimePicker dtpDenNgay = null!;
        private RoundedButton btnThongKe = null!;

        // KPI Labels
        private Label lblKpiDoanhThu = null!;
        private Label lblKpiHoaDon = null!;
        private Label lblKpiSanPham = null!;

        // Tab Views
        private TabControl tabControl = null!;
        private DataGridView dgvDoanhThu = null!;
        private DataGridView dgvTopSanPham = null!;
        private DataGridView dgvTonKho = null!;

        public FormThongKe()
        {
            InitializeCustomComponents();
            LoadThongKe();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Báo Cáo & Thống Kê Doanh Thu";
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

            var lblTu = new Label { Text = "Từ ngày:", Location = new Point(14, 18), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            dtpTuNgay = new DateTimePicker { Location = new Point(85, 14), Width = 130, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Font = new Font("Segoe UI", 10F) };

            var lblDen = new Label { Text = "Đến ngày:", Location = new Point(235, 18), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UITheme.TextMain };
            dtpDenNgay = new DateTimePicker { Location = new Point(310, 14), Width = 130, Format = DateTimePickerFormat.Short, Value = DateTime.Today, Font = new Font("Segoe UI", 10F) };

            btnThongKe = new RoundedButton
            {
                Text = "⚡ XEM BÁO CÁO",
                Location = new Point(460, 11),
                Width = 140,
                Height = 34,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btnThongKe.Click += (s, e) => LoadThongKe();

            pnlFilterCard.Controls.Add(lblTu);
            pnlFilterCard.Controls.Add(dtpTuNgay);
            pnlFilterCard.Controls.Add(lblDen);
            pnlFilterCard.Controls.Add(dtpDenNgay);
            pnlFilterCard.Controls.Add(btnThongKe);

            this.Controls.Add(pnlFilterCard);

            // KPI Summary Row
            var pnlKpi = new Panel
            {
                Dock = DockStyle.Top,
                Height = 105,
                Padding = new Padding(0, 10, 0, 10),
                BackColor = UITheme.Background
            };
            
            RoundedPanel CreateKpiCard(string title, out Label valLabel, Color accent, int x)
            {
                var card = new RoundedPanel
                {
                    Location = new Point(x, 5),
                    Size = new Size(360, 85),
                    BorderRadius = 10,
                    BorderColor = UITheme.Border,
                    BackColor = Color.White,
                    Padding = new Padding(16, 12, 16, 12)
                };

                var pnlColor = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent };
                
                var lblT = new Label
                {
                    Text = title,
                    Location = new Point(20, 12),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = UITheme.TextSub
                };

                valLabel = new Label
                {
                    Text = "0",
                    Location = new Point(20, 36),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = accent
                };

                card.Controls.Add(lblT);
                card.Controls.Add(valLabel);
                card.Controls.Add(pnlColor);
                return card;
            }

            pnlKpi.Controls.Add(CreateKpiCard("💰  TỔNG DOANH THU", out lblKpiDoanhThu, UITheme.Primary, 0));
            pnlKpi.Controls.Add(CreateKpiCard("🧾  TỔNG SỐ HÓA ĐƠN", out lblKpiHoaDon, UITheme.Success, 375));
            pnlKpi.Controls.Add(CreateKpiCard("📱  SẢN PHẨM ĐÃ BÁN", out lblKpiSanPham, UITheme.Warning, 750));

            this.Controls.Add(pnlKpi);

            // Tab Control for Tables wrapped in a RoundedPanel
            var pnlTabsCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Padding = new Point(16, 8)
            };

            // Tab 1: Doanh thu theo ngày
            var tabDoanhThu = new TabPage("📈  Doanh Thu Theo Ngày");
            tabDoanhThu.BackColor = Color.White;
            dgvDoanhThu = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvDoanhThu);
            tabDoanhThu.Controls.Add(dgvDoanhThu);
            tabControl.TabPages.Add(tabDoanhThu);

            // Tab 2: Top sản phẩm bán chạy
            var tabTopSP = new TabPage("🏆  Sản Phẩm Bán Chạy");
            tabTopSP.BackColor = Color.White;
            dgvTopSanPham = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvTopSanPham);
            tabTopSP.Controls.Add(dgvTopSanPham);
            tabControl.TabPages.Add(tabTopSP);

            // Tab 3: Báo cáo tồn kho
            var tabTonKho = new TabPage("📦  Báo Cáo Tồn Kho");
            tabTonKho.BackColor = Color.White;
            dgvTonKho = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvTonKho);
            tabTonKho.Controls.Add(dgvTonKho);
            tabControl.TabPages.Add(tabTonKho);

            pnlTabsCard.Controls.Add(tabControl);
            this.Controls.Add(pnlTabsCard);

            pnlKpi.SendToBack();
            pnlFilterCard.SendToBack();
        }

        private void LoadThongKe()
        {
            var from = dtpTuNgay.Value.Date;
            var to = dtpDenNgay.Value.Date;

            // Update KPI
            var (tongTien, tongHD, tongSP) = _thongKeBLL.GetTongQuan(from, to);
            lblKpiDoanhThu.Text = $"{tongTien:N0} đ";
            lblKpiHoaDon.Text = $"{tongHD:N0} đơn";
            lblKpiSanPham.Text = $"{tongSP:N0} máy";

            // Tab 1: Doanh thu
            var dsDoanhThu = _thongKeBLL.GetDoanhThu(from, to);
            dgvDoanhThu.DataSource = dsDoanhThu;
            if (dgvDoanhThu.Columns["Ngay"] != null)
            {
                dgvDoanhThu.Columns["Ngay"].HeaderText = "Ngày";
                dgvDoanhThu.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgvDoanhThu.Columns["SoHoaDon"] != null) dgvDoanhThu.Columns["SoHoaDon"].HeaderText = "Số Hóa Đơn";
            if (dgvDoanhThu.Columns["DoanhThu"] != null)
            {
                dgvDoanhThu.Columns["DoanhThu"].HeaderText = "Tổng Doanh Thu (VNĐ)";
                dgvDoanhThu.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
            }

            // Tab 2: Top SP
            var dsTop = _thongKeBLL.GetTopSanPhamBanChay(5);
            dgvTopSanPham.DataSource = dsTop;
            if (dgvTopSanPham.Columns["MaDT"] != null) dgvTopSanPham.Columns["MaDT"].HeaderText = "Mã";
            if (dgvTopSanPham.Columns["TenDT"] != null) dgvTopSanPham.Columns["TenDT"].HeaderText = "Tên Điện Thoại";
            if (dgvTopSanPham.Columns["TenHang"] != null) dgvTopSanPham.Columns["TenHang"].HeaderText = "Hãng";
            if (dgvTopSanPham.Columns["TongDaBan"] != null) dgvTopSanPham.Columns["TongDaBan"].HeaderText = "Đã Bán (Máy)";
            if (dgvTopSanPham.Columns["TongDoanhThu"] != null)
            {
                dgvTopSanPham.Columns["TongDoanhThu"].HeaderText = "Doanh Thu (VNĐ)";
                dgvTopSanPham.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
            }

            // Tab 3: Tồn kho
            var dsTon = _thongKeBLL.GetBaoCaoTonKho();
            dgvTonKho.DataSource = dsTon;
            if (dgvTonKho.Columns["MaDT"] != null) dgvTonKho.Columns["MaDT"].HeaderText = "Mã";
            if (dgvTonKho.Columns["TenDT"] != null) dgvTonKho.Columns["TenDT"].HeaderText = "Tên Điện Thoại";
            if (dgvTonKho.Columns["TenHang"] != null) dgvTonKho.Columns["TenHang"].HeaderText = "Hãng";
            if (dgvTonKho.Columns["TonKho"] != null) dgvTonKho.Columns["TonKho"].HeaderText = "Số Lượng Tồn";
            if (dgvTonKho.Columns["TrangThai"] != null) dgvTonKho.Columns["TrangThai"].HeaderText = "Trạng Thái";
            if (dgvTonKho.Columns["GiaBan"] != null)
            {
                dgvTonKho.Columns["GiaBan"].HeaderText = "Giá Bán";
                dgvTonKho.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            }
        }
    }
}
