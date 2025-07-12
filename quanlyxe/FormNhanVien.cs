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
    public partial class FormNhanVien : Form
    {
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                
                textBox2.Text = row.Cells["TenNhanVien"].Value.ToString();
                string gioiTinh = row.Cells["GioiTinh"].Value.ToString();
                if (gioiTinh == "Nam")
                {
                    rdNam.Checked = true; // Chọn RadioButton cho Nam
                    rdNu.Checked = false; // Hủy chọn cho Nữ
                }
                else if (gioiTinh == "Nu")
                {
                    rdNu.Checked = true; // Chọn RadioButton cho Nữ
                    rdNam.Checked = false; // Hủy chọn cho Nam
                }
                else
                {
                    rdNam.Checked = false; // Hủy chọn cho Nam
                    rdNu.Checked = false; // Hủy chọn cho Nữ
                }

                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NamSinh"].Value);
                textBox3.Text = row.Cells["DiaChi"].Value.ToString();
                textBox4.Text = row.Cells["DienThoai"].Value.ToString();
                textBox5.Text = row.Cells["Email"].Value.ToString();
                txtTaikhoan.Text = row.Cells["TenTaiKhoan"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();
                textBox1.Text = row.Cells["Role"].Value.ToString();

                // Thiết lập giới tính

            }
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM NhanVien";

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

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO NhanVien ( TenNhanVien, GioiTinh, NamSinh, DiaChi, DienThoai, Email, TenTaiKhoan, MatKhau, Role) " +
                                   "VALUES ( @TenNhanVien, @GioiTinh, @NamSinh, @DiaChi, @DienThoai, @Email,  @TenTaiKhoan, @MatKhau, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@TenNhanVien", textBox2.Text); // textBoxTenNhanVien cho tên nhân viên
                        command.Parameters.AddWithValue("@GioiTinh", rdNam.Checked ? "Nam" : "Nữ");
                        command.Parameters.AddWithValue("@NamSinh", dateTimePicker1.Value.Date); // dateTimePickerNamSinh cho năm sinh
                        command.Parameters.AddWithValue("@DiaChi", textBox3.Text); // textBoxDiaChi cho địa chỉ
                        command.Parameters.AddWithValue("@DienThoai", textBox4.Text); // textBoxDienThoai cho điện thoại
                        command.Parameters.AddWithValue("@Email", textBox5.Text); // textBoxEmail cho email
                        command.Parameters.AddWithValue("@TenTaiKhoan", txtTaikhoan.Text);
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
                    string query = "SELECT * FROM NhanVien";

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
                int maNhanVien = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNhanVien"].Value);

                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên với mã " + maNhanVien + "?",
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
                            string query = "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

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
            // Kiểm tra xem có hàng nào được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy mã nhân viên từ hàng được chọn
                int maNhanVien = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNhanVien"].Value);

                string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE NhanVien SET TenNhanVien = @TenNhanVien, NamSinh = @NamSinh, " +
                                       "GioiTinh = @GioiTinh, DiaChi = @DiaChi, DienThoai = @DienThoai, Email = @Email, TenTaiKhoan = @TenTaiKhoan, MatKhau = @MatKhau, Role = @Role " +
                                       "WHERE MaNhanVien = @MaNhanVien";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                            command.Parameters.AddWithValue("@TenNhanVien", textBox2.Text);
                            command.Parameters.AddWithValue("@GioiTinh", rdNam.Checked ? "Nam" : "Nữ");
                            command.Parameters.AddWithValue("@NamSinh", dateTimePicker1.Value.Date);
                            command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                            command.Parameters.AddWithValue("@DienThoai", textBox4.Text);
                            command.Parameters.AddWithValue("@Email", textBox5.Text);
                            command.Parameters.AddWithValue("@TenTaiKhoan", txtTaikhoan.Text);
                            command.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);

                            command.Parameters.AddWithValue("@Role", textBox1.Text);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                                LoadData(); // Cập nhật DataGridView sau khi sửa
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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            txtMatKhau.Clear();
            txtTaikhoan.Clear();

            // Đặt lại giá trị của DateTimePicker
            dateTimePicker1.Value = DateTime.Now; // Hoặc giá trị mặc định mà bạn muốn
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        

        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            FormKhachHang kh = new FormKhachHang();
            kh.Show();
            this.Hide();
        }

        private void toolStripMenuItem5_Click_1(object sender, EventArgs e)
        {
            SanPham sp = new SanPham();
            sp.Show();
            this.Hide();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); new TimKiem().Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string userRole = "admin"; // Lấy role từ nơi bạn lưu trữ
            FormTrangChu tc = new FormTrangChu(userRole);
            tc.Show();
            this.Hide();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void khácHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
