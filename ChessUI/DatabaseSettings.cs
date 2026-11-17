namespace ChessUI
{
    internal static class DatabaseSettings
    {
        public static string ConnectionString =>
            System.Environment.GetEnvironmentVariable("CHESS_DB_CONNECTION_STRING")
            ?? "Server=localhost;Database=UserAuthentication;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
    }
}
