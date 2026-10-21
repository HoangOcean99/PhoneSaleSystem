-- =============================================
-- HỆ THỐNG QUẢN LÝ BÁN ĐIỆN THOẠI
-- File: 01_CreateDatabase_Tables.sql
-- Mô tả: Tạo cơ sở dữ liệu và các bảng theo báo cáo đồ án
-- =============================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PhoneSaleDB')
BEGIN
    CREATE DATABASE PhoneSaleDB;
END
GO

USE PhoneSaleDB;
GO

-- 1. BẢNG TAIKHOAN
IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL
    DROP TABLE dbo.TaiKhoan;
GO

CREATE TABLE dbo.TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen VARCHAR(20) NOT NULL DEFAULT 'NhanVien' -- 'Admin', 'NhanVien'
);
GO

-- 2. BẢNG NHANVIEN
IF OBJECT_ID(N'dbo.NhanVien', N'U') IS NOT NULL
    DROP TABLE dbo.NhanVien;
GO

CREATE TABLE dbo.NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NULL,
    DiaChi NVARCHAR(200) NULL,
    ChucVu NVARCHAR(50) NULL,
    MaTK INT NULL UNIQUE,
    CONSTRAINT FK_NhanVien_TaiKhoan FOREIGN KEY (MaTK) REFERENCES dbo.TaiKhoan(MaTK) ON DELETE SET NULL
);
GO

-- 3. BẢNG KHACHHANG
IF OBJECT_ID(N'dbo.KhachHang', N'U') IS NOT NULL
    DROP TABLE dbo.KhachHang;
GO

CREATE TABLE dbo.KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(200) NULL
);
GO

-- 4. BẢNG HANG
IF OBJECT_ID(N'dbo.Hang', N'U') IS NOT NULL
    DROP TABLE dbo.Hang;
GO

CREATE TABLE dbo.Hang (
    MaHang INT IDENTITY(1,1) PRIMARY KEY,
    TenHang NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- 5. BẢNG DIENTHOAI
IF OBJECT_ID(N'dbo.DienThoai', N'U') IS NOT NULL
    DROP TABLE dbo.DienThoai;
GO

CREATE TABLE dbo.DienThoai (
    MaDT INT IDENTITY(1,1) PRIMARY KEY,
    TenDT NVARCHAR(100) NOT NULL,
    MaHang INT NOT NULL,
    RAM VARCHAR(10) NULL,
    BoNho VARCHAR(20) NULL,
    MauSac NVARCHAR(30) NULL,
    GiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
    GiaBan DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuong INT NOT NULL DEFAULT 0,
    BaoHanh INT NOT NULL DEFAULT 12, -- số tháng bảo hành
    CONSTRAINT FK_DienThoai_Hang FOREIGN KEY (MaHang) REFERENCES dbo.Hang(MaHang) ON DELETE NO ACTION
);
GO

-- 6. BẢNG HOADON
IF OBJECT_ID(N'dbo.HoaDon', N'U') IS NOT NULL
    DROP TABLE dbo.HoaDon;
GO

CREATE TABLE dbo.HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT NOT NULL,
    MaKH INT NOT NULL,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (MaNV) REFERENCES dbo.NhanVien(MaNV),
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES dbo.KhachHang(MaKH)
);
GO

-- 7. BẢNG CHITIETHOADON
IF OBJECT_ID(N'dbo.ChiTietHoaDon', N'U') IS NOT NULL
    DROP TABLE dbo.ChiTietHoaDon;
GO

CREATE TABLE dbo.ChiTietHoaDon (
    MaHD INT NOT NULL,
    MaDT INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHD, MaDT),
    CONSTRAINT FK_ChiTietHoaDon_HoaDon FOREIGN KEY (MaHD) REFERENCES dbo.HoaDon(MaHD) ON DELETE CASCADE,
    CONSTRAINT FK_ChiTietHoaDon_DienThoai FOREIGN KEY (MaDT) REFERENCES dbo.DienThoai(MaDT)
);
GO
