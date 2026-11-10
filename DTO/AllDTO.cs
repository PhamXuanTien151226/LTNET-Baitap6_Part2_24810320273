using System;
using System.Collections.Generic;

namespace MultiManageApp.DTO
{
    // ==========================================
    // DTO BÀI 6.3: QUẢN LÝ DỰ ÁN & CÔNG VIỆC
    // ==========================================
    public class DuAnDTO
    {
        public int MaDuAn { get; set; }
        public string TenDuAn { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; } = "Đang chạy";
    }

    public class TaskDTO
    {
        public int MaTask { get; set; }
        public int MaDuAn { get; set; }
        public string TenTask { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public DateTime HanNop { get; set; }
        public string DoUuTien { get; set; } = "Trung bình";
        public string TrangThai { get; set; } = "To Do";
    }

    // ==========================================
    // DTO BÀI 6.4: QUẢN LÝ GARAGE & SỬA CHỮA
    // ==========================================
    public class ChiTietSuaChuaDTO
    {
        public int MaPhuTung { get; set; }
        public string TenPhuTung { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal TienCong { get; set; }
        public decimal ThanhTien => (SoLuong * DonGia) + TienCong;
    }

    public class PhieuSuaChuaDTO
    {
        public string BienSo { get; set; } = string.Empty;
        public string NoiDungSua { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
        public List<ChiTietSuaChuaDTO> ChiTiet { get; set; } = new List<ChiTietSuaChuaDTO>();
    }

    // ==========================================
    // DTO BÀI 6.5: QUẢN LÝ KARAOKE
    // ==========================================
    public class PhongHatDTO
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = "Thường";
        public decimal GiaTheoGio { get; set; }
        public string TrangThai { get; set; } = "Trống";
    }

    public class ChiTietDichVuDTO
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}