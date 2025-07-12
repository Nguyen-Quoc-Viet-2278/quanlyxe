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
    public partial class NhapThongTinMuaHang : Form
    {
        string connectionString = "server=.; database=QLYSach; Integrated Security=true;";
        public NhapThongTinMuaHang(string tenSanPham, decimal gia, string hinhAnhPath)
        {
            InitializeComponent();

            // Set form title
            this.Text = $"Mua {tenSanPham}";

            // Create and set labels for product information
            Label nameLabel = new Label
            {
                Text = $"Tên sản phẩm: {tenSanPham}",
                AutoSize = true,
                Location = new Point(10, 100)
            };

            Label priceLabel = new Label
            {
                Text = $"Giá: {gia:N0} VNĐ",
                AutoSize = true,
                Location = new Point(10, 130),
                ForeColor = Color.Red
            };

            // Create PictureBox for product image
            PictureBox pictureBox = new PictureBox
            {
                Image = Image.FromFile(hinhAnhPath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 250,
                Height = 250,
                Location = new Point(10, 160)
            };

            // Create a NumericUpDown control for quantity selection
            Label quantityLabel = new Label
            {
                Text = "Số lượng:",
                Location = new Point(10, 420)
            };
            NumericUpDown quantityUpDown = new NumericUpDown
            {
                Location = new Point(120, 420),
                Minimum = 1, // Minimum quantity
                Maximum = 100, // Adjust as needed
                Value = 1 // Default value
            };

            // Label to display total price
            Label totalPriceLabel = new Label
            {
                Text = "Thành tiền: 0 VNĐ",
                AutoSize = true,
                Location = new Point(10, 460),
                ForeColor = Color.Blue
            };

            // Update total price when quantity changes
            quantityUpDown.ValueChanged += (sender, e) =>
            {
                decimal totalPrice = gia * quantityUpDown.Value;
                totalPriceLabel.Text = $"Thành tiền: {totalPrice:N0} VNĐ";
            };
            Button backButton = new Button
            {
                Text = "Quay lại trang chủ",
                Location = new Point(10, 490)
            };

            // Event handler for the back button click
            backButton.Click += (sender, e) =>
            {
                // Redirect back to the home page
                string userRole = "kh"; // Assuming you have the user role stored somewhere
                FormTrangChu homePage = new FormTrangChu(userRole);
                homePage.Show();
                this.Hide();
            };

            // Add the back button to the form
            this.Controls.Add(backButton);

            // Create and set labels and textboxes for customer information
            Label nameInputLabel = new Label { Text = "Tên:", Location = new Point(10, 10) };
            TextBox nameTextBox = new TextBox { Location = new Point(120, 10), Width = 150 };

            Label phoneLabel = new Label { Text = "Số điện thoại:", Location = new Point(10, 40) };
            TextBox phoneTextBox = new TextBox { Location = new Point(120, 40), Width = 150 };

            Label addressLabel = new Label { Text = "Địa chỉ:", Location = new Point(10, 70) };
            TextBox addressTextBox = new TextBox { Location = new Point(120, 70), Width = 150 };

            Button submitButton = new Button { Text = "Xác Nhận", Location = new Point(100, 490) };
            submitButton.Click += (sender, e) =>
            {
                // Lấy thông tin từ các textbox
                string customerName = nameTextBox.Text;
                string phoneNumber = phoneTextBox.Text;
                string address = addressTextBox.Text;
                int quantity = (int)quantityUpDown.Value;
                decimal totalPrice = gia * quantity; // Tính tổng tiền

                // Kết nối đến SQL Server
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO HoaDon (TenKhachHang, DienThoai, DiaChi, TenSanPham, SoLuong, ThanhTien) VALUES (@TenKhachHang, @DienThoai, @DiaChi, @TenSanPham, @SoLuong, @ThanhTien)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số vào câu lệnh
                        command.Parameters.AddWithValue("@TenKhachHang", customerName);
                        command.Parameters.AddWithValue("@DienThoai", phoneNumber);
                        command.Parameters.AddWithValue("@DiaChi", address);
                        command.Parameters.AddWithValue("@TenSanPham", tenSanPham); // Lưu tên sản phẩm
                        command.Parameters.AddWithValue("@SoLuong", quantity);
                        command.Parameters.AddWithValue("@ThanhTien", totalPrice);

                        // Thực thi câu lệnh
                        command.ExecuteNonQuery();
                    }
                }

                // Hiển thị thông báo và đóng form
                MessageBox.Show($"Cảm ơn {customerName} đã đặt hàng!\nTổng tiền: {totalPrice:N0} VNĐ");
                this.Close(); // Đóng form sau khi lưu dữ liệu

                string userRole = "kh"; // Lấy role từ nơi bạn lưu trữ
                FormTrangChu tc = new FormTrangChu(userRole);
                tc.Show();
                this.Hide();
            };

            // Add controls to the form
            this.Controls.Add(nameLabel);
            this.Controls.Add(priceLabel);
            this.Controls.Add(pictureBox);
            this.Controls.Add(quantityLabel);
            this.Controls.Add(quantityUpDown);
            this.Controls.Add(totalPriceLabel);
            this.Controls.Add(nameInputLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(phoneLabel);
            this.Controls.Add(phoneTextBox);
            this.Controls.Add(addressLabel);
            this.Controls.Add(addressTextBox);
            this.Controls.Add(submitButton);
        }
    }
}
