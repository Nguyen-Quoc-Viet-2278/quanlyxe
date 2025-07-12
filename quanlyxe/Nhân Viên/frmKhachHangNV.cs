using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace quanlyxe
{
    public partial class frmKhachHangNV : Form
    {
        public frmKhachHangNV()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox2.Text = row.Cells["TenKhachHang"].Value.ToString();
                textBox3.Text = row.Cells["DiaChi"].Value.ToString();
                textBox4.Text = row.Cells["DienThoai"].Value.ToString();
                txtTaikhoan.Text = row.Cells["TaiKhoan"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();
                textBox1.Text = row.Cells["Role"].Value.ToString();

            }
        }

        private void frmKhachHangNV_Load(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM KhachHang";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Giả sử DataGridView của bạn tên là dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            txtMatKhau.Clear();
            txtTaikhoan.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO KhachHang ( TenKhachHang, DiaChi, DienThoai, TaiKhoan, MatKhau, Role) " +
                                   "VALUES ( @TenKhachHang, @DiaChi, @DienThoai, @TaiKhoan, @MatKhau, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TenKhachHang", textBox2.Text); // textBoxTenKhachHang cho tên nhân viên
                        command.Parameters.AddWithValue("@DiaChi", textBox3.Text); // textBoxDiaChi cho địa chỉ
                        command.Parameters.AddWithValue("@DienThoai", textBox4.Text); // textBoxDienThoai cho điện thoại
                        command.Parameters.AddWithValue("@TaiKhoan", txtTaikhoan.Text);
                        command.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);

                        command.Parameters.AddWithValue("@Role", textBox1.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Dữ liệu đã được lưu thành công!");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

        }
        private void LoadData()
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM KhachHang";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Giả sử DataGridView của bạn tên là dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Kiểm tra xem có hàng nào được chọn không
            {
                // Lấy mã nhân viên từ hàng được chọn
                int maKhachHang = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaKhachHang"].Value);

                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên với mã " + maKhachHang + "?",
                                               "Xác nhận xóa",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) // Nếu chọn "Có"
                {
                    string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaKhachHang", maKhachHang);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Dữ liệu đã được xóa thành công!");
                                    LoadData(); // Cập nhật DataGridView sau khi xóa
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy nhân viên với mã đã cho.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để xóa.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if any row is selected
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the customer ID from the selected row
                int maKhachHang = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaKhachHang"].Value);

                string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Update as necessary

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE KhachHang SET TenKhachHang = @TenKhachHang, " + // Fixed the query here
                                       "DiaChi = @DiaChi, DienThoai = @DienThoai, TaiKhoan = @TaiKhoan, MatKhau = @MatKhau, Role = @Role " +
                                       "WHERE MaKhachHang = @MaKhachHang";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@TenKhachHang", textBox2.Text);
                            command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                            command.Parameters.AddWithValue("@DienThoai", textBox4.Text);
                            command.Parameters.AddWithValue("@TaiKhoan", txtTaikhoan.Text);
                            command.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);
                            command.Parameters.AddWithValue("@Role", textBox1.Text);
                            command.Parameters.AddWithValue("@MaKhachHang", maKhachHang); // Ensure this is added

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                                LoadData(); // Update DataGridView after editing
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhân viên với mã đã cho.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để sửa.");
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FormNhanVien nv = new FormNhanVien();
            nv.Show();
            this.Hide();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon();
            hd.Show();
            this.Hide();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ThongKe tk = new ThongKe();
            tk.Show();
            this.Hide();
        }

        private void khoHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKhoHang kh = new FormKhoHang();
            kh.Show();
            this.Hide();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKhachHang kh = new FormKhachHang();
            kh.Show();
            this.Hide();
        }

        private void trởVềTrangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                FormDangNhap dn = new FormDangNhap();
                dn.Show();
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }
    }

}
