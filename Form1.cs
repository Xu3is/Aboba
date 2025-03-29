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

namespace SportSchool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Подписка на событие PreviewKeyDown для textBox1
            textBox1.PreviewKeyDown += textBox1_PreviewKeyDown;

            // Подписка на событие PreviewKeyDown для textBox2
            textBox2.PreviewKeyDown += textBox2_PreviewKeyDown;
            textBox2.KeyDown += textBox2_KeyDown;
        }

        // Обработка события для textBox1, чтобы разрешить переход только на textBox2 по Tab
        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Разрешаем переход только по Tab
            if (e.KeyCode == Keys.Tab)
            {
                // Позволяем переходить на textBox2 по Tab
                e.IsInputKey = false;
            }
            else
            {
                // Блокируем любые другие переходы
                e.IsInputKey = true;
            }
        }

        // Обработка для textBox2, чтобы запрещать переходы (включая Tab)
        private void textBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Блокируем любые попытки перемещения из textBox2
            e.IsInputKey = true;
        }

        // Переключение на button1 при нажатии Enter в textBox2
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Проверка на Enter
            {
                button1.PerformClick(); // Имитируем клик по кнопке
                e.Handled = true; // Останавливаем дальнейшую обработку нажатия Enter
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

        private void button1_Click(object sender, EventArgs e)
        {
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

            if (input1 == adminlogin && input2 == adminpassword)
            {
                this.Hide();
                admin adminForm = new admin();
                adminForm.ShowDialog();
                this.Close(); // Закрываем Form1 после входа в admin
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


