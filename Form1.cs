using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SportSchool {
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string userlogin = "user";
            string userpassword = "user";
            string adminlogin = "admin";
            string adminpassword = "admin";

            string input1 = textBox1.Text.Trim();
            string input2 = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                MessageBox.Show("Поля не могут быть пустыми!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidInput(input1) || !IsValidInput(input2))
            {
                MessageBox.Show("Введены некорректные данные! Используйте только буквы.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            input1 = input1.ToLower();
            input2 = input2.ToLower();

            if (input1 == userlogin && input2 == userpassword)
            {
                this.Hide();
                User userForm = new User();
                userForm.ShowDialog();
                this.Show();
                textBox1.Clear();
                textBox2.Clear();
            }
            else if (input1 == adminlogin && input2 == adminpassword)
            {
                this.Hide();
                admin adminForm = new admin();
                adminForm.ShowDialog();
                this.Show();
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Завершаем приложение при любом закрытии Form1
        }
    }
}