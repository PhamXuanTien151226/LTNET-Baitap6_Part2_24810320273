using System.Data;
using MultiManageApp.DAL;
using MultiManageApp.DTO;

namespace MultiManageApp.BUS
{
    public class TaskBUS
    {
        private TaskDAL dal = new TaskDAL();
        public DataTable LayDuAn() => dal.GetDanhSachDuAn();
        public DataTable LayTaskTheoDuAn(int maDA, string status) => dal.GetTasksByDuAn(maDA, status);
        public bool ChuyenTrangThai(int maTask, string status) => dal.UpdateTrangThaiTask(maTask, status);
        public DataTable LayTaskQuaHan() => dal.GetTasksQuaHan();
    }

    public class GarageBUS
    {
        private GarageDAL dal = new GarageDAL();
        public DataTable LayXe() => dal.GetDanhSachXe();
        public DataTable LayPhuTung() => dal.GetPhuTung();
        public (bool Success, string Message) LapPhieu(PhieuSuaChuaDTO p)
        {
            if (p.ChiTiet.Count == 0) return (false, "Chưa chọn phụ tùng/dịch vụ sửa chữa!");
            return dal.LapPhieuSuaChua(p);
        }
    }

    public class KaraokeBUS
    {
        private KaraokeDAL dal = new KaraokeDAL();
        public DataTable LayPhong() => dal.GetPhongHat();
        public DataTable LayDichVu() => dal.GetDichVu();
        public bool MoPhongHat(int maP) => dal.MoPhong(maP);
        public (bool Success, string Message) ThanhToan(int maP) => dal.ThanhToanPhong(maP);
    }
}