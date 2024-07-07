using Newtonsoft.Json.Linq;
using Olympia.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Olympia.Forms {
    public partial class TypeCode : Form {
        public Player player;
        public Image image;
        public string roomCode = "";
        public TcpClient client;
        public List<List<Player>> friendList;
        private bool isExit = false;
        public TypeCode() {
            InitializeComponent();
        }

        public bool getIsExit() {
            return isExit;
        }

        private async void btnEnter_Click(object sender, EventArgs e) {
            HttpClient httpClient = new HttpClient();
            try {
                string url = "https://olympiawebservice.azurewebsites.net/api/Room/" + tbRoomCode.Text;
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) {
                    var res = await response.Content.ReadAsStringAsync();
                    JObject keyValuePairs = JObject.Parse(res);
                    JToken roomRes = keyValuePairs["isFull"];
                    if (roomRes.Value<bool>()) {
                        MessageBox.Show("Phòng " + tbRoomCode.Text + " đã đủ người!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } else {
                        roomCode = tbRoomCode.Text;
                        Room pc = new Room();
                        pc.client = client;
                        pc.player = player;
                        pc.image = image;
                        pc.roomCode = roomCode;
                        pc.friendList = friendList;
                        pc.isAdmin = false;
                        pc.Show();
                        isExit = true;
                        Close();
                    }
                } else {
                    MessageBox.Show("Không tìm thấy phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
