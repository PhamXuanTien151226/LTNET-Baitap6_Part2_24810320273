using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MultiManageApp.BUS;

namespace MultiManageApp.GUI
{
    public class frmKaraokeMain : Form
    {
        private KaraokeBUS bus = new KaraokeBUS();
        private FlowLayoutPanel pnlGrid = null!;
        private int selectedMaPhong = 0;
        private Label lblSelected = null!;

        public frmKaraokeMain()
        {
            this.Text = "Bài 6.5: Hệ thống Quản lý Đặt phòng & Dịch vụ Karaoke";
            this.Size = new Size(950, 580);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 65, Padding = new Padding(10) };
            lblSelected = new Label { Text = "Chưa chọn phòng", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            Button btnMo = new Button { Text = "MỞ PHÒNG HÁT", Location = new Point(320, 15), Size = new Size(160, 36), BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat };
            Button btnPay = new Button { Text = "TÍNH TIỀN & TRẢ PHÒNG", Location = new Point(490, 15), Size = new Size(200, 36), BackColor = Color.LightCoral, Font = new Font("Segoe UI", 9F, FontStyle.Bold), FlatStyle = FlatStyle.Flat };

            btnMo.Click += (s, e) =>
            {
                if (selectedMaPhong == 0) return;
                if (bus.MoPhongHat(selectedMaPhong)) { MessageBox.Show("Đã mở phòng thành công!"); RenderRooms(); }
            };

            btnPay.Click += (s, e) =>
            {
                if (selectedMaPhong == 0) return;
                var res = bus.ThanhToan(selectedMaPhong);
                MessageBox.Show(res.Message, res.Success ? "Hóa đơn" : "Thông báo");
                if (res.Success) RenderRooms();
            };

            pnlTop.Controls.AddRange(new Control[] { lblSelected, btnMo, btnPay });

            pnlGrid = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };

            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlTop);

            RenderRooms();
        }

        private void RenderRooms()
        {
            pnlGrid.Controls.Clear();
            var dt = bus.LayPhong();
            foreach (DataRow r in dt.Rows)
            {
                int maP = Convert.ToInt32(r["MaPhong"]);
                string tenP = r["TenPhong"].ToString()!;
                string tt = r["TrangThai"].ToString()!;
                decimal gia = Convert.ToDecimal(r["GiaTheoGio"]);

                Button btn = new Button
                {
                    Text = $"{tenP}\n({tt})\n{gia:N0} đ/h",
                    Size = new Size(160, 110),
                    Margin = new Padding(15),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    BackColor = tt == "Trống" ? Color.LightGreen : Color.LightPink,
                    FlatStyle = FlatStyle.Flat
                };

                btn.Click += (s, e) =>
                {
                    selectedMaPhong = maP;
                    lblSelected.Text = $"Đang chọn: {tenP} - Trạng thái: {tt}";
                };

                pnlGrid.Controls.Add(btn);
            }
        }
    }
}