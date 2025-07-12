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
    public partial class SanPham : Form
    {
        private string imagePath;
        public SanPham()
        {

            InitializeComponent();
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            LoadTenHang(); // Gọi phương thức để tải tên hãng
            LoadData(); // Gọi phương thức để tải dữ liệu sản phẩm
        }
        private void LoadTenHang()
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TenHang FROM HangSanPham";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBoxTenHang.Items.Clear();

                    while (reader.Read())
                    {
                        comboBoxTenHang.Items.Add(reader["TenHang"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate text boxes with data from the selected row
                textBox1.Text = row.Cells["MaSanPham"].Value.ToString();
                textBox2.Text = row.Cells["TenSanPham"].Value.ToString();
                textBox3.Text = row.Cells["Gia"].Value.ToString();
                comboBoxTenHang.SelectedItem = row.Cells["TenHang"].Value.ToString();
                textBox5.Text = row.Cells["SoLuong"].Value.ToString();
                // Populate ChiTiet TextBox
                textBox4.Text = row.Cells["ChiTiet"].Value.ToString();

                // Get the image path and display the image
                imagePath = row.Cells["HinhAnh"].Value.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath)) // Check if the image path exists
                {
                    pictureBox1.Image = Image.FromFile(imagePath); // Display the image
                }
                else
                {
                    pictureBox1.Image = null; // Clear the PictureBox if no valid image is found
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO SanPham (MaSanPham, TenSanPham, Gia, SoLuong, TenHang, HinhAnh, ChiTiet) " +
                                   "VALUES (@MaSanPham, @TenSanPham, @Gia, @SoLuong, @TenHang, @HinhAnh, @ChiTiet)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaSanPham", int.Parse(textBox1.Text));
                        command.Parameters.AddWithValue("@TenSanPham", textBox2.Text);
                        command.Parameters.AddWithValue("@Gia", decimal.Parse(textBox3.Text));
                        command.Parameters.AddWithValue("@SoLuong", int.Parse(textBox5.Text));
                        command.Parameters.AddWithValue("@TenHang", comboBoxTenHang.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@HinhAnh", imagePath); // Save image path
                        command.Parameters.AddWithValue("@ChiTiet", textBox4.Text); // Add ChiTiet parameter

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
            string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM SanPham";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Ensure that ChiTiet is included in the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int maSanPham = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaSanPham"].Value);
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa Sản Phẩm với mã " + maSanPham + "?",
                                               "Xác nhận xóa",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "DELETE FROM SanPham WHERE MaSanPham = @MaSanPham";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaSanPham", maSanPham);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Dữ liệu đã được xóa thành công!");
                                    LoadData();
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy Sản Phẩm với mã đã cho.");
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int maSanPham = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaSanPham"].Value);
                string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE SanPham SET TenSanPham = @TenSanPham, Gia = @Gia, TenHang = @TenHang, @SoLuong = @SoLuong, HinhAnh = @HinhAnh, ChiTiet = @ChiTiet " +
                                       "WHERE MaSanPham = @MaSanPham";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaSanPham", maSanPham);
                            command.Parameters.AddWithValue("@TenSanPham", textBox2.Text);
                            command.Parameters.AddWithValue("@Gia", decimal.Parse(textBox3.Text));
                            command.Parameters.AddWithValue("@TenHang", comboBoxTenHang.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@SoLuong", textBox5.Text);
                            command.Parameters.AddWithValue("@HinhAnh", imagePath); // Update image path
                            command.Parameters.AddWithValue("@ChiTiet", textBox4.Text); // Update ChiTiet

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy Sản phẩm với mã đã cho.");
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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            imagePath = null; // Reset image path
            pictureBox1.Image = null; // Clear the PictureBox
            textBox1.ReadOnly = false; // Bỏ trạng thái chỉ đọc
        }

        private void buttonBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName; // Store the selected image path
                    pictureBox1.Image = Image.FromFile(imagePath); // Display the image
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName; // Lưu đường dẫn hình ảnh đã chọn
                    pictureBox1.Image = Image.FromFile(imagePath); // Hiển thị hình ảnh trong PictureBox
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FormNhanVien nv = new FormNhanVien();
            nv.Show();
            this.Hide();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FormKhachHang kh = new FormKhachHang();
            kh.Show();
            this.Hide();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string userRole = "admin"; // Lấy role từ nơi bạn lưu trữ
            FormTrangChu tc = new FormTrangChu(userRole);
            tc.Show();
            this.Hide();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimKiem tk = new TimKiem();
            tk.Show();
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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
