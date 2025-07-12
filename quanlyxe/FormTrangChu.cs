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
using System.IO;

namespace quanlyxe
{
    public partial class FormTrangChu : Form
    {
        string connectionString = "server=.; database=QLYSach; Integrated Security=true;"; // Thay thế theo cấu hình của bạn
        public FormTrangChu(string role)
        {
            InitializeComponent();
            LoadProducts();
            

            // Hide MenuStrip based on user role
            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("ad", StringComparison.OrdinalIgnoreCase))
            {
               menuStrip1.Visible = true; 
            }
            else if (role.Equals("KhachHang", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("kh", StringComparison.OrdinalIgnoreCase))
            {
                menuStrip1.Visible = true; // Hide menu for employees and member customers
            }
        }

        //

        private void LoadProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT HinhAnh, TenSanPham, Gia, ChiTiet FROM SanPham", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string hinhAnhPath = reader["HinhAnh"].ToString();
                    string tenSanPham = reader["TenSanPham"].ToString();
                    decimal gia = (decimal)reader["Gia"];
                    string chiTiet = reader["ChiTiet"].ToString();

                    // Check if the image file exists
                    if (!File.Exists(hinhAnhPath))
                    {
                        MessageBox.Show($"File not found: {hinhAnhPath}");
                        continue;
                    }

                    // Create a panel for each product
                    Panel productPanel = CreateProductPanel(hinhAnhPath, tenSanPham, gia, chiTiet);
                    flowLayoutPanel1.Controls.Add(productPanel);
                }
            }
        }

        private Panel CreateProductPanel(string hinhAnhPath, string tenSanPham, decimal gia, string chiTiet)
        {
            Panel productPanel = new Panel
            {
                Width = 140,
                Height = 180,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 120,
                Height = 120,
                Dock = DockStyle.Top,
                Image = Image.FromFile(hinhAnhPath) // Load image
            };

            // Attach click event to the PictureBox
            pictureBox.Click += (sender, e) => ShowProductDetails(tenSanPham, gia, hinhAnhPath, chiTiet);

            Label nameLabel = new Label
            {
                Text = tenSanPham,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            Label priceLabel = new Label
            {
                Text = $"Giá: {gia:N0} VNĐ",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30,
                ForeColor = Color.Red
            };

            // Add PictureBox and Labels to the panel
            productPanel.Controls.Add(pictureBox);
            productPanel.Controls.Add(nameLabel);
            productPanel.Controls.Add(priceLabel);

            return productPanel;
        }

        private void ShowProductDetails(string tenSanPham, decimal gia, string hinhAnhPath, string chiTiet)
        {
            Form detailsForm = new Form
            {
                Width = 300,
                Height = 500,
                Text = tenSanPham
            };

            Label nameLabel = new Label
            {
                Text = $"Tên sản phẩm: {tenSanPham}",
                AutoSize = true,
                Location = new Point(10, 10)
            };

            Label priceLabel = new Label
            {
                Text = $"Giá: {gia:N0} VNĐ",
                AutoSize = true,
                Location = new Point(10, 40),
                ForeColor = Color.Red
            };

            Label detailLabel = new Label
            {
                Text = $"Chi tiết: {chiTiet}",
                AutoSize = true,
                MaximumSize = new Size(250, 100),
                Location = new Point(10, 70)
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = Image.FromFile(hinhAnhPath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 250,
                Height = 250,
                Location = new Point(10, 100)
            };

            // Create "Mua Hàng" button
            Button buyButton = new Button
            {
                Text = "Mua Hàng",
                Location = new Point(10, 360),
                Width = 120
            };
            buyButton.Click += (sender, e) =>
            {
                NhapThongTinMuaHang purchaseForm = new NhapThongTinMuaHang(tenSanPham, gia, hinhAnhPath);
                purchaseForm.Show();
                this.Hide(); // Optionally hide the current form
                this.Close();
            };

            // Create "Thêm vào Giỏ Hàng" button
            Button addToCartButton = new Button
            {
                Text = "Thêm vào Giỏ Hàng",
                Location = new Point(140, 360),
                Width = 120
            };
            addToCartButton.Click += (sender, e) =>
            {
                // Logic for adding to cart
                MessageBox.Show($"{tenSanPham} đã được thêm vào giỏ hàng.");
            };

            // Add controls to the details form
            detailsForm.Controls.Add(nameLabel);
            detailsForm.Controls.Add(priceLabel);
            detailsForm.Controls.Add(detailLabel);
            detailsForm.Controls.Add(pictureBox);
            detailsForm.Controls.Add(buyButton);
            detailsForm.Controls.Add(addToCartButton);

            detailsForm.ShowDialog(); // Show as a dialog
        }


        private void PictureBox_Click(object sender, EventArgs e)
        {
            
        }
        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FormNhanVien NhanVien = new FormNhanVien();
            NhanVien.Show();
            this.Hide();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FormKhachHang KhachHang = new FormKhachHang();
            KhachHang.Show();
            this.Hide();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SanPham sp = new SanPham();
            sp.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void tìmKiếmToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void tìmKiếmToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            TimKiem tk = new TimKiem();
            tk.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void đăngXuấtToolStripMenuItem_Click_1(object sender, EventArgs e)
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void hóaĐơnToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon();
            hd.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
