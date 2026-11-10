using MySql.Data.MySqlClient;

namespace MultiManageApp.DAL
{
    public static class DatabaseHelper
    {
        public static readonly string TaskConn = "Server=localhost;Port=3306;Database=ProjectTaskDB;Uid=root;Pwd=;Charset=utf8mb4;";
        public static readonly string GarageConn = "Server=localhost;Port=3306;Database=GarageDB;Uid=root;Pwd=;Charset=utf8mb4;";
        public static readonly string KaraokeConn = "Server=localhost;Port=3306;Database=KaraokeDB;Uid=root;Pwd=;Charset=utf8mb4;";

        public static MySqlConnection GetTaskConnection() => new MySqlConnection(TaskConn);
        public static MySqlConnection GetGarageConnection() => new MySqlConnection(GarageConn);
        public static MySqlConnection GetKaraokeConnection() => new MySqlConnection(KaraokeConn);
    }
}