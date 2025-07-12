CREATE DATABASE QLYXE
GO

USE QLYXE;
GO

CREATE TABLE HangSanPham (
    MaHang INT PRIMARY KEY,
    TenHang NVARCHAR(100)
);

CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY,
	TenHang NVARCHAR(100),  -- Thêm cột TenHang tại đây
    TenSanPham NVARCHAR(100),
    Gia DECIMAL(10)
    
);
    

CREATE TABLE TaiKhoan (
    TenTaiKhoan VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(50),
    Email VARCHAR(100)
);

CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY,
    TenKhachHang NVARCHAR(100),
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(20)
);

CREATE TABLE NhanVien (
    MaNhanVien INT PRIMARY KEY,
    TenNhanVien NVARCHAR(100),
    GioiTinh NVARCHAR(10),
    NamSinh DATE,
    DiaChi VARCHAR(255),
    DienThoai VARCHAR(20),
    Email VARCHAR(100)
);

INSERT INTO HangSanPham (MaHang, TenHang)
VALUES 
(1, N'HANG XE A'), 
(2, N'HANG XE B'); 

INSERT INTO SanPham (MaSanPham, TenSanPham, Gia, TenHang)
VALUES (1, N'Sản phẩm A', 100000, N'HANG XE A');  -- Chèn dữ liệu mẫu


INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, Email)
VALUES (N'Lai', N'123', N'lai@gmail.com');

INSERT INTO NhanVien (MaNhanVien, TenNhanVien, GioiTinh, NamSinh, DiaChi, DienThoai, Email)
VALUES (1, N'Phạm Văn C', N'Nam', '1992-07-20', N'19/3C, Q3', '0912345678', 'phamc@gmail.com');

INSERT INTO KhachHang (MaKhachHang, TenKhachHang, DiaChi, DienThoai)
VALUES (1, N'Phạm Văn A', N'19/3C, Q3', '0912345678');

SELECT * FROM TaiKhoan;
SELECT * FROM SanPham;
SELECT * FROM NhanVien;
SELECT * FROM KhachHang;



INSERT INTO SanPham (MaSanPham, TenHang, TenSanPham, Gia)
SELECT 
    ROW_NUMBER() OVER (ORDER BY MaHang) AS MaSanPham,  -- Tạo mã sản phẩm tự động
    TenHang,  -- Lấy tên hãng
    N'Sản phẩm ' + CAST(MaHang AS NVARCHAR) AS TenSanPham,  -- Tạo tên sản phẩm
    100000 AS Gia  -- Giá mặc định
FROM HangSanPham;