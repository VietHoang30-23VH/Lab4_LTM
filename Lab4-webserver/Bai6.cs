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
    public partial class Bai6 : Form
    {
        public Bai6()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtUsername.Text)) && (!string.IsNullOrEmpty(txtPassword.Text)))
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
                            rtbPOST.AppendText($"Detail:{detail}\n");
                            return;
                        }
                        var tokenType = responseObj["token_type"]?.ToString();
                        var accessType = responseObj["access_token"]?.ToString();
                        rtbPOST.AppendText($"Token:{tokenType}\nAccess:{accessType}\nĐăng nhập thành công!");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, accessType);
                        var getUserUrl = "https://nt106.uitiot.vn/api/v1/user/me";
                        var getUserResponse = await client.GetAsync(getUserUrl);
                        var getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();
                        rtbGet.AppendText(getUserResponseString);

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
            rtbPOST.Clear();
            rtbGet.Clear();
        }
    }
}
