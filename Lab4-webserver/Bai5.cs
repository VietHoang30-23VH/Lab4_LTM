using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
namespace Lab4_webserver
{
    public partial class Bai5 : Form
    {
        public Bai5()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtUsername.Text))&& (!string.IsNullOrEmpty(txtPassword.Text)))
            { 
                var url = txtUrl.Text.Trim();
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text.Trim();
                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent
                    {
                        {new StringContent(username),"username"},
                        {new StringContent(password),"password"}
                    };
                    try
                    {
                        var response = await client.PostAsync(url, content);
                        var responseStr = await response.Content.ReadAsStringAsync();
                        var responseObj = JObject.Parse(responseStr);
                        if (!response.IsSuccessStatusCode)
                        {
                            var detail = responseObj["detail"].ToString();
                            rtbInformation.AppendText($"Detail:{detail}\n");
                            return;
                        }
                        var tokenType = responseObj["token_type"]?.ToString();
                        var accessType = responseObj["access_token"]?.ToString();
                        rtbInformation.AppendText($"Token:{tokenType}\nAccess:{accessType}\nĐăng nhập thành công!");
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else
            {
                MessageBox.Show("Nhập tài khoản và mật khẩu!");
                return;
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            rtbInformation.Clear();
        }
    }
}
