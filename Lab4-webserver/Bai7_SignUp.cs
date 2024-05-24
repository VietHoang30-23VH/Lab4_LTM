using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace Lab4_webserver
{
    public partial class Bai7_SignUp : Form
    {
        private string x;
        public Bai7_SignUp()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtPhone.Clear();
            timeBirthday.Value = DateTime.Now;
            cbLanguage.Text = "";
            cbSex.Text = "";
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            x = (cbSex.SelectedItem.ToString() == "Male") ? "1" : "0";
            var userData = new
            {
                username = txtUsername.Text,
                email = txtEmail.Text,
                password = txtPassword.Text,
                first_name = txtFirstname.Text,
                last_name = txtLastname.Text,
                sex = x,
                birthday = timeBirthday.Text,
                language = cbLanguage.SelectedItem.ToString(),
                phone = txtPhone.Text
            };

            // Chuyển đổi dữ liệu thành chuỗi JSON
            string jsonData = JsonConvert.SerializeObject(userData);

            // Đường dẫn API
            string apiUrl = "https://nt106.uitiot.vn/api/v1/user/signup";

            // Tạo đối tượng HttpClient
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Tạo nội dung yêu cầu từ chuỗi JSON
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    // Thực hiện yêu cầu POST
                    var response = await httpClient.PostAsync(apiUrl, content);

                    // Đọc nội dung phản hồi
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Hiển thị mã phản hồi và nội dung phản hồi
                    Console.WriteLine($"HTTP Status Code: {response.StatusCode}");
                    Console.WriteLine($"Response Content: {responseContent}");
                    this.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
