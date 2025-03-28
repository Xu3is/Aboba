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
    public partial class User : Form
    {
        private GroupBox infoGroupBox;
        private Button lastClickedButton;

        public User()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1239, 772);
            SetupFormControls();
        }

        private void SetupFormControls()
        {
            Label titleLabel = new Label();
            titleLabel.Text = "Секции спортивной школы";
            titleLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            titleLabel.Location = new Point(40, 40);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            Button section1Button = new Button();
            section1Button.Text = "Футбол";
            section1Button.Font = new Font("Arial", 14);
            section1Button.Location = new Point(40, 100);
            section1Button.Size = new Size(200, 60);
            section1Button.Click += (s, e) => ShowSectionInfo(section1Button, "Футбол", "Иванов И.И.", "Пн, Ср, Пт: 16:00-18:00", "20 мест");
            this.Controls.Add(section1Button);

            Button section2Button = new Button();
            section2Button.Text = "Плавание";
            section2Button.Font = new Font("Arial", 14);
            section2Button.Location = new Point(40, 180);
            section2Button.Size = new Size(200, 60);
            section2Button.Click += (s, e) => ShowSectionInfo(section2Button, "Плавание", "Петров П.П.", "Вт, Чт: 15:00-17:00", "15 мест");
            this.Controls.Add(section2Button);

            Button section3Button = new Button();
            section3Button.Text = "Баскетбол";
            section3Button.Font = new Font("Arial", 14);
            section3Button.Location = new Point(40, 260);
            section3Button.Size = new Size(200, 60);
            section3Button.Click += (s, e) => ShowSectionInfo(section3Button, "Баскетбол", "Сидоров С.С.", "Сб, Вс: 10:00-12:00", "18 мест");
            this.Controls.Add(section3Button);

            Button aboutUsButton = new Button();
            aboutUsButton.Text = "Информация о нас";
            aboutUsButton.Font = new Font("Arial", 14);
            aboutUsButton.Location = new Point(40, 650);
            aboutUsButton.Size = new Size(200, 60);
            aboutUsButton.Click += (s, e) => ShowAboutUsInfo(aboutUsButton);
            this.Controls.Add(aboutUsButton);

            infoGroupBox = new GroupBox();
            infoGroupBox.Text = "Информация о секции";
            infoGroupBox.Location = new Point(300, 100);
            infoGroupBox.Size = new Size(900, 600);
            infoGroupBox.Visible = false;
            this.Controls.Add(infoGroupBox);

            button1.Font = new Font("Times New Roman", 16); // Увеличен шрифт
            button1.Location = new Point(990, 0); // Сдвинуто вправо: 1239 - 231 - 10 (отступ) = 998, отступ сверху 20
            button1.Size = new Size(231, 107); // Увеличен размер кнопки
            this.Controls.Add(button1);
        }

        private void ShowSectionInfo(Button clickedButton, string sectionName, string coach, string schedule, string slots)
        {
            if (lastClickedButton != null && lastClickedButton != clickedButton)
            {
                lastClickedButton.BackColor = SystemColors.Control;
            }
            clickedButton.BackColor = Color.LightGreen;
            lastClickedButton = clickedButton;

            infoGroupBox.Controls.Clear();
            infoGroupBox.Text = "Информация о секции";

            Label nameLabel = new Label();
            nameLabel.Text = $"Секция: {sectionName}";
            nameLabel.Font = new Font("Arial", 14);
            nameLabel.Location = new Point(20, 40);
            nameLabel.AutoSize = true;
            infoGroupBox.Controls.Add(nameLabel);

            Label coachLabel = new Label();
            coachLabel.Text = $"Тренер: {coach}";
            coachLabel.Font = new Font("Arial", 14);
            coachLabel.Location = new Point(20, 80);
            coachLabel.AutoSize = true;
            infoGroupBox.Controls.Add(coachLabel);

            Label scheduleLabel = new Label();
            scheduleLabel.Text = $"Расписание: {schedule}";
            scheduleLabel.Font = new Font("Arial", 14);
            scheduleLabel.Location = new Point(20, 120);
            scheduleLabel.AutoSize = true;
            infoGroupBox.Controls.Add(scheduleLabel);

            Label slotsLabel = new Label();
            slotsLabel.Text = $"Места: {slots}";
            slotsLabel.Font = new Font("Arial", 14);
            slotsLabel.Location = new Point(20, 160);
            slotsLabel.AutoSize = true;
            infoGroupBox.Controls.Add(slotsLabel);

            Button enrollButton = new Button();
            enrollButton.Text = "Записаться";
            enrollButton.Font = new Font("Arial", 14);
            enrollButton.Location = new Point(20, 500);
            enrollButton.Size = new Size(200, 60);
            enrollButton.Click += (s, e) => EnrollInSection(sectionName);
            infoGroupBox.Controls.Add(enrollButton);

            infoGroupBox.Visible = true;
        }

        private void ShowAboutUsInfo(Button clickedButton)
        {
            if (lastClickedButton != null && lastClickedButton != clickedButton)
            {
                lastClickedButton.BackColor = SystemColors.Control;
            }
            clickedButton.BackColor = Color.LightGreen;
            lastClickedButton = clickedButton;

            infoGroupBox.Controls.Clear();
            infoGroupBox.Text = "Информация о нас";

            Label aboutUsLabel = new Label();
            aboutUsLabel.Text = "Мы - спортивная школа 'Чемпион', основанная в 2010 году. " +
                                "Наша миссия - развитие физической культуры и спорта среди молодежи. " +
                                "Мы предлагаем занятия по различным видам спорта под руководством " +
                                "опытных тренеров. Присоединяйтесь к нам, чтобы стать сильнее и здоровее!";
            aboutUsLabel.Font = new Font("Arial", 14);
            aboutUsLabel.Location = new Point(20, 40);
            aboutUsLabel.Size = new Size(860, 200);
            aboutUsLabel.AutoSize = false;
            infoGroupBox.Controls.Add(aboutUsLabel);

            infoGroupBox.Visible = true;
        }

        private void EnrollInSection(string sectionName)
        {
            DialogResult result = MessageBox.Show($"Вы хотите записаться на секцию {sectionName}?",
                "Подтверждение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show($"Вы успешно записаны на секцию {sectionName}!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button1_MouseClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы точно хотите выйти?", "Подтверждение выхода",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Скрываем текущую форму User
            Form1 form1 = new Form1(); // Создаем экземпляр Form1
            form1.ShowDialog(); // Открываем Form1 как модальное окно
            this.Close(); // Закрываем форму User после закрытия Form1
        }
    }
}