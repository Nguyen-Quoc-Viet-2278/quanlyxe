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
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        public static class UserSession
        {
            public static string Role { get; set; }
        }


        string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn
        private void button1_Click(object sender, EventArgs e)
        {

            string username = textBox1.Text;
            string password = textBox2.Text;


            //nhan vien
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MatKhau, Role FROM NhanVien WHERE TenTaiKhoan = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string dbPassword = reader["MatKhau"].ToString();
                    string accountType = reader["Role"].ToString(); // Có thể xác định quyền của nhân viên từ đây

                    if (password == dbPassword)
                    {
                        HoaDon mainForm = new HoaDon();
                        this.Hide();
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu không chính xác.");
                    }
                }
            }

            //khach hang
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MatKhau, Role FROM KhachHang WHERE TaiKhoan = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string dbPassword = reader["MatKhau"].ToString();
                    string accountType = reader["Role"].ToString(); // Lấy vai trò của khách hàng

                    if (password == dbPassword)
                    {
                        FormTrangChu mainForm = new FormTrangChu(accountType);
                        this.Hide();
                        mainForm.Show();

                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu không chính xác.");
                    }
                }
            }

            //admin
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MatKhau, Role FROM TaiKhoan WHERE TenTaiKhoan = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string dbPassword = reader["MatKhau"].ToString();
                    string accountType = reader["Role"].ToString(); // Lấy vai trò của khách hàng

                    if (password == dbPassword)
                    {
                        FormTrangChu mainForm = new FormTrangChu(accountType);
                        this.Hide();
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu không chính xác.");
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

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKy dk = new DangKy();
            dk.Show();
            this.Hide();
        }
    }
}
