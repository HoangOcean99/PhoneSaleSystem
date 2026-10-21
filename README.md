# HỆ THỐNG QUẢN LÝ BÁN ĐIỆN THOẠI (PHONE SALE SYSTEM)

Hệ thống quản lý và bán điện thoại được thiết kế và xây dựng theo chuẩn kiến trúc 3 lớp (3-Tier / N-Tier Architecture) bằng ngôn ngữ **C# (.NET 8)**, giao diện **Windows Forms**, cơ sở dữ liệu **Microsoft SQL Server** và truy xuất dữ liệu thông qua **ADO.NET** (sử dụng `Microsoft.Data.SqlClient`).

---

## 1. TECHSTACK CỦA DỰ ÁN

| Thành phần | Công nghệ / Thư viện | Vai trò |
| :--- | :--- | :--- |
| **Ngôn ngữ** | C# 12 / .NET 8 (LTS) | Xử lý logic và nghiệp vụ toàn hệ thống |
| **Giao diện (UI)** | Windows Forms (WinForms) | Giao diện tương tác người dùng hiện đại, trực quan |
| **Cơ sở dữ liệu** | Microsoft SQL Server | Lưu trữ dữ liệu quan hệ, đảm bảo toàn vẹn dữ liệu |
| **Truy xuất dữ liệu** | ADO.NET (`Microsoft.Data.SqlClient`) | Kết nối, thực thi Stored Procedure, Transaction, chống SQL Injection |
| **Môi trường phát triển** | Visual Studio 2022 / VS Code / .NET CLI | Biên dịch, chạy và gỡ lỗi hệ thống |

---

## 2. KIẾN TRÚC HỆ THỐNG (3-TIER ARCHITECTURE)

Hệ thống được tổ chức phân tầng rõ ràng nhằm đảm bảo tính độc lập, khả năng mở rộng và dễ bảo trì. Chi tiết sơ đồ kiến trúc và luồng dữ liệu xem tại [document/ARCHITECTURE.md](file:///E:/Làm%20Hộ/PhoneSaleSystem/document/ARCHITECTURE.md).

```
PhoneSaleSystem/
│
├── PhoneSaleSystem.sln                     # Solution file cho Visual Studio
├── PhoneSaleSystem.slnx                    # Modern Solution file (.NET 8/10)
├── README.md                               # Hướng dẫn khởi chạy, cấu hình và sử dụng
│
├── database/                               # Thư mục kịch bản Cơ sở dữ liệu SQL Server
│   ├── 01_CreateDatabase_Tables.sql        # Tạo CSDL PhoneSaleDB và 7 bảng dữ liệu
│   ├── 02_StoredProcedures.sql             # Các Stored Procedure cho thống kê, báo cáo
│   └── 03_SeedData.sql                     # Dữ liệu mẫu (Admin, Hãng, Điện thoại, Khách hàng)
│
├── document/                               # Toàn bộ tài liệu dự án
│   ├── ARCHITECTURE.md                     # Tài liệu thiết kế tổng thể kiến trúc hệ thống
│   ├── BAOCAO_HETHONGBANDIENTHOAI.md       # Báo cáo đồ án chi tiết
│   ├── C#_BAOCAOLON_HETHONGBANDIENTHOAI.docx # File Word gốc của dự án
│   └── images/                             # Thư mục hình ảnh minh họa báo cáo
│       ├── image1.png                      # Logo trường ĐH Công nghệ GTVT
│       ├── image2.png                      # Sơ đồ quy trình bán hàng
│       └── image3.png                      # Sơ đồ mô hình đề xuất
│
└── sourceCode/                             # Mã nguồn phân tầng theo chuẩn kiến trúc
    │
    ├── PhoneSaleSystem.DTO/                # [Tầng DTO - Data Transfer Objects / Entities]
    │   ├── TaiKhoanDTO.cs                  # Thực thể Tài khoản (MaTK, TenDangNhap, MatKhau, Quyen)
    │   ├── NhanVienDTO.cs                  # Thực thể Nhân viên
    │   ├── KhachHangDTO.cs                 # Thực thể Khách hàng
    │   ├── HangDTO.cs                      # Thực thể Hãng sản xuất (Apple, Samsung, Xiaomi...)
    │   ├── DienThoaiDTO.cs                 # Thực thể Sản phẩm Điện thoại & Tồn kho
    │   ├── HoaDonDTO.cs                    # Thực thể Hóa đơn bán hàng
    │   ├── ChiTietHoaDonDTO.cs             # Thực thể Chi tiết từng món trong hóa đơn
    │   └── ThongKeDTO.cs                   # Thực thể phục vụ báo cáo doanh thu & tồn kho
    │
    ├── PhoneSaleSystem.DAL/                # [Tầng DAL - Data Access Layer (ADO.NET)]
    │   ├── DatabaseHelper.cs               # Quản lý ConnectionString, ExecuteQuery, ExecuteNonQuery, Transaction
    │   ├── TaiKhoanDAL.cs                  # Đăng nhập, CRUD tài khoản
    │   ├── NhanVienDAL.cs                  # Truy vấn, thêm, sửa, xóa nhân viên
    │   ├── KhachHangDAL.cs                 # Truy vấn, thêm, sửa, xóa khách hàng
    │   ├── HangDAL.cs                      # Quản lý hãng điện thoại
    │   ├── DienThoaiDAL.cs                 # Truy vấn, tìm kiếm, lọc theo hãng, cập nhật tồn kho
    │   ├── HoaDonDAL.cs                    # Quản lý hóa đơn bán hàng
    │   ├── ChiTietHoaDonDAL.cs             # Quản lý dòng chi tiết hóa đơn
    │   └── ThongKeDAL.cs                   # Truy vấn doanh thu theo ngày, top sản phẩm, tồn kho
    │
    ├── PhoneSaleSystem.BLL/                # [Tầng BLL - Business Logic Layer]
    │   ├── TaiKhoanBLL.cs                  # Kiểm tra đăng nhập, đổi mật khẩu, phân quyền
    │   ├── NhanVienBLL.cs                  # Ràng buộc nghiệp vụ nhân viên
    │   ├── KhachHangBLL.cs                 # Ràng buộc nghiệp vụ khách hàng
    │   ├── HangBLL.cs                      # Xử lý logic hãng điện thoại
    │   ├── DienThoaiBLL.cs                 # Nghiệp vụ kiểm tra giá, tồn kho, thêm/sửa/xóa máy
    │   ├── BanHangBLL.cs                   # Nghiệp vụ bán hàng trung tâm: kiểm tra kho, tính tiền, trừ kho, Transaction
    │   ├── HoaDonBLL.cs                    # Nghiệp vụ tra cứu hóa đơn và chi tiết
    │   └── ThongKeBLL.cs                   # Nghiệp vụ tính toán doanh thu, top sản phẩm bán chạy
    │
    └── PhoneSaleSystem.GUI/                # [Tầng GUI - Presentation Layer (WinForms)]
        ├── Program.cs                      # Điểm khởi chạy ứng dụng
        ├── App.config                      # Cấu hình chuỗi kết nối SQL Server
        ├── Helpers/
        │   ├── SessionManager.cs           # Quản lý phiên đăng nhập hiện tại và quyền truy cập
        │   └── UITheme.cs                  # Bảng màu hiện đại (Primary Navy, Accent Blue) & style Controls
        └── Forms/
            ├── FormLogin.cs                # [UC01] Màn hình Đăng nhập hệ thống
            ├── FormMain.cs                 # Màn hình chính Dashboard với Sidebar điều hướng mượt mà
            ├── FormDienThoai.cs            # [UC02] Quản lý điện thoại, giá bán, tồn kho, lọc theo hãng
            ├── FormKhachHang.cs            # [UC03] Quản lý thông tin và lịch sử khách hàng
            ├── FormNhanVien.cs             # [UC04] Quản lý nhân viên và cấp phát tài khoản
            ├── FormBanHang.cs              # [UC05, UC06] Nghiệp vụ bán hàng, giỏ hàng, thanh toán & in hóa đơn
            ├── FormHoaDon.cs               # [UC07] Tra cứu, lọc hóa đơn theo ngày, xem chi tiết
            └── FormThongKe.cs              # [UC08] Dashboard thống kê KPI, biểu đồ doanh thu & tồn kho
```

---

## 3. HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

### Bước 1: Thiết lập Cơ sở dữ liệu (SQL Server)
1. Mở **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio**.
2. Kết nối tới SQL Server instance của bạn (ví dụ: `.` hoặc `localhost` hoặc `SQLEXPRESS`).
3. Mở và chạy lần lượt 3 file script trong thư mục `database/`:
   - `01_CreateDatabase_Tables.sql` (Tạo database `PhoneSaleDB` và 7 bảng)
   - `02_StoredProcedures.sql` (Tạo các Stored Procedure)
   - `03_SeedData.sql` (Chèn dữ liệu mẫu ban đầu)
### Bước 2: Cấu hình chuỗi kết nối qua file `.env` (Không bị lộ mật khẩu khi commit)
Hệ thống hỗ trợ đọc cấu hình trực tiếp từ file `.env` ở thư mục gốc (đã được cấu hình trong `.gitignore` để không bị commit lên GitHub/Git):
1. Mở file `.env` tại thư mục gốc của dự án (hoặc sao chép từ `.env.example`).
2. Tùy chỉnh chuỗi kết nối SQL Server của máy bạn:
```env
DB_CONNECTION_STRING="Server=.\\SQLEXPRESS;Database=PhoneSaleDB;Trusted_Connection=True;TrustServerCertificate=True;"
```
- Nếu dùng SQL Authentication (user `sa`):
```env
DB_CONNECTION_STRING="Server=.\\SQLEXPRESS;Database=PhoneSaleDB;User Id=sa;Password=mat_khau;TrustServerCertificate=True;"
```
*(Nếu không có file `.env`, hệ thống sẽ tự động dùng giá trị mặc định trong mã nguồn)*.

### Bước 3: Biên dịch và chạy ứng dụng
- **Cách 1 (Bằng Visual Studio):** Mở `PhoneSaleSystem.sln`, chọn project `PhoneSaleSystem.GUI` làm Startup Project và nhấn `F5`.
- **Cách 2 (Bằng Terminal / .NET CLI):**
  ```powershell
  dotnet run --project sourceCode/PhoneSaleSystem.GUI/PhoneSaleSystem.GUI.csproj
  ```

---

## 4. TÀI KHOẢN MẪU ĐĂNG NHẬP

| Tên đăng nhập | Mật khẩu | Quyền hạn | Mô tả phạm vi truy cập |
| :--- | :--- | :--- | :--- |
| **admin** | `123456` | **Admin** | Toàn quyền: Điện thoại, Bán hàng, Khách hàng, Nhân viên, Hóa đơn, Thống kê |
| **nhanvien1** | `123456` | **NhanVien** | Bán hàng, Quản lý khách hàng, Xem danh mục điện thoại, Lập hóa đơn |
