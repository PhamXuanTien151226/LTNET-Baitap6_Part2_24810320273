using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace MultiManageApp.DAL
{
    public class TaskDAL
    {
        public DataTable GetDanhSachDuAn()
        {
            using var conn = DatabaseHelper.GetTaskConnection();
            var da = new MySqlDataAdapter("SELECT MaDuAn, TenDuAn FROM DuAn", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetTasksByDuAn(int maDuAn, string trangThai)
        {
            using var conn = DatabaseHelper.GetTaskConnection();
            string sql = "SELECT MaTask, TenTask, MoTa, HanNop, DoUuTien, TrangThai FROM CongViec WHERE MaDuAn = @MaDA";
            if (trangThai != "Tất cả") sql += " AND TrangThai = @Status";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaDA", maDuAn);
            if (trangThai != "Tất cả") cmd.Parameters.AddWithValue("@Status", trangThai);

            var da = new MySqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public bool UpdateTrangThaiTask(int maTask, string trangThaiMoi)
        {
            using var conn = DatabaseHelper.GetTaskConnection();
            conn.Open();
            string sql = "UPDATE CongViec SET TrangThai = @TrangThai WHERE MaTask = @MaTask";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
            cmd.Parameters.AddWithValue("@MaTask", maTask);
            return cmd.ExecuteNonQuery() > 0;
        }

        public DataTable GetTasksQuaHan()
        {
            using var conn = DatabaseHelper.GetTaskConnection();
            conn.Open();
            string sql = "SELECT MaTask, TenTask, HanNop, TrangThai FROM CongViec WHERE HanNop < NOW() AND TrangThai != 'Done'";
            var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
    }
}