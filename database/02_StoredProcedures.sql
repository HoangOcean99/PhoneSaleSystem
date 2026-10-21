-- =============================================
-- HỆ THỐNG QUẢN LÝ BÁN ĐIỆN THOẠI
-- File: 02_StoredProcedures.sql
-- Mô tả: Các Stored Procedures cho báo cáo, thống kê và nghiệp vụ bán hàng
-- =============================================

USE PhoneSaleDB;
GO

-- 1. SP: Thống kê doanh thu theo khoảng thời gian
CREATE OR ALTER PROCEDURE sp_ThongKeDoanhThu
    @TuNgay DATETIME,
    @DenNgay DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CAST(NgayLap AS DATE) AS Ngay,
        COUNT(MaHD) AS SoHoaDon,
        SUM(TongTien) AS DoanhThu
    FROM dbo.HoaDon
    WHERE NgayLap >= @TuNgay AND NgayLap <= @DenNgay
    GROUP BY CAST(NgayLap AS DATE)
    ORDER BY Ngay ASC;
END
GO

-- 2. SP: Thống kê top sản phẩm bán chạy
CREATE OR ALTER PROCEDURE sp_TopSanPhamBanChay
    @Top INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Top)
        dt.MaDT,
        dt.TenDT,
        h.TenHang,
        SUM(ct.SoLuong) AS TongDaBan,
        SUM(ct.ThanhTien) AS TongDoanhThu
    FROM dbo.ChiTietHoaDon ct
    INNER JOIN dbo.DienThoai dt ON ct.MaDT = dt.MaDT
    INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
    GROUP BY dt.MaDT, dt.TenDT, h.TenHang
    ORDER BY TongDaBan DESC;
END
GO

-- 3. SP: Báo cáo tồn kho
CREATE OR ALTER PROCEDURE sp_BaoCaoTonKho
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        dt.MaDT,
        dt.TenDT,
        h.TenHang,
        dt.RAM,
        dt.BoNho,
        dt.MauSac,
        dt.GiaBan,
        dt.SoLuong AS TonKho,
        CASE 
            WHEN dt.SoLuong <= 5 THEN N'Sắp hết hàng'
            WHEN dt.SoLuong = 0 THEN N'Hết hàng'
            ELSE N'Còn hàng'
        END AS TrangThai
    FROM dbo.DienThoai dt
    INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
    ORDER BY dt.SoLuong ASC;
END
GO
