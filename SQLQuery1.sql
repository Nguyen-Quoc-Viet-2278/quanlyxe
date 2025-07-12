CREATE DATABASE QLYSach
GO

USE QLYSach;
GO

CREATE TABLE HangSanPham (
    MaHang INT PRIMARY KEY,
    TenHang NVARCHAR(100)
);

CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY,
	TenHang NVARCHAR(100),  -- Thêm cột TenHang tại đây
    TenSanPham NVARCHAR(100),
    Gia DECIMAL(10),
	SoLuong INT NOT NULL,
	HinhAnh NVARCHAR(255),
	ChiTiet NVARCHAR(100)
    
);
    

CREATE TABLE TaiKhoan (
    TenTaiKhoan VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(50),
    Email VARCHAR(100),
	Role NVARCHAR(50)
);

CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY IDENTITY(1,1),
    TenKhachHang NVARCHAR(100),
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(20),
	TaiKhoan NVARCHAR(100),
	MatKhau NVARCHAR(100),
	Role NVARCHAR(50)
);

CREATE TABLE NhanVien (
    MaNhanVien INT PRIMARY KEY IDENTITY(1,1),
    TenNhanVien NVARCHAR(100),
    GioiTinh NVARCHAR(10),
    NamSinh DATE,
    DiaChi VARCHAR(255),
    DienThoai VARCHAR(20),
    Email VARCHAR(100),
	TenTaiKhoan NVARCHAR(100),
	MatKhau NVARCHAR(100),
	Role NVARCHAR(50),
	
);

CREATE TABLE HoaDon (
    MaHoaDon INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    TenKhachHang NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(20) NOT NULL,
    DiaChi NVARCHAR(255) NOT NULL,
    TenSanPham NVARCHAR(100) NOT NULL,
    SoLuong INT NOT NULL,
    ThanhTien DECIMAL(10, 2) NOT NULL,
    NgayDatHang DATETIME DEFAULT GETDATE() -- Automatically records the date of the order
);



INSERT INTO HoaDon (TenKhachHang, DienThoai, DiaChi, TenSanPham, SoLuong, ThanhTien, NgayDatHang)
VALUES 
('Nguyễn Văn Nam', '0901234567', N'Số 1, Đường ABC, Quận 1', N'Theon S', 2, 26000000, '2024-10-02 17:05:54.280'),
(N'Trần Thị B', '0912345678', N'Số 2, Đường DEF, Quận 2', N'EVO200', 1, 20000000,'2024-10-02 17:05:54.280'),
(N'Lê Văn C', '0923456789', N'Số 3, Đường GHI, Quận 3', N'EVO200 Lite', 3, 45000000, '2024-10-02 17:05:54.280'),
(N'Phạm Văn A', N'0123456789', N'14/6, H.Bình Chánh', N'Aura S',1, 35000000, '2024-01-02'),
(N'Nguyễn Tâm Minh','0909321654', N'71/23, Q9', N'Aura S',2, 70000000, '2024-03-22'),
(N'Lê Anh Tài', '0333124578', N'55/4, Q1', N'Klara S 200', 1, 25000000, '2024-05-05'),
(N'Phạm Anh Tuấn', '0214677955',  N'12/21, P5, Q1', N'Theon S', 1, 13000000 , '2024-12-17'),
(N'Lê Hoàng Tuấn', '0912345678', N'10/4, Q5', 'EVO200',1, 20000000, '2024-09-03');


INSERT INTO HangSanPham (MaHang, TenHang)
VALUES 
(1, N'Nhà Xuất Bản Kim Đồng'),
(2, N'Nhà Xuất Bản Trẻ'),
(3, N'Nhà Xuất Bản Văn Học'),
(4, N'Nhà Xuất Bản Thế Giới'),
(5, N'Nhà Xuất Bản Lao Động'),
(6, N'Nhà Xuất Bản Phụ Nữ'),
(7, N'Nhà Xuất Bản Giáo Dục'),
(8, N'Nhà Xuất Bản Hội Nhà Văn'),
(9, N'Nhà Xuất Bản Tổng Hợp'),
(10,N'Nhà Xuất Bản Sống.');

INSERT INTO SanPham (MaSanPham, TenSanPham, Gia, TenHang, SoLuong, HinhAnh, ChiTiet)
VALUES
(1, N'Harry Potter và Hòn Đá Phù Thủy', 200000,N'Nhà Xuất Bản Kim Đồng', 50, 'hinh_1.jpg', N'Cuốn sách đầu tiên trong series Harry Potter.'),
(2, N'Bí Mật Của Những Hạt Mầm', 150000,N'Nhà Xuất Bản Trẻ', 30, 'hinh_2.jpg', N'Khám phá thế giới của những hạt mầm.'),
(3, N'Cô Gái Đến Từ Hôm Qua', 120000,N'Nhà Xuất Bản Văn Học', 40, 'hinh_3.jpg', N'Một tác phẩm nổi bật của nhà văn Nguyễn Nhật Ánh.'),
(4, N'Người Bán Hàng Vĩ Đại Nhất Thế Giới', 180000, N'Nhà Xuất Bản Thế Giới',25, 'hinh_4.jpg', N'Cuốn sách truyền cảm hứng về bán hàng.'),
(5, N'Chí Phèo', 100000, N'Nhà Xuất Bản Lao Động',60, 'hinh_5.jpg', N'Tác phẩm kinh điển của Nam Cao.'),
(6, N'Số Đỏ', 110000, N'Nhà Xuất Bản Phụ Nữ', 55, 'hinh_6.jpg', N'Một tác phẩm châm biếm xã hội của Vũ Trọng Phụng.'),
(7, N'Đắc Nhân Tâm', 250000, N'Nhà Xuất Bản Giáo Dục',20, 'hinh_7.jpg', N'Cuốn sách nổi tiếng về nghệ thuật giao tiếp.'),
(8, N'Kỹ Năng Đàm Phán', 220000, N'Nhà Xuất Bản Hội Nhà Văn',15, 'hinh_8.jpg', N'Cách để thành công trong các cuộc đàm phán.'),
(9, N'Tắt Đèn', 130000, N'Nhà Xuất Bản Tổng Hợp',45, 'hinh_9.jpg', N'Tác phẩm nổi bật của Ngô Tất Tố.'),
(10,N'Dế Mèn Phiêu Lưu Ký', 140000, N'Nhà Xuất Bản Sống.',35, 'hinh_10.jpg', N'Cuốn sách thiếu nhi kinh điển của Tô Hoài.');


INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, Email, Role)
VALUES 
(N'Viet', N'123', N'viet@gmail.com', N'admin');

INSERT INTO NhanVien (TenNhanVien, GioiTinh, NamSinh, DiaChi, DienThoai, Email, TenTaiKhoan, MatKhau, Role) VALUES
(N'Nguyễn Văn Nam', N'Nam', '1990-01-01', N'Hà Nội', '0123456788', 'nv1@example.com', 'admin1', 'adminpass1', N'Nhân Viên'),
(N'Trần Thị Linh', N'Nữ', '1992-02-02', N'Đà Nẵng', '0123456787', 'nv2@example.com', 'admin2', 'adminpass2', N'Nhân Viên'),
(N'Lê Văn Minh', N'Nam', '1988-03-03', 'TP.HCM', '0123456786', 'nv3@example.com', 'admin1', 'adminpass1', N'Nhân Viên'),
(N'Phạm Thị Nu', N'Nữ', '1995-04-04', N'Hải Phòng', '0123456785', 'nv4@example.com', 'admin2', 'adminpass2', N'Nhân Viên'),
(N'Nguyễn Văn Ô', 'Nam', '1993-05-05', N'Cần Thơ', '0123456784', 'nv5@example.com', 'admin1', 'adminpass1', N'Nhân Viên'),
(N'Trần Thị Thúy', N'Nữ', '1991-06-06', N'Nha Trang', '0123456783', 'nv6@example.com', 'admin2', 'adminpass2', N'Nhân Viên'),
(N'Lê Văn Qúy', 'Nam', '1994-07-07', N'Vũng Tàu', '0123456782', 'nv7@example.com', 'admin1', 'adminpass1', N'Nhân Viên'),
(N'Phạm Thị Hân',N'Nữ', '1989-08-08', N'Quy Nhơn', '0123456781', 'nv8@example.com', 'admin2', 'adminpass2', N'Nhân Viên'),
(N'Nguyễn Văn Sử', N'Nam', '1985-09-09', N'Đà Lạt', '0123456780', 'nv9@example.com', 'admin1', 'adminpass1', N'Nhân Viên'),
(N'Trần Thị Trinh', N'Nữ', '1996-10-10', N'Huế', '0123456799', 'nv10@example.com', 'admin2', 'adminpass2', N'Nhân Viên');

INSERT INTO KhachHang ( TenKhachHang, DiaChi, DienThoai,TaiKhoan, MatKhau, Role)
VALUES 
(N'Phạm Văn A', N'14/6, H.Bình Chánh', '0123456789',N'kh01', N'kh01',N'KhachHang'),
(N'Nguyễn Tâm Minh', N'71/23, Q9', '0909321654',N'nguyentamminh', N'kh01',N'KhachHang'),
(N'Lê Anh Tài', N'55/4, Q1', '0333124578',N'leanhtai', N'leanhtai',N'KhachHang'),
(N'Phạm Anh Tuấn', N'12/21, P5, Q1', '0214677955',N'phamanhtuan', N'kh01',N'KhachHang'),
(N'Lê Hoàng Tuấn', N'10/4, Q5', '0912345678',N'lehoangtuan', N'kh01',N'KhachHang');

SELECT * FROM TaiKhoan;
SELECT * FROM SanPham;
SELECT * FROM NhanVien;
SELECT * FROM KhachHang;
SELECT * FROM HoaDon;



-- Retrieve the maximum existing MaSanPham to avoid duplicates
DECLARE @MaxMaSanPham INT;
SELECT @MaxMaSanPham = ISNULL(MAX(MaSanPham), 0) FROM SanPham;

-- Insert new SanPham records with unique MaSanPham
INSERT INTO SanPham (MaSanPham, TenHang, TenSanPham, Gia)
SELECT 
    @MaxMaSanPham + ROW_NUMBER() OVER (ORDER BY MaHang) AS NewMaSanPham,
    TenHang,
    N'Sản phẩm ' + CAST(MaHang AS NVARCHAR) AS TenSanPham,
    100000 AS Gia
FROM HangSanPham
WHERE TenHang NOT IN (SELECT TenHang FROM SanPham);


