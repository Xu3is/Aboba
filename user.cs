using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;

namespace SportSchool
{
    public partial class User : Form
    {
        private GroupBox infoGroupBox;
        private Button lastClickedButton;
        private string connectionString;
        private DataTable coursesTable;

        public User()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            AppDomain.CurrentDomain.SetData("DataDirectory", projectDirectory);

            try
            {
                var connection = ConfigurationManager.ConnectionStrings["Database1ConnectionString"];
                if (connection == null || string.IsNullOrEmpty(connection.ConnectionString))
                {
                    throw new Exception("Строка подключения 'Database1ConnectionString' не найдена в конфигурационном файле или пуста.");
                }
                connectionString = connection.ConnectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1239, 772);
            LoadCoursesFromDatabase();
            SetupFormControls();
        }

        private void LoadCoursesFromDatabase()
        {
            try
            {
                coursesTable = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            k.код AS [Код курса], 
                            k.название AS [Название], 
                            k.описание AS [Описание], 
                            k.возраст AS [Возраст], 
                            k.цена AS [Цена], 
                            k.длительность AS [Длительность],
                            t.ФИО AS [Тренер],
                            t.телефон AS [Телефон]
                        FROM курсы k
                        LEFT JOIN занятия z ON k.код = z.курс
                        LEFT JOIN тренеры t ON z.тренер = t.код";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(coursesTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из базы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupFormControls()
        {
            Label titleLabel = new Label();
            titleLabel.Text = "Спортивные программы";
            titleLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            titleLabel.Location = new Point(40, 40);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            int yPosition = 100;
            if (coursesTable != null && coursesTable.Rows.Count > 0)
            {
                foreach (DataRow row in coursesTable.Rows)
                {
                    string courseName = row["Название"].ToString();
                    string description = row["Описание"].ToString();
                    string coach = row["Тренер"] != DBNull.Value ? row["Тренер"].ToString() : "Не указан";
                    string phone = row["Телефон"] != DBNull.Value ? row["Телефон"].ToString() : "Не указан";

                    Button sectionButton = new Button();
                    sectionButton.Text = courseName;
                    sectionButton.Font = new Font("Arial", 14);
                    sectionButton.Location = new Point(40, yPosition);
                    sectionButton.Size = new Size(200, 60);
                    sectionButton.BackColor = Color.White;
                    sectionButton.FlatStyle = FlatStyle.Flat;
                    sectionButton.FlatAppearance.BorderSize = 2;
                    sectionButton.Click += (s, e) => ShowSectionInfo(sectionButton, courseName, coach, phone, description);
                    this.Controls.Add(sectionButton);

                    yPosition += 80;
                }
            }
            else
            {
                Label noDataLabel = new Label();
                noDataLabel.Text = "Нет доступных секций.";
                noDataLabel.Font = new Font("Arial", 14);
                noDataLabel.Location = new Point(40, yPosition);
                noDataLabel.AutoSize = true;
                this.Controls.Add(noDataLabel);
            }

            Button aboutUsButton = new Button();
            aboutUsButton.Text = "Информация о нас";
            aboutUsButton.Font = new Font("Arial", 14);
            aboutUsButton.Location = new Point(40, 610);
            aboutUsButton.Size = new Size(200, 60);
            aboutUsButton.BackColor = Color.White;
            aboutUsButton.FlatStyle = FlatStyle.Flat;
            aboutUsButton.FlatAppearance.BorderSize = 2;
            aboutUsButton.Click += (s, e) => ShowAboutUsInfo(aboutUsButton);
            this.Controls.Add(aboutUsButton);

            infoGroupBox = new GroupBox();
            infoGroupBox.Text = "Информация";
            infoGroupBox.Font = new Font("Arial", 18, FontStyle.Bold);
            infoGroupBox.Location = new Point(300, 100);
            infoGroupBox.Size = new Size(900, 600);
            infoGroupBox.Visible = false;
            infoGroupBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                using (Pen pen = new Pen(Color.Black, 3))
                {
                    g.DrawRectangle(pen, 0, 0, infoGroupBox.Width - 1, infoGroupBox.Height - 1);
                }
            };
            this.Controls.Add(infoGroupBox);

            button1.Text = "Войти как администратор";
            button1.Font = new Font("Arial", 14);
            button1.Location = new Point(990, 30); // Adjusted from 40 to 38 for visual alignment
            button1.Size = new Size(180, 60);
            button1.BackColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            this.Controls.Add(button1);
        }

        private void ShowSectionInfo(Button clickedButton, string sectionName, string coach, string phone, string description)
        {
            if (lastClickedButton != null && lastClickedButton != clickedButton)
            {
                lastClickedButton.BackColor = Color.White;
            }
            clickedButton.BackColor = Color.LightGreen;
            lastClickedButton = clickedButton;

            infoGroupBox.Controls.Clear();
            infoGroupBox.Text = "Информация";

            string ageGroup = "Не указана";
            foreach (DataRow row in coursesTable.Rows)
            {
                if (row["Название"].ToString() == sectionName)
                {
                    ageGroup = row["Возраст"].ToString();
                    break;
                }
            }

            Label ageGroupLabel = new Label();
            ageGroupLabel.Text = $"Возрастная группа: {ageGroup}";
            ageGroupLabel.Font = new Font("Arial", 14);
            ageGroupLabel.Location = new Point(20, 40);
            ageGroupLabel.AutoSize = true;
            infoGroupBox.Controls.Add(ageGroupLabel);

            Label coachLabel = new Label();
            coachLabel.Text = $"Тренер: {coach}";
            coachLabel.Font = new Font("Arial", 14);
            coachLabel.Location = new Point(20, 70);
            coachLabel.AutoSize = true;
            infoGroupBox.Controls.Add(coachLabel);

            Label phoneLabel = new Label();
            phoneLabel.Text = $"Номер телефона тренера: {phone}";
            phoneLabel.Font = new Font("Arial", 14);
            phoneLabel.Location = new Point(20, 100);
            phoneLabel.AutoSize = true;
            infoGroupBox.Controls.Add(phoneLabel);

            // Standardize description punctuation
            description = description.TrimEnd('.').Trim(); // Remove trailing periods and whitespace
            if (!description.EndsWith("."))
            {
                description += "."; // Add a single period if none exists
            }

            Label descriptionLabel = new Label();
            descriptionLabel.Text = $"Описание: {description}";
            descriptionLabel.Font = new Font("Arial", 14);
            descriptionLabel.Location = new Point(20, 130);
            descriptionLabel.Size = new Size(860, 120);
            descriptionLabel.AutoSize = false;
            infoGroupBox.Controls.Add(descriptionLabel);

            string sportName = sectionName.Split(' ')[0].ToLower();
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string assetsPath = Path.Combine(projectDirectory, "assets");

            int imageHeight = 230;
            int imageY = 250;

            PictureBox sportImage1 = new PictureBox();
            sportImage1.Size = new Size(400, imageHeight);
            sportImage1.Location = new Point(20, imageY);
            sportImage1.SizeMode = PictureBoxSizeMode.StretchImage;
            string imagePath1 = Path.Combine(assetsPath, $"{sportName}1.jpg");
            try
            {
                if (File.Exists(imagePath1))
                {
                    sportImage1.Image = Image.FromFile(imagePath1);
                }
                else
                {
                    MessageBox.Show($"Изображение не найдено: {imagePath1}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    sportImage1.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения 1: {ex.Message}\nПуть: {imagePath1}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sportImage1.BackColor = Color.Gray;
            }
            infoGroupBox.Controls.Add(sportImage1);

            PictureBox sportImage2 = new PictureBox();
            sportImage2.Size = new Size(400, imageHeight);
            sportImage2.Location = new Point(430, imageY);
            sportImage2.SizeMode = PictureBoxSizeMode.StretchImage;
            string imagePath2 = Path.Combine(assetsPath, $"{sportName}2.jpg");
            try
            {
                if (File.Exists(imagePath2))
                {
                    sportImage2.Image = Image.FromFile(imagePath2);
                }
                else
                {
                    MessageBox.Show($"Изображение не найдено: {imagePath2}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    sportImage2.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения 2: {ex.Message}\nПуть: {imagePath2}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sportImage2.BackColor = Color.Gray;
            }
            infoGroupBox.Controls.Add(sportImage2);

            Button enrollButton = new Button();
            enrollButton.Text = "Записаться";
            enrollButton.Font = new Font("Arial", 14);
            enrollButton.Location = new Point(20, 505);
            enrollButton.Size = new Size(200, 65);
            enrollButton.BackColor = Color.White;
            enrollButton.FlatStyle = FlatStyle.Flat;
            enrollButton.FlatAppearance.BorderSize = 2;
            enrollButton.Click += (s, e) => EnrollInSection(sectionName);
            infoGroupBox.Controls.Add(enrollButton);

            infoGroupBox.Visible = true;
        }

        private void ShowAboutUsInfo(Button clickedButton)
        {
            if (lastClickedButton != null && lastClickedButton != clickedButton)
            {
                lastClickedButton.BackColor = Color.White;
            }
            clickedButton.BackColor = Color.LightGreen;
            lastClickedButton = clickedButton;

            infoGroupBox.Controls.Clear();
            infoGroupBox.Text = "Информация о нас";

            Label aboutUsLabel = new Label();
            aboutUsLabel.Text = "Спортивная школа \"Солнышко\" – это место, где дети и подростки раскрывают свой потенциал, развивают силу, ловкость и уверенность в себе. " +
                                " Мы предлагаем разнообразные направления: баскетбол, футбол, йогу и другие активные занятия, которые помогают укрепить здоровье и привить любовь к спорту." +
                                " Наши опытные тренеры создают дружескую атмосферу, поддерживают каждого ученика и помогают достигать новых высот." +
                                " Занятия проходят в комфортных залах и на современных спортивных площадках. Присоединяйтесь к нам, и ваш ребенок станет частью дружной спортивной семьи!";
            aboutUsLabel.Font = new Font("Arial", 14);
            aboutUsLabel.Location = new Point(20, 40);
            aboutUsLabel.Size = new Size(860, 200);
            aboutUsLabel.AutoSize = false;
            infoGroupBox.Controls.Add(aboutUsLabel);

            Label contactUsLabel = new Label();
            contactUsLabel.Text = "Есть вопросы? Свяжитесь с нами и мы постараемся вам помочь!\n" +
                                  "Email: sunsportschool@yandex.ru\n" +
                                  "WhatsApp: +7 (999) 123-45-67\n" +
                                  "Контактное лицо: Забогова Светлана Денисовна\n" +
                                  "Москва, ул. Донская, д. 8 стр. 1\n";
            contactUsLabel.Font = new Font("Arial", 14);
            contactUsLabel.Location = new Point(20, 250);
            contactUsLabel.Size = new Size(860, 300);
            contactUsLabel.AutoSize = false;
            infoGroupBox.Controls.Add(contactUsLabel);

            infoGroupBox.Visible = true;
        }

        private void EnrollInSection(string sectionName)
        {
            int courseCode = -1;
            foreach (DataRow row in coursesTable.Rows)
            {
                if (row["Название"].ToString() == sectionName)
                {
                    courseCode = Convert.ToInt32(row["Код курса"]);
                    break;
                }
            }

            if (courseCode == -1)
            {
                MessageBox.Show("Ошибка: Курс не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (RegistrationForm regForm = new RegistrationForm(sectionName, courseCode, connectionString))
            {
                regForm.ShowDialog();
            }
        }

        private void Button1_MouseClick(object sender, MouseEventArgs e)
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
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Close();
        }
    }

    public partial class RegistrationForm : Form
    {
        private string sectionName;
        private int courseCode;
        private string connectionString;

        public RegistrationForm(string sectionName, int courseCode, string connectionString)
        {
            this.sectionName = sectionName;
            this.courseCode = courseCode;
            this.connectionString = connectionString;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = $"Запись на {sectionName}";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label fioLabel = new Label();
            fioLabel.Text = "ФИО:";
            fioLabel.Font = new Font("Arial", 12);
            fioLabel.Location = new Point(20, 20);
            fioLabel.AutoSize = true;
            this.Controls.Add(fioLabel);

            TextBox fioTextBox = new TextBox();
            fioTextBox.Name = "fioTextBox";
            fioTextBox.Font = new Font("Arial", 12);
            fioTextBox.Location = new Point(20, 50);
            fioTextBox.Size = new Size(350, 30);
            this.Controls.Add(fioTextBox);

            Label phoneLabel = new Label();
            phoneLabel.Text = "Номер телефона:";
            phoneLabel.Font = new Font("Arial", 12);
            phoneLabel.Location = new Point(20, 90);
            phoneLabel.AutoSize = true;
            this.Controls.Add(phoneLabel);

            TextBox phoneTextBox = new TextBox();
            phoneTextBox.Name = "phoneTextBox";
            phoneTextBox.Font = new Font("Arial", 12);
            phoneTextBox.Location = new Point(20, 120);
            phoneTextBox.Size = new Size(350, 30);
            this.Controls.Add(phoneTextBox);

            Label ageLabel = new Label();
            ageLabel.Text = "Возраст:";
            ageLabel.Font = new Font("Arial", 12);
            ageLabel.Location = new Point(20, 160);
            ageLabel.AutoSize = true;
            this.Controls.Add(ageLabel);

            TextBox ageTextBox = new TextBox();
            ageTextBox.Name = "ageTextBox";
            ageTextBox.Font = new Font("Arial", 12);
            ageTextBox.Location = new Point(20, 190);
            ageTextBox.Size = new Size(350, 30);
            this.Controls.Add(ageTextBox);

            Button submitButton = new Button();
            submitButton.Text = "Зарегистрироваться";
            submitButton.Font = new Font("Arial", 12);
            submitButton.Location = new Point(20, 240);
            submitButton.Size = new Size(350, 60);
            submitButton.Click += (s, e) => SubmitRegistration(fioTextBox.Text, phoneTextBox.Text, ageTextBox.Text);
            this.Controls.Add(submitButton);
        }

        private void SubmitRegistration(string fio, string phone, string ageInput)
        {
            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(ageInput))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(ageInput, out int age))
            {
                MessageBox.Show("Возраст должен быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string ageRangeQuery = "SELECT возраст FROM курсы WHERE код = @courseCode";
                    SqlCommand ageCmd = new SqlCommand(ageRangeQuery, conn);
                    ageCmd.Parameters.AddWithValue("@courseCode", courseCode);
                    string ageRange = ageCmd.ExecuteScalar()?.ToString();

                    if (string.IsNullOrEmpty(ageRange))
                    {
                        MessageBox.Show("Не удалось получить возрастной диапазон для курса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var parts = ageRange.Split('-');
                    if (parts.Length != 2 || !int.TryParse(parts[0].Trim(), out int minAge) || !int.TryParse(parts[1].Replace("лет", "").Trim(), out int maxAge))
                    {
                        MessageBox.Show("Неверный формат возрастного диапазона в базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (age < minAge || age > maxAge)
                    {
                        MessageBox.Show($"Ваш возраст ({age}) не соответствует диапазону для курса: {ageRange}.",
                            "Ошибка возраста", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string insertPupilQuery = @"
                        INSERT INTO ученики (ФИО, телефон, возраст, регистрация, курс)
                        OUTPUT INSERTED.код
                        VALUES (@fio, @phone, @age, @registrationDate, @course)";

                    DateTime registrationDate = DateTime.Now;

                    SqlCommand pupilCmd = new SqlCommand(insertPupilQuery, conn);
                    pupilCmd.Parameters.AddWithValue("@fio", fio);
                    pupilCmd.Parameters.AddWithValue("@phone", phone);
                    pupilCmd.Parameters.AddWithValue("@age", age);
                    pupilCmd.Parameters.AddWithValue("@registrationDate", registrationDate);
                    pupilCmd.Parameters.AddWithValue("@course", courseCode);

                    int pupilId = (int)pupilCmd.ExecuteScalar();

                    string insertPaymentQuery = @"
                        INSERT INTO платежи (ученик, курс, посещенно, оплачено)
                        VALUES (@pupilId, @course, 0, 0)";

                    SqlCommand paymentCmd = new SqlCommand(insertPaymentQuery, conn);
                    paymentCmd.Parameters.AddWithValue("@pupilId", pupilId);
                    paymentCmd.Parameters.AddWithValue("@course", courseCode);

                    paymentCmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show($"Вы записаны на занятия \"{sectionName}\"! Скоро вас добавят в специальный чат!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи в базу данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}