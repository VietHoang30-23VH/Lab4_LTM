using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace Lab4_webserver
{
    public partial class Bai7_AddFood : Form
    {
        private string tokentype;
        private string accesstoken;
        public Bai7_AddFood(string tokenType, string accessToken)
        {
            InitializeComponent();
            tokentype = tokenType;
            accesstoken = accessToken;
        }

        private readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://nt106.uitiot.vn")
        };

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFood.Clear();
            txtPrice.Clear();
            txtAddress.Clear();
            txtImage.Clear();
            txtDescribe.Clear();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://nt106.uitiot.vn/api/v1/monan/add";
            var requestData = new
            {
                ten_mon_an = txtFood.Text,
                gia = txtPrice.Text,
                mo_ta = txtDescribe.Text,
                hinh_anh = txtImage.Text,
                dia_chi = txtAddress.Text
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API và nhận phản hồi
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokentype, accesstoken);

            HttpResponseMessage responses = await httpClient.SendAsync(request);

            if (responses.IsSuccessStatusCode)
            {
                MessageBox.Show("Thêm món thành công");
            }
            else
            {
                string responseContent = await responses.Content.ReadAsStringAsync();
                MessageBox.Show("Thêm món thất bại: " + responseContent);
            }
        }
    }
}
