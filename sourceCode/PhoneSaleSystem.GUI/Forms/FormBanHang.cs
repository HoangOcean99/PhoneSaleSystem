using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PhoneSaleSystem.BLL;
using PhoneSaleSystem.DTO;
using PhoneSaleSystem.GUI.Controls;
using PhoneSaleSystem.GUI.Helpers;

namespace PhoneSaleSystem.GUI.Forms
{
    public class FormBanHang : Form
    {
        private readonly DienThoaiBLL _dienThoaiBLL = new();
        private readonly KhachHangBLL _khachHangBLL = new();
        private readonly BanHangBLL _banHangBLL = new();

        private ComboBox cboKhachHang = null!;
        private RoundedButton btnQuickAddKH = null!;
        private TextBox txtSearchDT = null!;
        private RoundedButton btnSearchDT = null!;
        private RoundedButton btnReloadDT = null!;
        private DataGridView dgvCatalog = null!;
        private NumericUpDown numQuantity = null!;
        private RoundedButton btnAddToCart = null!;

        // Giỏ hàng
        private DataGridView dgvCart = null!;
        private RoundedButton btnRemoveFromCart = null!;
        private RoundedButton btnClearCart = null!;
        private Label lblTongTien = null!;
        private NumericUpDown numKhachDua = null!;
        private Label lblTienThua = null!;
        private RoundedButton btnThanhToan = null!;

        private readonly List<ChiTietHoaDonDTO> _gioHang = new();

        public FormBanHang()
        {
            InitializeCustomComponents();
            LoadKhachHang();
            LoadCatalog();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Bán Hàng & Lập Hóa Đơn";
            this.Size = new Size(1180, 720);
            this.BackColor = UITheme.Background;
            this.Padding = new Padding(12);

            // Container Panel chia 2 cột với khoảng cách (gap)
            // Cột Trái (Catalog): Width = 620
            var pnlLeftWrapper = new Panel
            {
                Dock = DockStyle.Left,
                Width = 630,
                BackColor = UITheme.Background,
                Padding = new Padding(0, 0, 10, 0)
            };

            var cardLeft = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            pnlLeftWrapper.Controls.Add(cardLeft);

            // Cột Phải (Giỏ hàng & Thanh toán): Dock = Fill
            var pnlRightWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background,
                Padding = new Padding(10, 0, 0, 0)
            };

            var cardRight = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                BorderColor = UITheme.Border,
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            pnlRightWrapper.Controls.Add(cardRight);

            // ==========================================
            // 1. NỘI DUNG CARD TRÁI: DANH MỤC SẢN PHẨM
            // ==========================================
            var lblCatalogTitle = new Label
            {
                Text = "📦  DANH MỤC ĐIỆN THOẠI TRONG KHO",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = 32
            };

            // Thanh tìm kiếm nhanh
            var pnlCatalogSearch = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(0, 4, 0, 8) };
            
            txtSearchDT = new TextBox
            {
                Location = new Point(0, 8),
                Width = 350,
                Font = new Font("Segoe UI", 10.5F),
                PlaceholderText = "Nhập tên điện thoại, hãng cần tìm..."
            };

            btnSearchDT = new RoundedButton
            {
                Text = "🔍 Tìm",
                Location = new Point(360, 6),
                Width = 80,
                Height = 32,
                BorderRadius = 6,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White
            };
            btnSearchDT.Click += (s, e) =>
            {
                dgvCatalog.DataSource = _dienThoaiBLL.Search(txtSearchDT.Text);
                FormatCatalogGrid();
            };

            btnReloadDT = new RoundedButton
            {
                Text = "Tất cả",
                Location = new Point(448, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub
            };
            btnReloadDT.Click += (s, e) =>
            {
                txtSearchDT.Text = "";
                LoadCatalog();
            };

            pnlCatalogSearch.Controls.Add(txtSearchDT);
            pnlCatalogSearch.Controls.Add(btnSearchDT);
            pnlCatalogSearch.Controls.Add(btnReloadDT);

            // Thanh thao tác Thêm vào giỏ
            var pnlAddToCart = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(0, 8, 0, 0)
            };

            var lblQty = new Label
            {
                Text = "Số lượng mua:",
                Location = new Point(4, 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UITheme.TextMain
            };

            numQuantity = new NumericUpDown
            {
                Location = new Point(130, 13),
                Width = 75,
                Minimum = 1,
                Maximum = 100,
                Value = 1,
                Font = new Font("Segoe UI", 10.5F)
            };

            btnAddToCart = new RoundedButton
            {
                Text = "➕  THÊM VÀO GIỎ HÀNG",
                Location = new Point(220, 8),
                Width = 240,
                Height = 42,
                BorderRadius = 8,
                BackColor = UITheme.Success,
                HoverColor = UITheme.SuccessHover,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnAddToCart.Click += BtnAddToCart_Click;

            pnlAddToCart.Controls.Add(lblQty);
            pnlAddToCart.Controls.Add(numQuantity);
            pnlAddToCart.Controls.Add(btnAddToCart);

            // Bảng sản phẩm DataGridView
            dgvCatalog = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvCatalog);

            cardLeft.Controls.Add(dgvCatalog);
            cardLeft.Controls.Add(pnlAddToCart);
            cardLeft.Controls.Add(pnlCatalogSearch);
            cardLeft.Controls.Add(lblCatalogTitle);

            // ==========================================
            // 2. NỘI DUNG CARD PHẢI: GIỎ HÀNG & THANH TOÁN
            // ==========================================
            var lblCartTitle = new Label
            {
                Text = "🛒  GIỎ HÀNG & THANH TOÁN TẠI QUẦY",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = 32
            };

            // Chọn khách hàng
            var pnlKH = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(0, 4, 0, 8) };
            var lblKH = new Label
            {
                Text = "Khách hàng (*):",
                Location = new Point(0, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UITheme.TextMain
            };

            cboKhachHang = new ComboBox
            {
                Location = new Point(135, 7),
                Width = 270,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnQuickAddKH = new RoundedButton
            {
                Text = "➕ Mới",
                Location = new Point(415, 6),
                Width = 75,
                Height = 32,
                BorderRadius = 6,
                BackColor = UITheme.PrimaryLight,
                HoverColor = Color.FromArgb(224, 231, 255),
                ForeColor = UITheme.Primary,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnQuickAddKH.Click += (s, e) =>
            {
                using var formKH = new FormKhachHang();
                formKH.ShowDialog();
                LoadKhachHang();
            };

            pnlKH.Controls.Add(lblKH);
            pnlKH.Controls.Add(cboKhachHang);
            pnlKH.Controls.Add(btnQuickAddKH);

            // Thao tác xóa giỏ hàng
            var pnlCartActions = new Panel { Dock = DockStyle.Bottom, Height = 45, Padding = new Padding(0, 6, 0, 6) };
            
            btnRemoveFromCart = new RoundedButton
            {
                Text = "🗑 Xóa món",
                Location = new Point(0, 6),
                Width = 100,
                Height = 30,
                BorderRadius = 6,
                BackColor = UITheme.DangerLight,
                HoverColor = Color.FromArgb(254, 226, 226),
                ForeColor = UITheme.Danger,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnRemoveFromCart.Click += BtnRemoveFromCart_Click;

            btnClearCart = new RoundedButton
            {
                Text = "Làm rỗng giỏ",
                Location = new Point(110, 6),
                Width = 110,
                Height = 30,
                BorderRadius = 6,
                BackColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = UITheme.TextSub,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnClearCart.Click += (s, e) => { _gioHang.Clear(); RefreshCartGrid(); };

            pnlCartActions.Controls.Add(btnRemoveFromCart);
            pnlCartActions.Controls.Add(btnClearCart);

            // Khung tổng kết thanh toán (Summary Card bo tròn nền xám nhạt)
            var pnlSummary = new RoundedPanel
            {
                Dock = DockStyle.Bottom,
                Height = 195,
                BorderRadius = 8,
                BorderColor = UITheme.Border,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(16, 12, 16, 12)
            };

            var lblT1 = new Label
            {
                Text = "Tổng tiền thanh toán:",
                Location = new Point(12, 16),
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = UITheme.TextMain
            };

            lblTongTien = new Label
            {
                Text = "0 đ",
                Location = new Point(200, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = UITheme.Danger
            };

            var lblT2 = new Label
            {
                Text = "Tiền khách đưa:",
                Location = new Point(12, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = UITheme.TextSub
            };

            numKhachDua = new NumericUpDown
            {
                Location = new Point(200, 52),
                Width = 200,
                Maximum = 1000000000,
                ThousandsSeparator = true,
                Value = 0,
                Font = new Font("Segoe UI", 10.5F)
            };
            numKhachDua.ValueChanged += (s, e) => CalculateChange();

            var lblT3 = new Label
            {
                Text = "Tiền thừa trả khách:",
                Location = new Point(12, 92),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = UITheme.TextSub
            };

            lblTienThua = new Label
            {
                Text = "0 đ",
                Location = new Point(200, 92),
                AutoSize = true,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = UITheme.Success
            };

            btnThanhToan = new RoundedButton
            {
                Text = "💳  THANH TOÁN & IN HÓA ĐƠN",
                Location = new Point(12, 132),
                Width = 460,
                Height = 48,
                BorderRadius = 8,
                BackColor = UITheme.Primary,
                HoverColor = UITheme.PrimaryHover,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            btnThanhToan.Click += BtnThanhToan_Click;

            pnlSummary.Controls.Add(lblT1);
            pnlSummary.Controls.Add(lblTongTien);
            pnlSummary.Controls.Add(lblT2);
            pnlSummary.Controls.Add(numKhachDua);
            pnlSummary.Controls.Add(lblT3);
            pnlSummary.Controls.Add(lblTienThua);
            pnlSummary.Controls.Add(btnThanhToan);

            // Bảng giỏ hàng
            dgvCart = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvCart);

            cardRight.Controls.Add(dgvCart);
            cardRight.Controls.Add(pnlCartActions);
            cardRight.Controls.Add(pnlSummary);
            cardRight.Controls.Add(pnlKH);
            cardRight.Controls.Add(lblCartTitle);

            // Thêm 2 wrapper vào form với docking chuẩn
            this.Controls.Add(pnlRightWrapper);
            this.Controls.Add(pnlLeftWrapper);

            pnlLeftWrapper.SendToBack();
        }

        private void LoadKhachHang()
        {
            var list = _khachHangBLL.GetAll();
            cboKhachHang.DataSource = new BindingSource(list, null);
            cboKhachHang.DisplayMember = "HoTen";
            cboKhachHang.ValueMember = "MaKH";
        }

        private void LoadCatalog()
        {
            var list = _dienThoaiBLL.GetAll();
            dgvCatalog.DataSource = list;
            FormatCatalogGrid();
        }

        private void FormatCatalogGrid()
        {
            if (dgvCatalog.Columns["MaDT"] != null) dgvCatalog.Columns["MaDT"].HeaderText = "Mã";
            if (dgvCatalog.Columns["TenDT"] != null) dgvCatalog.Columns["TenDT"].HeaderText = "Tên Điện Thoại";
            if (dgvCatalog.Columns["TenHang"] != null) dgvCatalog.Columns["TenHang"].HeaderText = "Hãng";
            if (dgvCatalog.Columns["GiaBan"] != null)
            {
                dgvCatalog.Columns["GiaBan"].HeaderText = "Đơn Giá (VNĐ)";
                dgvCatalog.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            }
            if (dgvCatalog.Columns["SoLuong"] != null) dgvCatalog.Columns["SoLuong"].HeaderText = "Tồn";
            if (dgvCatalog.Columns["GiaNhap"] != null) dgvCatalog.Columns["GiaNhap"].Visible = false;
            if (dgvCatalog.Columns["MaHang"] != null) dgvCatalog.Columns["MaHang"].Visible = false;
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (dgvCatalog.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một điện thoại trong danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = (DienThoaiDTO)dgvCatalog.CurrentRow.DataBoundItem;
            int buyQty = (int)numQuantity.Value;

            if (selected.SoLuong < buyQty)
            {
                MessageBox.Show($"Sản phẩm '{selected.TenDT}' chỉ còn {selected.SoLuong} máy trong kho!", "Tồn kho không đủ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _gioHang.Find(x => x.MaDT == selected.MaDT);
            if (existing != null)
            {
                if (selected.SoLuong < existing.SoLuong + buyQty)
                {
                    MessageBox.Show($"Số lượng trong giỏ ({existing.SoLuong}) + số lượng thêm ({buyQty}) vượt quá tồn kho ({selected.SoLuong})!", "Tồn kho không đủ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing.SoLuong += buyQty;
                existing.ThanhTien = existing.SoLuong * existing.DonGia;
            }
            else
            {
                _gioHang.Add(new ChiTietHoaDonDTO(selected.MaDT, selected.TenDT, buyQty, selected.GiaBan));
            }

            RefreshCartGrid();
        }

        private void BtnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null) return;
            var item = (ChiTietHoaDonDTO)dgvCart.CurrentRow.DataBoundItem;
            _gioHang.Remove(item);
            RefreshCartGrid();
        }

        private void RefreshCartGrid()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = new BindingSource(_gioHang, null);

            if (dgvCart.Columns["MaHD"] != null) dgvCart.Columns["MaHD"].Visible = false;
            if (dgvCart.Columns["MaDT"] != null) dgvCart.Columns["MaDT"].HeaderText = "Mã";
            if (dgvCart.Columns["TenDT"] != null) dgvCart.Columns["TenDT"].HeaderText = "Sản Phẩm";
            if (dgvCart.Columns["SoLuong"] != null) dgvCart.Columns["SoLuong"].HeaderText = "SL";
            if (dgvCart.Columns["DonGia"] != null)
            {
                dgvCart.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvCart.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvCart.Columns["ThanhTien"] != null)
            {
                dgvCart.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvCart.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }

            decimal total = 0;
            foreach (var item in _gioHang) total += item.ThanhTien;
            lblTongTien.Text = $"{total:N0} đ";

            if (numKhachDua.Value < total)
            {
                numKhachDua.Value = total;
            }
            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal total = 0;
            foreach (var item in _gioHang) total += item.ThanhTien;

            decimal pay = numKhachDua.Value;
            decimal change = pay - total;
            lblTienThua.Text = change >= 0 ? $"{change:N0} đ" : "Khách đưa thiếu tiền!";
            lblTienThua.ForeColor = change >= 0 ? UITheme.Success : UITheme.Danger;
        }

        private void BtnThanhToan_Click(object? sender, EventArgs e)
        {
            if (cboKhachHang.SelectedValue is not int maKH || maKH <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng mua hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng thêm sản phẩm vào giỏ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = SessionManager.CurrentNhanVien?.MaNV ?? 1;

            if (_banHangBLL.ThanhToan(maNV, maKH, _gioHang, out int maHD, out string error))
            {
                MessageBox.Show($"Thanh toán thành công!\nMã hóa đơn: HD{maHD:D5}\nTổng thanh toán: {lblTongTien.Text}", 
                    "Giao Dịch Hoàn Tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _gioHang.Clear();
                RefreshCartGrid();
                LoadCatalog();
            }
            else
            {
                MessageBox.Show("Thanh toán thất bại:\n" + error, "Lỗi Giao Dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
