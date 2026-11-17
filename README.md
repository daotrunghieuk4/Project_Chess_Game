# Chess Game

Trò chơi cờ vua trên Windows dùng C# và WPF (.NET 8), gồm giao diện bàn cờ, luật di chuyển, phong cấp, tạm dừng, chơi với máy và lịch sử ván đấu theo tài khoản.

## Tổng quan

Dự án triển khai một ứng dụng cờ vua desktop với ba phần: luật chơi, giao diện tương tác và lưu trữ tài khoản/lịch sử. Cách tổ chức này giúp việc xử lý nước đi nằm trong lớp logic, trong khi WPF đảm nhiệm hiển thị bàn cờ, điều hướng màn hình và phản hồi thao tác người chơi.

Ứng dụng sử dụng các tài nguyên hình ảnh, con trỏ và âm thanh đi kèm để xây dựng trải nghiệm chơi trên Windows. SQL Server cung cấp dữ liệu tài khoản và lịch sử ván đấu theo người dùng.

## Chức năng

| Nhóm | Nội dung |
|---|---|
| Bàn cờ | Hiển thị quân cờ, chọn quân và thực hiện nước đi |
| Luật chơi | Xác định nước đi hợp lệ và xử lý trạng thái ván đấu |
| Tương tác | Phong cấp quân, tạm dừng và điều hướng màn hình |
| Đối thủ máy | Lựa chọn nước đi theo chiến lược ưu tiên ăn quân |
| Tài khoản | Đăng ký, đăng nhập và đổi mật khẩu |
| Lịch sử | Truy xuất thông tin các ván đấu theo tài khoản |

## Cấu trúc

| Thành phần | Nội dung |
|---|---|
| `ChessLogic/` | Trạng thái bàn cờ, quân cờ, nước đi hợp lệ và AI |
| `ChessUI/` | Cửa sổ WPF, đăng nhập, đăng ký và lịch sử |
| `ChessUI/Assets/` | Hình bàn cờ, quân cờ và âm thanh |
| `datachess.sql` | Script database SQL Server của dự án |
| `Chess.sln` | Solution để mở trong Visual Studio |

AI chọn nước ăn quân có giá trị cao nhất nếu có, sau đó chọn ngẫu nhiên trong các nước hợp lệ còn lại.

## Kiến trúc

```text
Thao tác người chơi
        ↓
ChessUI — hiển thị và điều khiển WPF
        ↓
ChessLogic — trạng thái và luật di chuyển

ChessUI ↔ SQL Server — tài khoản và lịch sử
```

`ChessLogic` là điểm bắt đầu khi tìm hiểu luật và chiến lược chọn nước đi. `ChessUI` chứa các cửa sổ, menu và mã xử lý sự kiện. Thông tin kết nối database được tập trung tại `ChessUI/DatabaseSettings.cs`, đọc từ biến môi trường để thuận tiện cấu hình giữa các máy.

## Yêu cầu

- Windows và .NET 8 SDK, hoặc Visual Studio 2022 với workload .NET Desktop Development.
- SQL Server cho tài khoản và lịch sử ván đấu.

## Chuẩn bị database

Mở `datachess.sql` bằng SQL Server Management Studio. Kiểm tra hai đường dẫn `FILENAME` ở phần tạo database và sửa theo thư mục dữ liệu SQL Server trên máy trước khi chạy script. Script tạo database `UserAuthentication`, bảng `Users` và `GameHistory`.

Ứng dụng mặc định kết nối SQL Server trên `localhost` bằng Windows Authentication. Để dùng server khác, đặt biến môi trường trong terminal chạy ứng dụng.

```powershell
$env:CHESS_DB_CONNECTION_STRING = 'Server=localhost;Database=UserAuthentication;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
dotnet restore Chess.sln
dotnet build Chess.sln --configuration Release
dotnet run --project ChessUI/ChessUI.csproj --configuration Release
```

Sau khi mở ứng dụng, đăng ký tài khoản rồi đăng nhập để tới menu chơi. Khi dùng Visual Studio, đặt `ChessUI` làm startup project.

## Tài nguyên giao diện

Hình bàn cờ được sử dụng trong ứng dụng.

![Tài nguyên bàn cờ](ChessUI/Assets/Board.png)

## Trải nghiệm ứng dụng

1. Chuẩn bị database và khởi chạy `ChessUI` theo hướng dẫn trên.
2. Đăng ký tài khoản, sau đó đăng nhập để vào giao diện chơi.
3. Chọn quân và thực hiện các nước đi theo trạng thái bàn cờ.
4. Sử dụng menu tạm dừng hoặc xử lý phong cấp khi tình huống xuất hiện.
5. Mở cửa sổ lịch sử để xem thông tin ván đấu gắn với tài khoản.

## Cấu hình môi trường

| Thành phần | Thiết lập |
|---|---|
| Database | `UserAuthentication` trên SQL Server |
| Xác thực kết nối mặc định | Windows Authentication |
| Kết nối tùy chỉnh | Biến `CHESS_DB_CONNECTION_STRING` |
| Project khởi chạy | `ChessUI/ChessUI.csproj` |
| Cấu hình build ví dụ | `Release` |

Với SQL Server Express hoặc instance có tên, thay phần `Server` trong chuỗi kết nối cho phù hợp. Tài khoản Windows hoặc SQL dùng để kết nối cần quyền truy cập database và các bảng của ứng dụng. Khi chạy qua Visual Studio, bảo đảm tiến trình IDE nhận được biến môi trường đã thiết lập.

## Triển khai và quản lý tài khoản

Cấu hình hiện tại phục vụ môi trường local. Luồng tài khoản đối chiếu mật khẩu trực tiếp trong SQL; khi triển khai cho người dùng, chuyển sang lưu hash mật khẩu có salt và sử dụng quyền database phù hợp. Quản lý thông tin kết nối ngoài mã nguồn và thay thế mọi thông tin xác thực cũ đã xuất hiện trong lịch sử commit trước khi tái sử dụng môi trường đó.

## Hướng dẫn phát triển

Khi điều chỉnh luật hoặc cách chọn nước đi, bắt đầu trong `ChessLogic/` và kiểm tra các trạng thái bàn cờ liên quan. Khi thay đổi giao diện, chỉnh cửa sổ XAML cùng phần xử lý sự kiện tương ứng trong `ChessUI/`. Các thư mục `.vs`, `bin` và `obj` được tạo lại bởi công cụ phát triển và đã được loại khỏi phần mã nguồn theo dõi.

Nếu build báo thiếu workload, kiểm tra .NET Desktop Development trong Visual Studio Installer. Nếu đăng nhập không kết nối được database, kiểm tra tên instance, biến môi trường và việc thực thi script SQL trước khi kiểm tra các bước giao diện.
