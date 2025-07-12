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
            string connectionString = "server=.; database=QLYXE; Integrated Security=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TenHang FROM HangSanPham"; // Giả sử bạn chỉ cần tên hãng

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    comboBoxTenHang.Items.Clear(); // Xóa các mục cũ trong ComboBox

                    while (reader.Read())
                    {
                        comboBoxTenHang.Items.Add(reader["TenHang"].ToString()); // Thêm tên hãng vào ComboBox
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["MaSanPham"].Value.ToString();
                textBox2.Text = row.Cells["TenSanPham"].Value.ToString();
                textBox3.Text = row.Cells["Gia"].Value.ToString();

                // Thiết lập tên hãng trong ComboBox
                comboBoxTenHang.SelectedItem = row.Cells["TenHang"].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYXE; Integrated Security=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO SanPham (MaSanPham, TenSanPham, Gia, TenHang) " +
                                   "VALUES (@MaSanPham, @TenSanPham, @Gia, @TenHang)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaSanPham", int.Parse(textBox1.Text));
                        command.Parameters.AddWithValue("@TenSanPham", textBox2.Text);
                        command.Parameters.AddWithValue("@Gia", decimal.Parse(textBox3.Text));
                        command.Parameters.AddWithValue("@TenHang", comboBoxTenHang.SelectedItem.ToString()); // Lấy tên hãng từ ComboBox

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
            string connectionString = "server=.; database=QLYXE; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM SanPham";

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
                int maSanPham = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaSanPham"].Value);

                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa Sản Phẩm với mã " + maSanPham + "?",
                                               "Xác nhận xóa",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) // Nếu chọn "Có"
                {
                    string connectionString = "server=.; database=QLYXE; Integrated Security=true;"; // Thay thế theo cấu hình của bạn

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
                                    LoadData(); // Cập nhật DataGridView sau khi xóa
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

                string connectionString = "server=.; database=QLYXE; Integrated Security=true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE SanPham SET TenSanPham = @TenSanPham, Gia = @Gia, TenHang = @TenHang " +
                                       "WHERE MaSanPham = @MaSanPham";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaSanPham", maSanPham);
                            command.Parameters.AddWithValue("@TenSanPham", textBox2.Text);
                            command.Parameters.AddWithValue("@Gia", decimal.Parse(textBox3.Text));
                            command.Parameters.AddWithValue("@TenHang", comboBoxTenHang.SelectedItem.ToString()); // Cập nhật tên hãng

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
    }
}
