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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=.; database=QLYXE; Integrated Security=true;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM TaiKhoan WHERE TenTaiKhoan = @TenTaiKhoan AND MatKhau = @MatKhau";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenTaiKhoan", textBox1.Text);
                    command.Parameters.AddWithValue("@MatKhau", textBox2.Text);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 1)
                    {
                        MessageBox.Show("Đăng nhập thành công!");
                        FormTrangChu TrangChu = new FormTrangChu();
                        TrangChu.Show();
                        this.Hide();
                            
                        // Tiến tới form hoặc chức năng tiếp theo
                    }
                    else
                    {
                        MessageBox.Show("Tên tài khoản hoặc mật khẩu không hợp lệ.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái của CheckBox
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false; // Hiện mật khẩu
            }
            else
            {
                textBox2.UseSystemPasswordChar = true; // Ẩn mật khẩu
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
