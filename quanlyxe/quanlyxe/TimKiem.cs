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
    public partial class TimKiem : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=QLYXE;Integrated Security=True;";
        public TimKiem()
        {
            InitializeComponent();
        }

        private void TimKiem_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Nhân viên");
            comboBox1.Items.Add("Khách hàng");
            comboBox1.Items.Add("Hóa đơn");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ma = textBox1.Text;
            string ten = textBox2.Text;
            string bang = comboBox1.SelectedItem.ToString();

            // Tạo một DataTable để lưu kết quả tìm kiếm
            DataTable result = new DataTable();

            // Câu truy vấn SQL để tìm kiếm theo bảng, mã và tên
            string query = "";

            if (bang == "Nhân viên")
            {
                query = "SELECT * FROM NhanVien WHERE MaNhanVien LIKE @Ma AND TenNhanVien LIKE @Ten";
            }
            else if (bang == "Khách hàng")
            {
                query = "SELECT * FROM KhachHang WHERE MaKhachHang LIKE @Ma AND TenKhachHang LIKE @Ten";
            }
            else if (bang == "Hóa đơn")
            {
                query = "SELECT * FROM DonDatHang WHERE MaDonHang LIKE @Ma AND TenKhachHang LIKE @Ten";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ma", "%" + ma + "%");
                    command.Parameters.AddWithValue("@Ten", "%" + ten + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(result);
                    }
                }
            }

            dataGridView1.DataSource = result;
        }
    }
}
