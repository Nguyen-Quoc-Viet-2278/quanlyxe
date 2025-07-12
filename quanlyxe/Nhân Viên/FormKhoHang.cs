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
    public partial class FormKhoHang : Form
    {
        string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
        public FormKhoHang()
        {
            InitializeComponent();
        }

        private void FormKhoHang_Load(object sender, EventArgs e)
        {
            LoadProducts();
            SetupDataGridView();
        }

        private void LoadProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MaSanPham, TenSanPham FROM SanPham", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxProducts.Items.Add(new { Value = reader["MaSanPham"], Text = reader["TenSanPham"] });
                }

                comboBoxProducts.DisplayMember = "Text";
                comboBoxProducts.ValueMember = "Value";
                reader.Close();
            }
        }
        private void SetupDataGridView()
        {
            dataGridViewStock.Columns.Clear();
            dataGridViewStock.Columns.Add("TenSanPham", "Tên Sản Phẩm");
            dataGridViewStock.Columns.Add("RemainingStock", "Số Lượng Tồn Kho");
            dataGridViewStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxProducts.SelectedItem != null)
            {
                int selectedProductId = (int)((dynamic)comboBoxProducts.SelectedItem).Value;
                int stock = GetStock(selectedProductId);
                int sold = GetSoldQuantity(selectedProductId);
                int remainingStock = stock - sold;

                // Thêm dòng mới vào DataGridView
                dataGridViewStock.Rows.Add(GetProductName(selectedProductId), remainingStock);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.");
            }
        }
        private int GetStock(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT SoLuong FROM SanPham WHERE MaSanPham = @MaSanPham", connection);
                command.Parameters.AddWithValue("@MaSanPham", productId);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetSoldQuantity(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT SUM(SoLuong) FROM HoaDon WHERE TenSanPham = (SELECT TenSanPham FROM SanPham WHERE MaSanPham = @MaSanPham)", connection);
                command.Parameters.AddWithValue("@MaSanPham", productId);
                object result = command.ExecuteScalar();
                return result != DBNull.Value ? (int)result : 0;
            }
        }
        private string GetProductName(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TenSanPham FROM SanPham WHERE MaSanPham = @MaSanPham", connection);
                command.Parameters.AddWithValue("@MaSanPham", productId);
                return (string)command.ExecuteScalar();
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

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
