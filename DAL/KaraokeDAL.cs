using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace MultiManageApp.DAL
{
    public class KaraokeDAL
    {
        public DataTable GetPhongHat()
        {
            using var conn = DatabaseHelper.GetKaraokeConnection();
            var da = new MySqlDataAdapter("SELECT MaPhong, TenPhong, LoaiPhong, GiaTheoGio, TrangThai FROM PhongHat", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetDichVu()
        {
            using var conn = DatabaseHelper.GetKaraokeConnection();
            var da = new MySqlDataAdapter("SELECT MaDV, TenDV, DonGia, TonKho FROM DichVu", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public bool MoPhong(int maPhong)
        {
            using var conn = DatabaseHelper.GetKaraokeConnection();
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                var cmdUpdate = new MySqlCommand("UPDATE PhongHat SET TrangThai = 'Đang hát' WHERE MaPhong = @MaP", conn, trans);
                cmdUpdate.Parameters.AddWithValue("@MaP", maPhong);
                cmdUpdate.ExecuteNonQuery();

                var cmdHD = new MySqlCommand("INSERT INTO HoaDon (MaPhong, GioVao, TrangThai) VALUES (@MaP, NOW(), 0)", conn, trans);
                cmdHD.Parameters.AddWithValue("@MaP", maPhong);
                cmdHD.ExecuteNonQuery();

                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }

        public (bool Success, string Message) ThanhToanPhong(int maPhong)
        {
            using var conn = DatabaseHelper.GetKaraokeConnection();
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                string sqlGetHD = @"SELECT hd.MaHD, hd.GioVao, p.GiaTheoGio 
                                   FROM HoaDon hd 
                                   JOIN PhongHat p ON hd.MaPhong = p.MaPhong 
                                   WHERE hd.MaPhong = @MaP AND hd.TrangThai = 0 
                                   ORDER BY hd.MaHD DESC LIMIT 1";
                var cmdGet = new MySqlCommand(sqlGetHD, conn, trans);
                cmdGet.Parameters.AddWithValue("@MaP", maPhong);

                int maHD = 0;
                DateTime gioVao = DateTime.Now;
                decimal giaGio = 0;

                using (var reader = cmdGet.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        maHD = reader.GetInt32("MaHD");
                        gioVao = reader.GetDateTime("GioVao");
                        giaGio = reader.GetDecimal("GiaTheoGio");
                    }
                }

                if (maHD == 0)
                {
                    trans.Rollback();
                    return (false, "Phòng này chưa có hóa đơn mở!");
                }

                DateTime gioRa = DateTime.Now;
                double soGio = (gioRa - gioVao).TotalHours;
                if (soGio < 0.2) soGio = 0.5;
                decimal tienPhong = (decimal)Math.Round(soGio * (double)giaGio, 0);

                var cmdUpHD = new MySqlCommand("UPDATE HoaDon SET GioRa = @GioRa, TongTienPhong = @TienP, TrangThai = 1 WHERE MaHD = @MaHD", conn, trans);
                cmdUpHD.Parameters.AddWithValue("@GioRa", gioRa);
                cmdUpHD.Parameters.AddWithValue("@TienP", tienPhong);
                cmdUpHD.Parameters.AddWithValue("@MaHD", maHD);
                cmdUpHD.ExecuteNonQuery();

                var cmdUpP = new MySqlCommand("UPDATE PhongHat SET TrangThai = 'Trống' WHERE MaPhong = @MaP", conn, trans);
                cmdUpP.Parameters.AddWithValue("@MaP", maPhong);
                cmdUpP.ExecuteNonQuery();

                trans.Commit();
                return (true, $"Thanh toán thành công!\nThời gian hát: {soGio:N1} giờ\nTiền phòng: {tienPhong:N0} VNĐ");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return (false, "Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}