# BÁO CÁO BÀI TẬP THỰC HÀNH LẬP TRÌNH .NET C# (BÀI 6.3, 6.4, 6.5)

## MỤC 1: THÔNG TIN SINH VIÊN
- **Họ và tên sinh viên:** Phạm Xuân Tiến
- **Mã sinh viên:** 24810320273
- **Lớp / Khóa:** D19QTANM1 - Khoa Công nghệ Thông tin (EPU)
- **Tên Đề tài / Bài tập:** 
  - Bài 6.3: Hệ thống Quản lý Công việc & Tiến độ Dự án (Project & Task Management)
  - Bài 6.4: Hệ thống Quản lý Garage và Bảo dưỡng Ô tô (Garage Management)
  - Bài 6.5: Hệ thống Quản lý Đặt phòng & Dịch vụ Karaoke (Karaoke Management)

---

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN
1. Khởi động MySQL trên **XAMPP Control Panel** (Port 3306).
2. Chạy tệp script SQL tại thư mục `Database/ScriptDB.sql` để tạo 3 CSDL: `ProjectTaskDB`, `GarageDB`, `KaraokeDB`.
3. Mở Solution bằng **Visual Studio 2022**.
4. Nhấn phím **F5** để khởi chạy Menu điều hướng tổng hợp (`frmMainApp`).

---

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

### 1. Màn hình Chính / Tổng quan hệ thống
![Màn hình chính](images/01_main_screen.png)
*Minh họa: Giao diện điều hướng trung tâm tích hợp cả 3 hệ thống thực hành 6.3, 6.4, 6.5.*

### 2. Màn hình Quản lý Danh mục (CRUD & Lọc trạng thái Task)
![Màn hình CRUD](images/02_crud_screen.png)
*Minh họa: Lọc danh sách công việc theo dự án và trạng thái (To Do, In Progress, Done) bằng parameterized query.*

### 3. Màn hình Thực thi Nghiệp vụ chính (Visual Grid & Mở phòng Karaoke)
![Màn hình Nghiệp vụ](images/03_transaction_screen.png)
*Minh họa: Sơ đồ phòng hát trực quan (Visual Grid Layout), chuyển trạng thái phòng và mở hóa đơn tự động.*

### 4. Màn hình Báo cáo - Thống kê (Cảnh báo Trễ hạn Overdue)
![Màn hình Thống kê](images/04_report_screen.png)
*Minh họa: Sử dụng SqlDataReader quét các công việc quá HanNop mà chưa hoàn thành để đưa ra cảnh báo kịp thời.*

### 5. Minh họa An toàn Dữ liệu (Báo lỗi Transaction Rollback)
![Minh họa Xử lý lỗi](images/05_error_handling.png)
*Minh họa: Kích hoạt trans.Rollback() hủy toàn bộ phiếu sửa chữa khi số lượng phụ tùng xuất vượt quá số lượng tồn kho.*