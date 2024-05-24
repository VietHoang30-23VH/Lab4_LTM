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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab4_webserver
{
    public partial class Bai7 : Form
    {
        private string receivedTokenType;
        private string receivedAccessToken;
        private int count = 0;
        List<string> thongtin = new List<string>();
        private string in4 = "";
        private int dem = 0;
        public Bai7(string tokentype, string accesstoken)
        {
            InitializeComponent();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            receivedTokenType = tokentype;
            receivedAccessToken = accesstoken;
            Hienthimonan();
        }

        private readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(@"https://nt106.uitiot.vn")
        };

        public class MonAnResponse
        {
            public List<MonAn> Data { get; set; }
        }

        public class MonAn
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("ten_mon_an")]
            public string TenMonAn { get; set; }

            [JsonProperty("gia")]
            public decimal Gia { get; set; }

            [JsonProperty("mo_ta")]
            public string MoTa { get; set; }

            [JsonProperty("hinh_anh")]
            public string HinhAnh { get; set; }

            [JsonProperty("dia_chi")]
            public string DiaChi { get; set; }

            [JsonProperty("nguoi_dong_gop")]
            public string NguoiDongGop { get; set; }
        }

        public async void Hienthimonan()
        {
            string apiUrl = "https://nt106.uitiot.vn/api/v1/monan/all";
            var requestData = new
            {
                current = 1,
                pageSize = 5
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API và nhận phản hồi
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(receivedTokenType, receivedAccessToken);

            HttpResponseMessage responses = await httpClient.SendAsync(request);

            if (responses.IsSuccessStatusCode)
            {
                string responseContent = await responses.Content.ReadAsStringAsync();
                MonAnResponse responseObject = JsonConvert.DeserializeObject<MonAnResponse>(responseContent);

                foreach (var monAn in responseObject.Data)
                {
                    string monan = monAn.TenMonAn;
                    string gia = monAn.Gia.ToString();
                    string diachi = monAn.DiaChi;
                    string nguoidonggop = monAn.NguoiDongGop;
                    string imgurl = monAn.HinhAnh;
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Load(imgurl);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    string information = monan + "," + gia + "," + diachi + "," + nguoidonggop + "," + imgurl;
                    thongtin.Add(information);
                    Addprogressbar(monan, gia, diachi, nguoidonggop, pictureBox);
                }
            }
        }

        private void Addprogressbar(string monan, string gia, string diachi, string nguoidonggop, PictureBox pictureBox)
        {
            Bai7_Food hienthi = new Bai7_Food();
            hienthi.Settenmonan(monan);
            hienthi.Setgiamonan(gia);
            hienthi.Setdiachi(diachi);
            hienthi.Setnguoidonggop(nguoidonggop);
            hienthi.SetPictureBox(pictureBox);
            flowLayoutPanel1.Controls.Add(hienthi);
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Control randomControl = GetRandomControl();
            if (randomControl != null)
            {
                Bai7_DisplayFood form2 = new Bai7_DisplayFood(randomControl, in4, dem);
                form2.Show();
                dem++;
            }
            else
            {
                MessageBox.Show("Không có món để lựa chọn.");
            }
        }

        public Control GetRandomControl()
        {
            Control randomControl = null;
            List<Control> allControls = flowLayoutPanel1.Controls.Cast<Control>().ToList();

            if (allControls.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, allControls.Count);
                randomControl = allControls[randomIndex];
                in4 = thongtin[randomIndex];
            }
            return randomControl;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Bai7_AddFood them = new Bai7_AddFood(receivedTokenType, receivedAccessToken);
            them.Show();
        }

        private async void btnAll_Click(object sender, EventArgs e)
        {
            count = 0;
            await HandleRequest();
        }

        private async void btnContributor_Click(object sender, EventArgs e)
        {
            count = 1;
            await HandleRequest();
        }

        private async void cbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleRequest();
        }

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            await HandleRequest();
        }

        private async Task HandleRequest()
        {
            if (cbPageSize.SelectedItem == null || cbPage.SelectedItem == null)
                return;

            int currentPage = int.Parse(cbPage.SelectedItem.ToString());
            int pageSize = int.Parse(cbPageSize.SelectedItem.ToString());

            string apiUrl = (count == 0) ? "https://nt106.uitiot.vn/api/v1/monan/all" : "https://nt106.uitiot.vn/api/v1/monan/my-dishes";
            await SendRequest(apiUrl, currentPage, pageSize);
        }

        private async Task SendRequest(string apiUrl, int currentPage, int pageSize)
        {
            var requestData = new
            {
                current = currentPage,
                pageSize = pageSize,
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(receivedTokenType, receivedAccessToken);

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                MonAnResponse responseObject = JsonConvert.DeserializeObject<MonAnResponse>(responseContent);

                flowLayoutPanel1.Controls.Clear();
                thongtin.Clear();

                foreach (var monAn in responseObject.Data)
                {
                    string monan = monAn.TenMonAn;
                    string gia = monAn.Gia.ToString();
                    string diachi = monAn.DiaChi;
                    string nguoidonggop = monAn.NguoiDongGop;
                    string imgurl = monAn.HinhAnh;
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Load(imgurl);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    string information = $"{monan},{gia},{diachi},{nguoidonggop},{imgurl}";
                    thongtin.Add(information);
                    Addprogressbar(monan, gia, diachi, nguoidonggop, pictureBox);
                }
            }
        }
    }
}
