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
    public partial class DangKy : Form
    {

        string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
        public DangKy()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
            txtConfirmMatKhau.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenKhachHang = txtTenKhachHang.Text;
            string diaChi = txtDiaChi.Text;
            string dienThoai = txtDienThoai.Text;
            string taiKhoan = txtTaiKhoan.Text;
            string matKhau = txtMatKhau.Text;
            string confirmMatKhau = txtConfirmMatKhau.Text;

            // Validate that the password and confirm password match
            if (matKhau != confirmMatKhau)
            {
                MessageBox.Show("Mật khẩu không khớp, vui lòng thử lại.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO KhachHang (TenKhachHang, DiaChi, DienThoai, TaiKhoan, MatKhau, Role) " +
                               "VALUES (@TenKhachHang, @DiaChi, @DienThoai, @TaiKhoan, @MatKhau, 'KhachHang')";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                command.Parameters.AddWithValue("@DiaChi", diaChi);
                command.Parameters.AddWithValue("@DienThoai", dienThoai);
                command.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                command.Parameters.AddWithValue("@MatKhau", matKhau); // You might want to hash this

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Đăng ký thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormDangNhap dn = new FormDangNhap();
            dn.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái của CheckBox
            if (checkBox1.Checked)
            {
                txtConfirmMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.UseSystemPasswordChar = false;// Hiện mật khẩu
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
                txtConfirmMatKhau.UseSystemPasswordChar = true;// Ẩn mật khẩu
            }
        }
    }
}
