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
    public partial class HoaDon : Form
    {
        string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
        public HoaDon()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            LoadData();


        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM HoaDon";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    // Định dạng cột 'ThanhTien' (giá tiền) với dấu chấm
                    if (dataGridView1.Columns["ThanhTien"] != null)
                    {
                        dataGridView1.Columns["ThanhTien"].DefaultCellStyle.Format = "N0"; // Hiển thị số nguyên
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Ensure these column names match exactly with your database schema
                textBox1.Text = row.Cells["MaHoaDon"].Value?.ToString();
                textBox2.Text = row.Cells["TenKhachHang"].Value?.ToString();
                textBox4.Text = row.Cells["DienThoai"].Value?.ToString();
                textBox3.Text = row.Cells["DiaChi"].Value?.ToString();

                textBox8.Text = row.Cells["TenSanPham"].Value?.ToString();
                textBox7.Text = row.Cells["SoLuong"].Value?.ToString();
                textBox5.Text = row.Cells["ThanhTien"].Value?.ToString();

                // Safely handle the date conversion
                if (row.Cells["NgayDatHang"].Value != null)
                {
                    DateTime ngayDatHang;
                    if (DateTime.TryParse(row.Cells["NgayDatHang"].Value.ToString(), out ngayDatHang))
                    {
                        dateTimePicker1.Value = ngayDatHang;
                    }
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now; // Fallback if the cell is null
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            dateTimePicker1.Value = DateTime.Now;
            textBox7.Clear();
            textBox8.Clear();
            textBox1.ReadOnly = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy mã nhân viên từ hàng được chọn
                int maHoaDon = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaHoaDon"].Value);

                string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn              
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE HoaDon SET TenKhachHang = @TenKhachHang, DienThoai = @DienThoai, " +
                                       " TenSanPham = @TenSanPham, SoLuong = @SoLuong, ThanhTien = @ThanhTien, NgayDatHang= @NgayDatHang " +
                                       "WHERE MaHoaDon = @MaHoaDon";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                            command.Parameters.AddWithValue("@TenKhachHang", textBox2.Text);
                            command.Parameters.AddWithValue("@DienThoai", textBox4.Text);
                            command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                            command.Parameters.AddWithValue("@TenSanPham", textBox8.Text);
                            command.Parameters.AddWithValue("@SoLuong", textBox7.Text);
                            command.Parameters.AddWithValue("@ThanhTien", textBox5.Text);
                            command.Parameters.AddWithValue("@NgayDatHang", dateTimePicker1.Value.Date);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                                LoadData(); // Cập nhật DataGridView sau khi sửa
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy hóa đơn với mã đã cho.");
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
            textBox1.ReadOnly = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Kiểm tra xem có hàng nào được chọn không
            {
                // Lấy mã nhân viên từ hàng được chọn
                int maHoaDon = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaHoaDon"].Value);

                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn với mã " + maHoaDon + "?",
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
                            string query = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Dữ liệu đã được xóa thành công!");
                                    LoadData(); // Cập nhật DataGridView sau khi xóa
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy hóa đơn với mã đã cho.");
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

        private void button4_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            string connectionString = "server=.; database=QLYSach; Integrated Security=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO HoaDon (TenKhachHang, DienThoai, DiaChi, TenSanPham, SoLuong, ThanhTien, NgayDatHang) " +
                                   "VALUES (@TenKhachHang, @DienThoai, @DiaChi, @TenSanPham, @SoLuong, @ThanhTien, @NgayDatHang)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenKhachHang", textBox2.Text);
                        command.Parameters.AddWithValue("@DienThoai", textBox4.Text);
                        command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                        command.Parameters.AddWithValue("@TenSanPham", textBox8.Text);
                        command.Parameters.AddWithValue("@SoLuong", textBox7.Text);
                        command.Parameters.AddWithValue("@ThanhTien", textBox5.Text);
                        command.Parameters.AddWithValue("@NgayDatHang", dateTimePicker1.Value.Date);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dữ liệu đã được thêm thành công!");
                            LoadData(); // Refresh the DataGridView to show the new entry
                            ClearInputFields(); // Optional: Clear input fields after insertion
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi thêm dữ liệu.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        // Optional: Method to clear input fields after adding a record
        private void ClearInputFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            dateTimePicker1.Value = DateTime.Now;
            textBox7.Clear();
            textBox8.Clear();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

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
            frmKhachHangNV khnv = new frmKhachHangNV();
            khnv.Show();
            this.Hide();
        }
    }
}
