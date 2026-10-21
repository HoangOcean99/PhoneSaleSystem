# 📖 HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN PHONE SALE SYSTEM (TỪ A - Z)

> **Tài liệu hướng dẫn chi tiết dành cho người mới**: Đi từ bước clone dự án từ GitHub về máy cho đến khi khởi chạy thành công ứng dụng trên máy tính của bạn.

---

## 📌 MỤC LỤC
1. [Yêu cầu môi trường (Prerequisites)](#1-yêu-cầu-môi-trường-prerequisites)
2. [Bước 1: Clone mã nguồn từ GitHub](#bước-1-clone-mã-nguồn-từ-github)
3. [Bước 2: Cấu hình file môi trường `.env`](#bước-2-cấu-hình-file-môi-trường-env)
4. [Bước 3: Khởi tạo Cơ sở Dữ liệu SQL Server](#bước-3-khởi-tạo-cơ-sở-dữ-liệu-sql-server)
5. [Bước 4: Biên dịch và Khởi chạy Ứng dụng](#bước-4-biên-dịch-và-khởi-chạy-ứng-dụng)
6. [Bước 5: Đăng nhập và Trải nghiệm Hệ thống](#bước-5-đăng-nhập-và-trải-nghiệm-hệ-thống)
7. [Xử lý các sự cố thường gặp (Troubleshooting)](#7-xử-lý-các-sự-cố-thường-gặp-troubleshooting)

---

## 1. YÊU CẦU MÔI TRƯỜNG (PREREQUISITES)

Trước khi bắt đầu, hãy đảm bảo máy tính của bạn đã cài đặt các công cụ sau:

| Công cụ | Phiên bản khuyến nghị | Mục đích |
| :--- | :--- | :--- |
| **Hệ điều hành** | Windows 10 hoặc Windows 11 (64-bit) | Chạy ứng dụng Windows Forms |
| **.NET SDK** | **.NET 8.0 SDK** (LTS) | Biên dịch và chạy mã nguồn C# |
| **Microsoft SQL Server** | SQL Server 2019 / 2022 hoặc **SQL Server Express** | Lưu trữ cơ sở dữ liệu `PhoneSaleDB` |
| **Công cụ quản lý SQL** | **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio** | Mở và thực thi các file kịch bản SQL |
| **IDE phát triển** | **Visual Studio 2022** *(Workload: .NET Desktop Development)* hoặc VS Code | Mở solution, debug và chỉnh sửa code |
| **Git** | Phiên bản mới nhất | Clone và đồng bộ mã nguồn |

> 💡 **Kiểm tra nhanh**: Mở PowerShell hoặc Command Prompt và gõ:
> ```powershell
> dotnet --version   # Kết quả cần hiển thị 8.0.xxx
> git --version      # Kiểm tra Git đã cài đặt
> ```

---

## BƯỚC 1: CLONE MÃ NGUỒN TỪ GITHUB

1. Mở **Terminal / PowerShell** hoặc **Git Bash** tại thư mục bạn muốn lưu dự án (ví dụ: ổ `D:\` hoặc `E:\`).
2. Gõ lệnh clone dự án về máy:
   ```powershell
   git clone <URL_REPO_GITHUB_CỦA_BẠN>
   ```
   *(Ví dụ: `git clone https://github.com/your-username/PhoneSaleSystem.git`)*

3. Di chuyển vào thư mục dự án vừa clone:
   ```powershell
   cd PhoneSaleSystem
   ```

---

## BƯỚC 2: CẤU HÌNH FILE MÔI TRƯỜNG `.env`

Để bảo mật, thông tin kết nối CSDL cá nhân không được đưa lên GitHub mà nằm trong file `.env` cục bộ trên máy bạn.

1. Tại thư mục gốc dự án (`PhoneSaleSystem/`), tạo một file mới đặt tên là **`.env`** (bằng cách copy từ file mẫu [`.env.example`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/.env.example)):
   - Trong PowerShell:
     ```powershell
     Copy-Item .env.example .env
     ```
   - Hoặc chuột phải vào `.env.example` ➔ Copy ➔ Paste ➔ Đổi tên thành `.env`.

2. Mở file `.env` vừa tạo và điền chuỗi kết nối phù hợp với máy của bạn:

   * **Trường hợp A (Khuyên dùng - Windows Authentication với SQLEXPRESS)**:
     ```env
     DB_CONNECTION_STRING="Server=.\SQLEXPRESS;Database=PhoneSaleDB;Trusted_Connection=True;TrustServerCertificate=True;"
     ```
   * **Trường hợp B (SQL Server mặc định không có đuôi SQLEXPRESS)**:
     ```env
     DB_CONNECTION_STRING="Server=.;Database=PhoneSaleDB;Trusted_Connection=True;TrustServerCertificate=True;"
     ```
   * **Trường hợp C (Dùng tài khoản SQL Server `sa` qua cổng 1433 hoặc Docker)**:
     ```env
     DB_CONNECTION_STRING="Server=127.0.0.1,1433;Database=PhoneSaleDB;User Id=sa;Password=mật_khẩu_sa_của_bạn;TrustServerCertificate=True;"
     ```

*(File `.env` đã được cấu hình trong `.gitignore` nên bạn hoàn toàn yên tâm không bao giờ sợ bị lộ mật khẩu khi commit code lên Git).*

---

## BƯỚC 3: KHỞI TẠO CƠ SỞ DỮ LIỆU SQL SERVER

1. Khởi động phần mềm **SSMS (SQL Server Management Studio)** hoặc **Azure Data Studio**.
2. Kết nối tới Server tương ứng bạn đã cấu hình ở Bước 2:
   - Server name: `.\SQLEXPRESS` (hoặc `.` hoặc `127.0.0.1,1433`).
   - Authentication: Chọn `Windows Authentication` (hoặc `SQL Server Authentication` nếu dùng user `sa`).
   - Nhấn **Connect**.

3. Mở và chạy lần lượt **3 file SQL** trong thư mục [`database/`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/database) theo đúng thứ tự:
   - 📄 **File 1**: [`database/01_CreateDatabase_Tables.sql`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/database/01_CreateDatabase_Tables.sql)
     - Nhấn **Execute (F5)** để tạo CSDL `PhoneSaleDB` và 7 bảng dữ liệu (`TaiKhoan`, `NhanVien`, `KhachHang`, `Hang`, `DienThoai`, `HoaDon`, `ChiTietHoaDon`).
   - 📄 **File 2**: [`database/02_StoredProcedures.sql`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/database/02_StoredProcedures.sql)
     - Nhấn **Execute (F5)** để tạo các Stored Procedure phục vụ báo cáo doanh thu & tồn kho.
   - 📄 **File 3**: [`database/03_SeedData.sql`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/database/03_SeedData.sql)
     - Nhấn **Execute (F5)** để nạp dữ liệu mẫu ban đầu (tài khoản admin/nhân viên, danh mục hãng, sản phẩm điện thoại và khách hàng).

> ✅ **Kiểm tra thành công**: Bạn mở tab truy vấn mới, gõ lệnh `SELECT * FROM PhoneSaleDB.dbo.TaiKhoan;` nếu thấy có tài khoản `admin` và `nhanvien1` là cơ sở dữ liệu đã sẵn sàng 100%!

---

## BƯỚC 4: BIÊN DỊCH VÀ KHỞI CHẠY ỨNG DỤNG

Bạn có thể chọn **1 trong 2 cách** dưới đây:

### Cách 1: Chạy bằng Visual Studio 2022 (Khuyên dùng)
1. Nhấp đúp vào file [`PhoneSaleSystem.sln`](file:///e:/L%C3%A0m%20H%E1%BB%99/PhoneSaleSystem/PhoneSaleSystem.sln) để mở toàn bộ dự án trong Visual Studio 2022.
2. Tại cửa sổ **Solution Explorer** (bên phải):
   - Mở thư mục `sourceCode`.
   - Click chuột phải vào project **`PhoneSaleSystem.GUI`** ➔ Chọn 👉 **Set as Startup Project** *(tên project sẽ được in đậm)*.
3. Nhấn phím **F5** trên bàn phím (hoặc bấm nút tam giác xanh **Start** trên thanh công cụ).
4. Ứng dụng sẽ tự động build các tầng Backend (DTO, DAL, BLL) và mở cửa sổ đăng nhập hiện đại lên màn hình!

### Cách 2: Chạy trực tiếp từ dòng lệnh Terminal (.NET CLI)
1. Mở PowerShell ngay tại thư mục gốc `PhoneSaleSystem`.
2. Gõ lệnh sau và nhấn **Enter**:
   ```powershell
   dotnet run --project sourceCode/PhoneSaleSystem.GUI/PhoneSaleSystem.GUI.csproj
   ```
3. Hệ thống sẽ tự động khôi phục NuGet package, biên dịch và bật ứng dụng lên ngay lập tức.

---

## BƯỚC 5: ĐĂNG NHẬP VÀ TRẢI NGHIỆM HỆ THỐNG

Khi cửa sổ đăng nhập **PhoneSale Pro** xuất hiện, sử dụng các tài khoản có sẵn:

| Tài khoản | Tên đăng nhập | Mật khẩu | Quyền hạn & Phạm vi truy cập |
| :--- | :--- | :--- | :--- |
| **Quản trị viên** | **`admin`** | **`123456`** | **Toàn quyền**: Bán hàng, Quản lý kho điện thoại, Khách hàng, Quản lý nhân viên & Phân quyền, Thống kê KPI doanh thu. |
| **Nhân viên thu ngân** | **`nhanvien1`** | **`123456`** | Bán hàng tại quầy, Xem tồn kho, Quản lý khách hàng, Tra cứu hóa đơn (Ẩn tính năng quản lý nhân sự & báo cáo doanh thu). |

---

## 7. XỬ LÝ CÁC SỰ CỐ THƯỜNG GẶP (TROUBLESHOOTING)

### Lỗi 1: `Instance failure` khi bấm nút Đăng nhập
* **Nguyên nhân**: Tên máy chủ SQL Server trong file `.env` không tồn tại hoặc dịch vụ SQL Server đang bị tắt.
* **Cách khắc phục**:
  1. Mở hộp thoại `Run` (`Windows + R`), gõ `services.msc` ➔ Tìm dịch vụ **SQL Server (SQLEXPRESS)** hoặc **SQL Server (MSSQLSERVER)** ➔ Chuột phải chọn **Start**.
  2. Mở file `.env` kiểm tra lại `Server=.\SQLEXPRESS` (nếu máy cài bản Express) hoặc `Server=.` (nếu cài bản mặc định).

### Lỗi 2: Visual Studio báo lỗi thiếu SDK hoặc TargetFramework `net8.0-windows`
* **Nguyên nhân**: Máy bạn chưa cài đặt .NET 8 SDK hoặc Visual Studio thiếu workload WinForms.
* **Cách khắc phục**: Mở **Visual Studio Installer** ➔ Chọn **Modify** phiên bản VS 2022 của bạn ➔ Tích chọn workload **.NET desktop development** và nhấn Install/Modify.

### Lỗi 3: `Login failed for user 'sa'`
* **Nguyên nhân**: Nhập sai mật khẩu tài khoản `sa` hoặc tài khoản `sa` chưa được kích hoạt trong SQL Server.
* **Cách khắc phục**: Đổi chuỗi kết nối sang dùng **Windows Authentication** bằng cách đặt `Trusted_Connection=True;` và bỏ phần `User Id=sa;Password=...` đi.

---

🎉 **Chúc bạn thực hiện thành công và có buổi báo cáo đồ án đạt kết quả cao nhất!**
