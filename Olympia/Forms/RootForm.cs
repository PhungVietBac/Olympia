using System;
using System.Windows.Forms;

namespace Olympia.Forms {
    public partial class RootForm : Form {
        public RootForm() {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e) {
            Visible = false;
            LogIn logIn = new LogIn(this);
            logIn.ShowDialog();
        }

        private void btnDangKy_Click(object sender, EventArgs e) {
            Visible = false;
            SignUp signUp = new SignUp(this);
            signUp.ShowDialog();
        }
    }
}
