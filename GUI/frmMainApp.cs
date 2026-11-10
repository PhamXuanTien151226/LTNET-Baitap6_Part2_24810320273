using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultiManageApp.GUI
{
    public class frmMainApp : Form
    {
        public frmMainApp()
        {
            this.Text = "HỆ THỐNG THỰC HÀNH C# .NET - BÀI TẬP 6.3, 6.4, 6.5";
            this.Size = new Size(540, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lbl = new Label
            {
                Text = "BÁO CÁO THỰC HÀNH LẬP TRÌNH .NET C#\nSinh viên: Phạm Xuân Tiến - 24810320273\nLớp: D19QTANM1",
                Dock = DockStyle.Top,
                Height = 85,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue
            };

            Button btn1 = new Button
            {
                Text = "1. BÀI 6.3: QUẢN LÝ TIẾN ĐỘ DỰ ÁN & TASK",
                Location = new Point(60, 100),
                Size = new Size(400, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };

            Button btn2 = new Button
            {
                Text = "2. BÀI 6.4: QUẢN LÝ GARAGE & SỬA CHỮA Ô TÔ",
                Location = new Point(60, 165),
                Size = new Size(400, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.LightSalmon,
                FlatStyle = FlatStyle.Flat
            };

            Button btn3 = new Button
            {
                Text = "3. BÀI 6.5: QUẢN LÝ ĐẶT PHÒNG HÁT KARAOKE",
                Location = new Point(60, 230),
                Size = new Size(400, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            btn1.Click += (s, e) => new frmTaskMain().ShowDialog();
            btn2.Click += (s, e) => new frmGarageMain().ShowDialog();
            btn3.Click += (s, e) => new frmKaraokeMain().ShowDialog();

            this.Controls.AddRange(new Control[] { lbl, btn1, btn2, btn3 });
        }
    }
}