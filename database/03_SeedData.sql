-- ====================================================================
-- HỆ THỐNG QUẢN LÝ BÁN ĐIỆN THOẠI
-- File: 03_SeedData.sql (Chuẩn duy nhất)
-- Mô tả: Dữ liệu mẫu khởi tạo cho hệ thống (An toàn, có thể chạy nhiều lần)
-- ====================================================================

USE PhoneSaleDB;
GO

PRINT N'>> Bắt đầu nạp dữ liệu mẫu vào PhoneSaleDB...';
GO

-- 1. HÃNG ĐIỆN THOẠI
IF NOT EXISTS (SELECT 1 FROM dbo.Hang WHERE TenHang = N'Apple')
    INSERT INTO dbo.Hang (TenHang) VALUES (N'Apple');

IF NOT EXISTS (SELECT 1 FROM dbo.Hang WHERE TenHang = N'Samsung')
    INSERT INTO dbo.Hang (TenHang) VALUES (N'Samsung');

IF NOT EXISTS (SELECT 1 FROM dbo.Hang WHERE TenHang = N'Xiaomi')
    INSERT INTO dbo.Hang (TenHang) VALUES (N'Xiaomi');

IF NOT EXISTS (SELECT 1 FROM dbo.Hang WHERE TenHang = N'OPPO')
    INSERT INTO dbo.Hang (TenHang) VALUES (N'OPPO');

IF NOT EXISTS (SELECT 1 FROM dbo.Hang WHERE TenHang = N'Vivo')
    INSERT INTO dbo.Hang (TenHang) VALUES (N'Vivo');
GO

-- 2. TÀI KHOẢN HỆ THỐNG (Mật khẩu: 123456)
IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'admin')
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen) VALUES ('admin', '123456', 'Admin');

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'nhanvien1')
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen) VALUES ('nhanvien1', '123456', 'NhanVien');

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = 'nhanvien2')
    INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen) VALUES ('nhanvien2', '123456', 'NhanVien');
GO

-- 3. NHÂN VIÊN (Gán liên kết với Tài khoản tương ứng)
DECLARE @MaTK_Admin INT = (SELECT MaTK FROM dbo.TaiKhoan WHERE TenDangNhap = 'admin');
DECLARE @MaTK_NV1 INT = (SELECT MaTK FROM dbo.TaiKhoan WHERE TenDangNhap = 'nhanvien1');
DECLARE @MaTK_NV2 INT = (SELECT MaTK FROM dbo.TaiKhoan WHERE TenDangNhap = 'nhanvien2');

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE HoTen = N'Nguyễn Quản Trị')
    INSERT INTO dbo.NhanVien (HoTen, SDT, DiaChi, ChucVu, MaTK)
    VALUES (N'Nguyễn Quản Trị', '0901234567', N'Hà Nội', N'Quản lý hệ thống', @MaTK_Admin);

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE HoTen = N'Trần Bán Hàng')
    INSERT INTO dbo.NhanVien (HoTen, SDT, DiaChi, ChucVu, MaTK)
    VALUES (N'Trần Bán Hàng', '0912345678', N'Hà Nội', N'Nhân viên bán hàng', @MaTK_NV1);

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien WHERE HoTen = N'Lê Thu Ngân')
    INSERT INTO dbo.NhanVien (HoTen, SDT, DiaChi, ChucVu, MaTK)
    VALUES (N'Lê Thu Ngân', '0987654321', N'Hà Đông, Hà Nội', N'Nhân viên thu ngân', @MaTK_NV2);
GO

-- 4. KHÁCH HÀNG
IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE SDT = '0988111222')
    INSERT INTO dbo.KhachHang (HoTen, SDT, DiaChi) VALUES (N'Nguyễn Văn An', '0988111222', N'Thanh Xuân, Hà Nội');

IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE SDT = '0977333444')
    INSERT INTO dbo.KhachHang (HoTen, SDT, DiaChi) VALUES (N'Trần Thị Bình', '0977333444', N'Cầu Giấy, Hà Nội');

IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE SDT = '0966555666')
    INSERT INTO dbo.KhachHang (HoTen, SDT, DiaChi) VALUES (N'Lê Hoàng Cường', '0966555666', N'Đống Đa, Hà Nội');

IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE SDT = '0911777888')
    INSERT INTO dbo.KhachHang (HoTen, SDT, DiaChi) VALUES (N'Phạm Thu Dung', '0911777888', N'Hoàng Mai, Hà Nội');
GO

-- 5. ĐIỆN THOẠI (Lấy MaHang động để tránh lệch ID)
DECLARE @Apple INT = (SELECT MaHang FROM dbo.Hang WHERE TenHang = N'Apple');
DECLARE @Samsung INT = (SELECT MaHang FROM dbo.Hang WHERE TenHang = N'Samsung');
DECLARE @Xiaomi INT = (SELECT MaHang FROM dbo.Hang WHERE TenHang = N'Xiaomi');
DECLARE @OPPO INT = (SELECT MaHang FROM dbo.Hang WHERE TenHang = N'OPPO');
DECLARE @Vivo INT = (SELECT MaHang FROM dbo.Hang WHERE TenHang = N'Vivo');

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'iPhone 15 128GB')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'iPhone 15 128GB', @Apple, '6GB', '128GB', N'Xanh dương', 17500000, 21990000, 15, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'iPhone 15 Pro Max 256GB')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'iPhone 15 Pro Max 256GB', @Apple, '8GB', '256GB', N'Titan tự nhiên', 26000000, 29990000, 8, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'Samsung Galaxy S24 Ultra')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'Samsung Galaxy S24 Ultra', @Samsung, '12GB', '256GB', N'Xám Titan', 23000000, 27490000, 12, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'Samsung Galaxy A55 5G')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'Samsung Galaxy A55 5G', @Samsung, '8GB', '128GB', N'Xanh Iceblue', 7200000, 9690000, 20, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'Xiaomi 14 5G 256GB')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'Xiaomi 14 5G 256GB', @Xiaomi, '12GB', '256GB', N'Đen', 16000000, 19990000, 10, 18);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'Redmi Note 13 Pro')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'Redmi Note 13 Pro', @Xiaomi, '8GB', '128GB', N'Tím', 5500000, 6990000, 25, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'OPPO Reno11 5G')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'OPPO Reno11 5G', @OPPO, '8GB', '256GB', N'Xanh biển', 8500000, 10990000, 14, 12);

IF NOT EXISTS (SELECT 1 FROM dbo.DienThoai WHERE TenDT = N'Vivo V30e 5G')
    INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
    VALUES (N'Vivo V30e 5G', @Vivo, '8GB', '256GB', N'Nâu', 7500000, 9490000, 16, 12);
GO

-- 6. HÓA ĐƠN & CHI TIẾT HÓA ĐƠN MẪU (Cho báo cáo thống kê ban đầu)
IF NOT EXISTS (SELECT 1 FROM dbo.HoaDon)
BEGIN
    DECLARE @MaNV_BanHang INT = (SELECT TOP 1 MaNV FROM dbo.NhanVien ORDER BY MaNV);
    DECLARE @MaKH1 INT = (SELECT TOP 1 MaKH FROM dbo.KhachHang ORDER BY MaKH);
    DECLARE @MaKH2 INT = (SELECT TOP 1 MaKH FROM dbo.KhachHang WHERE MaKH <> @MaKH1 ORDER BY MaKH);

    DECLARE @MaDT_iPhone15 INT = (SELECT MaDT FROM dbo.DienThoai WHERE TenDT = N'iPhone 15 128GB');
    DECLARE @MaDT_S24 INT = (SELECT MaDT FROM dbo.DienThoai WHERE TenDT = N'Samsung Galaxy S24 Ultra');

    -- Hóa đơn 1
    INSERT INTO dbo.HoaDon (MaNV, MaKH, NgayLap, TongTien)
    VALUES (@MaNV_BanHang, @MaKH1, DATEADD(DAY, -2, GETDATE()), 21990000);
    DECLARE @MaHD1 INT = SCOPE_IDENTITY();

    IF (@MaHD1 IS NOT NULL AND @MaDT_iPhone15 IS NOT NULL)
    BEGIN
        INSERT INTO dbo.ChiTietHoaDon (MaHD, MaDT, SoLuong, DonGia, ThanhTien)
        VALUES (@MaHD1, @MaDT_iPhone15, 1, 21990000, 21990000);
    END

    -- Hóa đơn 2
    INSERT INTO dbo.HoaDon (MaNV, MaKH, NgayLap, TongTien)
    VALUES (@MaNV_BanHang, @MaKH2, DATEADD(DAY, -1, GETDATE()), 27490000);
    DECLARE @MaHD2 INT = SCOPE_IDENTITY();

    IF (@MaHD2 IS NOT NULL AND @MaDT_S24 IS NOT NULL)
    BEGIN
        INSERT INTO dbo.ChiTietHoaDon (MaHD, MaDT, SoLuong, DonGia, ThanhTien)
        VALUES (@MaHD2, @MaDT_S24, 1, 27490000, 27490000);
    END
END
GO

PRINT N'>> [HOÀN TẤT] Nạp toàn bộ dữ liệu mẫu PhoneSaleDB thành công!';
GO
