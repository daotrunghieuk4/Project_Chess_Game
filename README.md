# Chess Game

Trò chơi cờ vua trên Windows dùng C# và WPF (.NET 8), gồm giao diện bàn cờ, luật di chuyển, phong cấp, tạm dừng, chơi với máy và lịch sử ván đấu theo tài khoản.

## Cấu trúc

| Thành phần | Nội dung |
|---|---|
| `ChessLogic/` | Trạng thái bàn cờ, quân cờ, nước đi hợp lệ và AI |
| `ChessUI/` | Cửa sổ WPF, đăng nhập, đăng ký và lịch sử |
| `ChessUI/Assets/` | Hình bàn cờ, quân cờ và âm thanh |
| `datachess.sql` | Script database SQL Server của dự án |
| `Chess.sln` | Solution để mở trong Visual Studio |

AI chọn nước ăn quân có giá trị cao nhất nếu có, sau đó chọn ngẫu nhiên trong các nước hợp lệ còn lại.

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

## Phạm vi sử dụng

Đây là dự án học tập. Phần tài khoản hiện đối chiếu mật khẩu trực tiếp trong SQL, cần bổ sung lưu mật khẩu băm trước khi sử dụng cho người dùng thật. Cấu hình kết nối đã chuyển sang biến môi trường; nếu thông tin đăng nhập SQL trong các commit cũ còn hiệu lực, chủ sở hữu cần đổi mật khẩu đó.
