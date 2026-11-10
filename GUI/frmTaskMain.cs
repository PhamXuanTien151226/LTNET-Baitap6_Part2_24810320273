using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MultiManageApp.BUS;

namespace MultiManageApp.GUI
{
    public class frmTaskMain : Form
    {
        private TaskBUS bus = new TaskBUS();
        private ComboBox cboDuAn = null!, cboTrangThai = null!;
        private DataGridView dgvTasks = null!;

        public frmTaskMain()
        {
            this.Text = "Bài 6.3: Quản lý Công việc & Tiến độ Dự án (Kanban/List View)";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10) };
            Label lblDA = new Label { Text = "Dự án:", Location = new Point(15, 18), AutoSize = true };
            cboDuAn = new ComboBox { Location = new Point(65, 15), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblTT = new Label { Text = "Lọc trạng thái:", Location = new Point(300, 18), AutoSize = true };
            cboTrangThai = new ComboBox { Location = new Point(390, 15), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "To Do", "In Progress", "Done" });
            cboTrangThai.SelectedIndex = 0;

            Button btnFilter = new Button { Text = "Lọc Task", Location = new Point(530, 13), Width = 90, Height = 30 };
            Button btnDone = new Button { Text = "Đánh dấu Hoàn thành (Done)", Location = new Point(630, 13), Width = 200, Height = 30, BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
            Button btnOverdue = new Button { Text = "Cảnh báo Trễ hạn", Location = new Point(840, 13), Width = 130, Height = 30, BackColor = Color.LightCoral, FlatStyle = FlatStyle.Flat };

            btnFilter.Click += (s, e) => LoadTasks();
            btnDone.Click += BtnDone_Click;
            btnOverdue.Click += BtnOverdue_Click;

            pnlTop.Controls.AddRange(new Control[] { lblDA, cboDuAn, lblTT, cboTrangThai, btnFilter, btnDone, btnOverdue });

            dgvTasks = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White };
            this.Controls.Add(dgvTasks);
            this.Controls.Add(pnlTop);

            LoadDuAn();
        }

        private void LoadDuAn()
        {
            var dt = bus.LayDuAn();
            cboDuAn.DataSource = dt;
            cboDuAn.DisplayMember = "TenDuAn";
            cboDuAn.ValueMember = "MaDuAn";
            if (cboDuAn.Items.Count > 0) LoadTasks();
        }

        private void LoadTasks()
        {
            if (cboDuAn.SelectedValue == null) return;
            int maDA = Convert.ToInt32(cboDuAn.SelectedValue);
            string tt = cboTrangThai.SelectedItem?.ToString() ?? "Tất cả";
            dgvTasks.DataSource = bus.LayTaskTheoDuAn(maDA, tt);
        }

        private void BtnDone_Click(object? sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow?.DataBoundItem is DataRowView row)
            {
                int maTask = Convert.ToInt32(row["MaTask"]);
                if (bus.ChuyenTrangThai(maTask, "Done"))
                {
                    MessageBox.Show("Đã chuyển trạng thái Task thành [Done]!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTasks();
                }
            }
        }

        private void BtnOverdue_Click(object? sender, EventArgs e)
        {
            var dt = bus.LayTaskQuaHan();
            string msg = "=== DANH SÁCH CÔNG VIỆC TRỄ HẠN (OVERDUE) ===\n\n";
            foreach (DataRow r in dt.Rows)
            {
                msg += $"- Task #{r["MaTask"]}: {r["TenTask"]} (Hạn nộp: {Convert.ToDateTime(r["HanNop"]):dd/MM/yyyy} | {r["TrangThai"]})\n";
            }
            if (dt.Rows.Count == 0) msg += "Tuyệt vời! Hiện không có công việc nào trễ hạn.";
            MessageBox.Show(msg, "Báo cáo cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}