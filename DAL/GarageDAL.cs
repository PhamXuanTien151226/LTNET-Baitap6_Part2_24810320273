using System;
using System.Data;
using MySql.Data.MySqlClient;
using MultiManageApp.DTO;

namespace MultiManageApp.DAL
{
    public class GarageDAL
    {
        public DataTable GetDanhSachXe()
        {
            using var conn = DatabaseHelper.GetGarageConnection();
            var da = new MySqlDataAdapter("SELECT x.BienSo, x.HieuXe, kh.TenKH, kh.DienThoai FROM Xe x JOIN KhachHang kh ON x.MaKH = kh.MaKH", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetPhuTung()
        {
            using var conn = DatabaseHelper.GetGarageConnection();
            var da = new MySqlDataAdapter("SELECT MaPhuTung, TenPhuTung, DonGia, SoLuongTon FROM PhuTung", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public (bool Success, string Message) LapPhieuSuaChua(PhieuSuaChuaDTO p)
        {
            using var conn = DatabaseHelper.GetGarageConnection();
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                string sqlPhieu = "INSERT INTO PhieuSuaChua (BienSo, NoiDungSua, TongTien) VALUES (@BienSo, @NoiDung, @TongTien); SELECT LAST_INSERT_ID();";
                var cmdP = new MySqlCommand(sqlPhieu, conn, trans);
                cmdP.Parameters.AddWithValue("@BienSo", p.BienSo);
                cmdP.Parameters.AddWithValue("@NoiDung", p.NoiDungSua);
                cmdP.Parameters.AddWithValue("@TongTien", p.TongTien);
                int maPhieu = Convert.ToInt32(cmdP.ExecuteScalar());

                foreach (var item in p.ChiTiet)
                {
                    string sqlCheck = "SELECT SoLuongTon FROM PhuTung WHERE MaPhuTung = @MaPT FOR UPDATE";
                    var cmdCheck = new MySqlCommand(sqlCheck, conn, trans);
                    cmdCheck.Parameters.AddWithValue("@MaPT", item.MaPhuTung);
                    int ton = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (ton < item.SoLuong)
                    {
                        trans.Rollback();
                        return (false, $"Phụ tùng [{item.TenPhuTung}] chỉ còn {ton}, không đủ {item.SoLuong} để xuất!");
                    }

                    var cmdUpdateTon = new MySqlCommand("UPDATE PhuTung SET SoLuongTon = SoLuongTon - @SL WHERE MaPhuTung = @MaPT", conn, trans);
                    cmdUpdateTon.Parameters.AddWithValue("@SL", item.SoLuong);
                    cmdUpdateTon.Parameters.AddWithValue("@MaPT", item.MaPhuTung);
                    cmdUpdateTon.ExecuteNonQuery();

                    var cmdCT = new MySqlCommand("INSERT INTO ChiTietSuaChua (MaPhieu, MaPhuTung, SoLuong, TienCong) VALUES (@MaPhieu, @MaPT, @SL, @TC)", conn, trans);
                    cmdCT.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    cmdCT.Parameters.AddWithValue("@MaPT", item.MaPhuTung);
                    cmdCT.Parameters.AddWithValue("@SL", item.SoLuong);
                    cmdCT.Parameters.AddWithValue("@TC", item.TienCong);
                    cmdCT.ExecuteNonQuery();
                }

                trans.Commit();
                return (true, $"Lập phiếu sửa chữa thành công! Mã phiếu: PSC-{maPhieu:D4}");
            }
            catch (Exception ex)
            {
                try { trans.Rollback(); } catch { }
                return (false, "Lỗi Transaction: " + ex.Message);
            }
        }
    }
}