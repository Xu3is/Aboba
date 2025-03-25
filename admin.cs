using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportSchool
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы точно хотите выйти?", "Подтверждение выхода",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); // Возвращаемся на Form1
            }
        }

        private void admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Завершаем приложение при любом закрытии admin
        }
    }
}