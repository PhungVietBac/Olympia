﻿using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Unidecode.NET;
using System.Net.Mail;
using System.Security.Cryptography;
using Olympia.Models;

namespace Olympia.Forms {
    public partial class SignUp : Form {
        private bool isOKName = false;
        private bool isOKUsername = false;
        private bool isOKPassword = false;
        private bool isOKRePassword = false;
        private bool isOKEmail = false;
        private bool isOKPhone = false;
        private bool isOKGender = false;
        private bool showPass = true;
        private bool showRePass = true;
        private bool isEmailVerify;
        private RootForm _rootForm { get; set; }
        public SignUp(RootForm rootForm) {
            InitializeComponent();
            _rootForm = rootForm;
        }

        // Quay về Root Form
        private void BackToRootForm(object sender, EventArgs args) {
            Close();
            _rootForm.Visible = true;
        }

        // Phương thức băm mật khẩu bằng SHA-256
        private string HashPassword(string password) {
            password += "group17";
            // Tạo đối tượng SHA256
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Chuyển đến form xác thực email
        private void btnSubmit_Click(object sender, EventArgs e) {
            if (isOKName && isOKUsername && isOKPassword && isOKRePassword && isOKEmail && isOKPhone && isOKGender) {

                VerifyEmail verifyEmail = new VerifyEmail();
                verifyEmail.email = tbEmail.Text;

                //băm password
                string pw = HashPassword(tbPassword.Text);
                verifyEmail.ShowDialog();
                isEmailVerify = verifyEmail.IsValidEmail();
                if (isEmailVerify) {
                    CreateAvatar createAvatar = new CreateAvatar();
                    createAvatar.newPlayer = new PlayerSignUp();
                    createAvatar.newPlayer.Name = tbName.Text;
                    createAvatar.newPlayer.Username = tbUsername.Text;
                    createAvatar.newPlayer.Password = pw.Trim();
                    int gender = cbbGender.SelectedIndex;
                    if (gender == 0)
                        createAvatar.newPlayer.Gender = Gender.Male;
                    else if (gender == 1)
                        createAvatar.newPlayer.Gender = Gender.Female;
                    else
                        createAvatar.newPlayer.Gender = Gender.Other;
                    createAvatar.newPlayer.Email = tbEmail.Text;
                    createAvatar.newPlayer.PhoneNumber = tbPhone.Text;
                    Close();
                    createAvatar.Show();
                }
            } else {
                MessageBox.Show("Thông tin chưa hợp lệ", "Lỗi tạo tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kiểm tra điều kiện của "Nhập mật khẩu"
        private void PasswordTextbox(object sender, EventArgs e) {
            tbPassword.MaxLength = 15;
            if (showPass)
                tbPassword.PasswordChar = '●';
            else
                tbPassword.PasswordChar = '\0';
            if (tbPassword.Text == "") {
                lblAlertPassword.Text = "ⓘ Mật khẩu không được để trống";
                lblAlertPassword.Visible = true;
                isOKPassword = false;
            } else {
                foreach (char c in tbPassword.Text) {
                    if (c == ' ') {
                        lblAlertPassword.Text = "ⓘ Mật khẩu không được chứa dấu cách";
                        lblAlertPassword.Visible = true;
                        isOKPassword = false;
                        break;
                    } else
                        lblAlertPassword.Visible = false;
                }
                isOKPassword = true;
            }
        }

        // Kiểm tra điều kiện của "Nhập lại mật khẩu"
        private void RePasswordTextbox(object sender, EventArgs e) {
            tbRePass.MaxLength = 20;
            if (showRePass)
                tbRePass.PasswordChar = '●';
            else
                tbRePass.PasswordChar = '\0';
            if (tbRePass.Text == "") {
                lblAlertRePass.Text = "ⓘ Mật khẩu không được để trống";
                lblAlertRePass.Visible = true;
                isOKRePassword = false;
            } else {
                if (tbRePass.Text != tbPassword.Text) {
                    lblAlertRePass.Text = "ⓘ Mật khẩu không trùng khớp";
                    lblAlertRePass.Visible = true;
                    isOKRePassword = false;
                } else {
                    lblAlertRePass.Visible = false;
                    isOKRePassword = true;
                }
            }
        }

        // Kiểm tra điều kiện của "Giới tính"
        private void cbbGender_TextChanged(object sender, EventArgs e) {
            ComboBox cbb = (ComboBox)sender;
            int index = cbb.FindString(cbb.Text.Trim());
            if (index < 0) {
                lblAlertGender.Text = "ⓘ Lựa chọn không phù hợp";
                lblAlertGender.Visible = true;
                isOKGender = false;
            } else {
                lblAlertGender.Visible = false;
                isOKGender = true;
            }
        }

        // Kiểm tra điều kiện của "Họ và tên"
        private void tbName_TextChanged(object sender, EventArgs e) {
            if (tbName.Text == "") {
                lblAlertName.Text = "ⓘ Tên không được để trống";
                lblAlertName.Visible = true;
                isOKName = false;
            } else {
                string temp = tbName.Text.Replace(" ", "");
                foreach (char c in temp.Unidecode()) {
                    if (!(c >= 'A' && c <= 'Z') && !(c >= 'a' && c <= 'z')) {
                        lblAlertName.Text = "ⓘ Chỉ được chứa các kí tự chữ";
                        lblAlertName.Visible = true;
                        isOKName = false;
                    } else
                        lblAlertName.Visible = false;
                }
                isOKName = true;
            }
        }

        // Hiển thị hoặc không hiển thị mật khẩu trong "Nhập mật khẩu"
        private void ptbPassword_Click(object sender, EventArgs e) {
            if (showPass) {
                tbPassword.PasswordChar = '\0';
                ptbPassword.Image = Properties.Resources.HidePass;
                showPass = false;
            } else {
                tbPassword.PasswordChar = '●';
                ptbPassword.Image = Properties.Resources.ShowPass;
                showPass = true;
            }
        }

        // Hiển thị hoặc không hiển thị mật khẩu trong "Nhập lại mật khẩu"
        private void ptbRePass_Click(object sender, EventArgs e) {
            if (showRePass) {
                tbRePass.PasswordChar = '\0';
                ptbRePass.Image = Properties.Resources.HidePass;
                showRePass = false;
            } else {
                tbRePass.PasswordChar = '●';
                ptbRePass.Image = Properties.Resources.ShowPass;
                showRePass = true;
            }
        }

        // Kiểm tra điều kiện của "Username"
        private async void tbUsername_TextChanged(object sender, EventArgs e) {
            if (tbUsername.Text == "") {
                lblAlertUsername.Text = "ⓘ Username không được để trống";
                lblAlertUsername.Visible = true;
                isOKUsername = false;
            } else {
                using (HttpClient client = new HttpClient()) {
                    try {
                        string url = "https://olympiawebservice.azurewebsites.net/api/Player/username?lookup=" + tbUsername.Text.Trim();
                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode) {
                            lblAlertUsername.Text = "ⓘ Username đã tồn tại";
                            lblAlertUsername.Visible = true;
                            isOKUsername = false;
                        } else {
                            lblAlertUsername.Visible = false;
                            isOKUsername = true;
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        // Kiểm tra định dạng của email
        private bool IsValidEmail(string email) {
            try {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            } catch (FormatException) {
                return false;
            }
        }

        // Kiểm tra điều kiện của "Email"
        private async void tbEmail_TextChangedAsync(object sender, EventArgs e) {
            if (tbEmail.Text == "") {
                lblAlertEmail.Text = "ⓘ Email không được để trống";
                lblAlertEmail.Visible = true;
                isOKEmail = false;
            } else {
                if (!IsValidEmail(tbEmail.Text)) {
                    lblAlertEmail.Text = "ⓘ Email không hợp lệ";
                    lblAlertEmail.Visible = true;
                    isOKEmail = false;
                } else {
                    using (HttpClient client = new HttpClient()) {
                        try {
                            string url = "https://olympiawebservice.azurewebsites.net/api/Player/email?lookup=" + tbEmail.Text.Trim();
                            var response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode) {
                                lblAlertEmail.Text = "ⓘ Email đã được sử dụng";
                                lblAlertEmail.Visible = true;
                                isOKEmail = false;
                            } else {
                                lblAlertEmail.Visible = false;
                                isOKEmail = true;
                            }
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        // Kiểm tra định dạng của số điện thoại
        private bool IsValidPhone(string phone) {
            if (phone.Length != 10)
                return false;
            foreach (char c in phone) {
                if (c < '0' || c > '9') {
                    return false;
                }
            }
            return true;
        }

        // Kiểm tra điều kiện của "Số điện thoại"
        private async void tbPhone_TextChanged(object sender, EventArgs e) {
            if (tbPhone.Text == "") {
                lblAlertPhone.Text = "ⓘ Số điện thoại không được để trống";
                lblAlertPhone.Visible = true;
                isOKPhone = false;
            } else {
                if (!IsValidPhone(tbPhone.Text)) {
                    lblAlertPhone.Text = "ⓘ Số điện thoại không hợp lệ";
                    lblAlertPhone.Visible = true;
                    isOKPhone = false;
                } else {
                    using (HttpClient client = new HttpClient()) {
                        try {
                            string url = "https://olympiawebservice.azurewebsites.net/api/Player/phone?lookup=" + tbPhone.Text.Trim();
                            var response = await client.GetAsync(url);
                            if (response.IsSuccessStatusCode) {
                                lblAlertPhone.Text = "ⓘ Số điện thoại đã được sử dụng";
                                lblAlertPhone.Visible = true;
                                isOKPhone = false;
                            } else {
                                lblAlertPhone.Visible = false;
                                isOKPhone = true;
                            }
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
