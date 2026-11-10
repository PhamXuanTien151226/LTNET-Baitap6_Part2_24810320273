using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MultiManageApp.BUS;
using MultiManageApp.DTO;

namespace MultiManageApp.GUI
{
    public class frmGarageMain : Form
    {
        private GarageBUS bus = new GarageBUS();
        private List<ChiTietSuaChuaDTO> chiTietList = new List<ChiTietSuaChuaDTO>();

        private ComboBox cboXe = null!, cboPhuTung = null!;
        private NumericUpDown numSL = null!, numCong = null!;
        private TextBox txtNoiDung = null!;
        private DataGridView dgvChiTiet = null!;
        private Label lblTongTien = null!;

        public frmGarageMain()
        {
            this.Text = "Bài 6.4: Quản lý Garage & Bảo dưỡng Ô tô (Transaction Rollback)";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlLeft = new Panel { Dock = DockStyle.Left, Width = 400, Padding = new Padding(15) };
            Label l1 = new Label { Text = "Biển số xe tiếp nhận:", Location = new Point(15, 15), AutoSize = true };
            cboXe = new ComboBox { Location = new Point(15, 35), Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l2 = new Label { Text = "Nội dung hư hỏng / sửa chữa:", Location = new Point(15, 75), AutoSize = true };
            txtNoiDung = new TextBox { Location = new Point(15, 95), Width = 360, Height = 60, Multiline = true };

            Label l3 = new Label { Text = "Chọn phụ tùng thay thế:", Location = new Point(15, 170), AutoSize = true };
            cboPhuTung = new ComboBox { Location = new Point(15, 190), Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l4 = new Label { Text = "Số lượng:", Location = new Point(15, 230), AutoSize = true };
            numSL = new NumericUpDown { Location = new Point(15, 250), Width = 100, Minimum = 1, Maximum = 1000, Value = 1 };

            Label l5 = new Label { Text = "Tiền công sửa (VNĐ):", Location = new Point(140, 230), AutoSize = true };
            numCong = new NumericUpDown { Location = new Point(140, 250), Width = 150, Minimum = 0, Maximum = 100000000, Value = 100000, Increment = 50000 };

            Button btnThem = new Button { Text = "Thêm Phụ tùng / Dịch vụ", Location = new Point(15, 300), Width = 360, Height = 38, BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat };
            btnThem.Click += BtnThem_Click;

            pnlLeft.Controls.AddRange(new Control[] { l1, cboXe, l2, txtNoiDung, l3, cboPhuTung, l4, numSL, l5, numCong, btnThem });

            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
            dgvChiTiet = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White };

            Panel pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 70 };
            lblTongTien = new Label { Text = "Tổng chi phí: 0 VNĐ", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.DarkRed };
            Button btnLuu = new Button { Text = "LẬP PHIẾU SỬA CHỮA (ACID)", Location = new Point(350, 12), Size = new Size(260, 45), BackColor = Color.ForestGreen, ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnLuu.Click += BtnLuu_Click;

            pnlBot.Controls.AddRange(new Control[] { lblTongTien, btnLuu });
            pnlRight.Controls.Add(dgvChiTiet);
            pnlRight.Controls.Add(pnlBot);

            this.Controls.Add(pnlRight);
            this.Controls.Add(pnlLeft);

            LoadData();
        }

        private void LoadData()
        {
            cboXe.DataSource = bus.LayXe();
            cboXe.DisplayMember = "BienSo";
            cboXe.ValueMember = "BienSo";

            cboPhuTung.DataSource = bus.LayPhuTung();
            cboPhuTung.DisplayMember = "TenPhuTung";
            cboPhuTung.ValueMember = "MaPhuTung";
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (cboPhuTung.SelectedItem is DataRowView row)
            {
                chiTietList.Add(new ChiTietSuaChuaDTO
                {
                    MaPhuTung = Convert.ToInt32(row["MaPhuTung"]),
                    TenPhuTung = row["TenPhuTung"].ToString()!,
                    SoLuong = (int)numSL.Value,
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    TienCong = numCong.Value
                });
                CapNhatBang();
            }
        }

        private void CapNhatBang()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = chiTietList;
            decimal tong = 0;
            foreach (var item in chiTietList) tong += item.ThanhTien;
            lblTongTien.Text = $"Tổng chi phí: {tong:N0} VNĐ";
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            decimal tong = 0;
            foreach (var item in chiTietList) tong += item.ThanhTien;

            var p = new PhieuSuaChuaDTO
            {
                BienSo = cboXe.SelectedValue?.ToString() ?? "30K-999.88",
                NoiDungSua = txtNoiDung.Text,
                TongTien = tong,
                ChiTiet = new List<ChiTietSuaChuaDTO>(chiTietList)
            };

            var res = bus.LapPhieu(p);
            if (res.Success)
            {
                MessageBox.Show(res.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chiTietList.Clear();
                CapNhatBang();
                LoadData();
            }
            else
            {
                MessageBox.Show(res.Message, "Lỗi Giao Dịch (Rollback)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}