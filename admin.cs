using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SportSchool
{
    public partial class admin : Form
    {
        private TabControl tabControl;
        private DataGridView paymentsGrid;
        private DataGridView coachesGrid;
        private DataGridView coursesGrid;
        private DataGridView lessonsGrid;
        private DataGridView studentsGrid;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

        public admin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 800);
            SetupAdminControls();
        }

        private void SetupAdminControls()
        {
            // Заголовок
            Label titleLabel = new Label();
            titleLabel.Text = "Панель администрирования";
            titleLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            // Создание TabControl
            tabControl = new TabControl();
            tabControl.Location = new Point(20, 60);
            tabControl.Size = new Size(1150, 650);
            this.Controls.Add(tabControl);

            // Вкладка 1: Платежи
            TabPage paymentsTab = new TabPage("Платежи");
            paymentsGrid = new DataGridView();
            paymentsGrid.Dock = DockStyle.Top;
            paymentsGrid.Height = 500;
            paymentsTab.Controls.Add(paymentsGrid);

            Button paymentsRefreshButton = new Button();
            paymentsRefreshButton.Text = "Обновить";
            paymentsRefreshButton.Font = new Font("Arial", 12);
            paymentsRefreshButton.Location = new Point(10, 520);
            paymentsRefreshButton.Size = new Size(100, 40);
            paymentsRefreshButton.Click += (s, e) => RefreshGrid("Платежи", paymentsGrid);
            paymentsTab.Controls.Add(paymentsRefreshButton);

            Button paymentsDeleteButton = new Button();
            paymentsDeleteButton.Text = "Удалить";
            paymentsDeleteButton.Font = new Font("Arial", 12);
            paymentsDeleteButton.Location = new Point(120, 520);
            paymentsDeleteButton.Size = new Size(100, 40);
            paymentsDeleteButton.Click += (s, e) => DeleteFromGrid(paymentsGrid);
            paymentsTab.Controls.Add(paymentsDeleteButton);

            Button paymentsAddButton = new Button();
            paymentsAddButton.Text = "Добавить";
            paymentsAddButton.Font = new Font("Arial", 12);
            paymentsAddButton.Location = new Point(230, 520);
            paymentsAddButton.Size = new Size(100, 40);
            paymentsAddButton.Click += (s, e) => OpenAddForm("Платежи", paymentsGrid);
            paymentsTab.Controls.Add(paymentsAddButton);

            tabControl.TabPages.Add(paymentsTab);
            LoadData("Платежи", paymentsGrid);

            // Вкладка 2: Тренеры
            TabPage coachesTab = new TabPage("Тренеры");
            coachesGrid = new DataGridView();
            coachesGrid.Dock = DockStyle.Top;
            coachesGrid.Height = 500;
            coachesTab.Controls.Add(coachesGrid);

            Button coachesRefreshButton = new Button();
            coachesRefreshButton.Text = "Обновить";
            coachesRefreshButton.Font = new Font("Arial", 12);
            coachesRefreshButton.Location = new Point(10, 520);
            coachesRefreshButton.Size = new Size(100, 40);
            coachesRefreshButton.Click += (s, e) => RefreshGrid("Тренеры", coachesGrid);
            coachesTab.Controls.Add(coachesRefreshButton);

            Button coachesDeleteButton = new Button();
            coachesDeleteButton.Text = "Удалить";
            coachesDeleteButton.Font = new Font("Arial", 12);
            coachesDeleteButton.Location = new Point(120, 520);
            coachesDeleteButton.Size = new Size(100, 40);
            coachesDeleteButton.Click += (s, e) => DeleteFromGrid(coachesGrid);
            coachesTab.Controls.Add(coachesDeleteButton);

            Button coachesAddButton = new Button();
            coachesAddButton.Text = "Добавить";
            coachesAddButton.Font = new Font("Arial", 12);
            coachesAddButton.Location = new Point(230, 520);
            coachesAddButton.Size = new Size(100, 40);
            coachesAddButton.Click += (s, e) => OpenAddForm("Тренеры", coachesGrid);
            coachesTab.Controls.Add(coachesAddButton);

            tabControl.TabPages.Add(coachesTab);
            LoadData("Тренеры", coachesGrid);

            // Вкладка 3: Курсы
            TabPage coursesTab = new TabPage("Курсы");
            coursesGrid = new DataGridView();
            coursesGrid.Dock = DockStyle.Top;
            coursesGrid.Height = 500;
            coursesTab.Controls.Add(coursesGrid);

            Button coursesRefreshButton = new Button();
            coursesRefreshButton.Text = "Обновить";
            coursesRefreshButton.Font = new Font("Arial", 12);
            coursesRefreshButton.Location = new Point(10, 520);
            coursesRefreshButton.Size = new Size(100, 40);
            coursesRefreshButton.Click += (s, e) => RefreshGrid("Курсы", coursesGrid);
            coursesTab.Controls.Add(coursesRefreshButton);

            Button coursesDeleteButton = new Button();
            coursesDeleteButton.Text = "Удалить";
            coursesDeleteButton.Font = new Font("Arial", 12);
            coursesDeleteButton.Location = new Point(120, 520);
            coursesDeleteButton.Size = new Size(100, 40);
            coursesDeleteButton.Click += (s, e) => DeleteFromGrid(coursesGrid);
            coursesTab.Controls.Add(coursesDeleteButton);

            Button coursesAddButton = new Button();
            coursesAddButton.Text = "Добавить";
            coursesAddButton.Font = new Font("Arial", 12);
            coursesAddButton.Location = new Point(230, 520);
            coursesAddButton.Size = new Size(100, 40);
            coursesAddButton.Click += (s, e) => OpenAddForm("Курсы", coursesGrid);
            coursesTab.Controls.Add(coursesAddButton);

            tabControl.TabPages.Add(coursesTab);
            LoadData("Курсы", coursesGrid);

            // Вкладка 4: Занятия
            TabPage lessonsTab = new TabPage("Занятия");
            lessonsGrid = new DataGridView();
            lessonsGrid.Dock = DockStyle.Top;
            lessonsGrid.Height = 500;
            lessonsTab.Controls.Add(lessonsGrid);

            Button lessonsRefreshButton = new Button();
            lessonsRefreshButton.Text = "Обновить";
            lessonsRefreshButton.Font = new Font("Arial", 12);
            lessonsRefreshButton.Location = new Point(10, 520);
            lessonsRefreshButton.Size = new Size(100, 40);
            lessonsRefreshButton.Click += (s, e) => RefreshGrid("Занятия", lessonsGrid);
            lessonsTab.Controls.Add(lessonsRefreshButton);

            Button lessonsDeleteButton = new Button();
            lessonsDeleteButton.Text = "Удалить";
            lessonsDeleteButton.Font = new Font("Arial", 12);
            lessonsDeleteButton.Location = new Point(120, 520);
            lessonsDeleteButton.Size = new Size(100, 40);
            lessonsDeleteButton.Click += (s, e) => DeleteFromGrid(lessonsGrid);
            lessonsTab.Controls.Add(lessonsDeleteButton);

            Button lessonsAddButton = new Button();
            lessonsAddButton.Text = "Добавить";
            lessonsAddButton.Font = new Font("Arial", 12);
            lessonsAddButton.Location = new Point(230, 520);
            lessonsAddButton.Size = new Size(100, 40);
            lessonsAddButton.Click += (s, e) => OpenAddForm("Занятия", lessonsGrid);
            lessonsTab.Controls.Add(lessonsAddButton);

            tabControl.TabPages.Add(lessonsTab);
            LoadData("Занятия", lessonsGrid);

            // Вкладка 5: Ученики
            TabPage studentsTab = new TabPage("Ученики");
            studentsGrid = new DataGridView();
            studentsGrid.Dock = DockStyle.Top;
            studentsGrid.Height = 500;
            studentsTab.Controls.Add(studentsGrid);

            Button studentsRefreshButton = new Button();
            studentsRefreshButton.Text = "Обновить";
            studentsRefreshButton.Font = new Font("Arial", 12);
            studentsRefreshButton.Location = new Point(10, 520);
            studentsRefreshButton.Size = new Size(100, 40);
            studentsRefreshButton.Click += (s, e) => RefreshGrid("Ученики", studentsGrid);
            studentsTab.Controls.Add(studentsRefreshButton);

            Button studentsDeleteButton = new Button();
            studentsDeleteButton.Text = "Удалить";
            studentsDeleteButton.Font = new Font("Arial", 12);
            studentsDeleteButton.Location = new Point(120, 520);
            studentsDeleteButton.Size = new Size(100, 40);
            studentsDeleteButton.Click += (s, e) => DeleteFromGrid(studentsGrid);
            studentsTab.Controls.Add(studentsDeleteButton);

            Button studentsAddButton = new Button();
            studentsAddButton.Text = "Добавить";
            studentsAddButton.Font = new Font("Arial", 12);
            studentsAddButton.Location = new Point(230, 520);
            studentsAddButton.Size = new Size(100, 40);
            studentsAddButton.Click += (s, e) => OpenAddForm("Ученики", studentsGrid);
            studentsTab.Controls.Add(studentsAddButton);

            tabControl.TabPages.Add(studentsTab);
            LoadData("Ученики", studentsGrid);

            // Кнопка "Выход" (перенесена в правый верхний угол)
            Button exitButton = new Button();
            exitButton.Text = "Выход из системы";
            exitButton.Font = new Font("Arial", 14);
            exitButton.Location = new Point(980, 15);
            exitButton.Size = new Size(190, 40);
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }

        // Метод для открытия формы добавления
        private void OpenAddForm(string tableName, DataGridView grid)
        {
            Form addForm = new Form();
            addForm.Text = $"Добавить запись в {tableName}";
            addForm.Size = new Size(400, 400);
            addForm.StartPosition = FormStartPosition.CenterParent;

            // Поля ввода в зависимости от таблицы
            switch (tableName)
            {
                case "Платежи":
                    AddTextBox(addForm, "Код ученика", 20, 20);
                    AddTextBox(addForm, "Код курса", 20, 60);
                    AddTextBox(addForm, "Посещено", 20, 100);
                    AddTextBox(addForm, "Оплачено", 20, 140);
                    break;
                case "Тренеры":
                    AddTextBox(addForm, "ФИО", 20, 20);
                    AddTextBox(addForm, "Почта", 20, 60);
                    AddTextBox(addForm, "Телефон", 20, 100);
                    AddTextBox(addForm, "Специализация", 20, 140);
                    AddTextBox(addForm, "Адрес", 20, 180);
                    break;
                case "Курсы":
                    AddTextBox(addForm, "Название", 20, 20);
                    AddTextBox(addForm, "Описание", 20, 60);
                    AddTextBox(addForm, "Возраст", 20, 100);
                    AddTextBox(addForm, "Цена", 20, 140);
                    AddTextBox(addForm, "Длительность", 20, 180);
                    break;
                case "Занятия":
                    AddTextBox(addForm, "Код курса", 20, 20);
                    AddTextBox(addForm, "Код тренера", 20, 60);
                    AddTextBox(addForm, "Дата (ГГГГ-ММ-ДД)", 20, 100);
                    AddTextBox(addForm, "Начало (ЧЧ:ММ)", 20, 140);
                    AddTextBox(addForm, "Конец (ЧЧ:ММ)", 20, 180);
                    break;
                case "Ученики":
                    AddTextBox(addForm, "ФИО", 20, 20);
                    AddTextBox(addForm, "Телефон", 20, 60);
                    AddTextBox(addForm, "Возраст", 20, 100);
                    AddTextBox(addForm, "Дата регистрации (ГГГГ-ММ-ДД)", 20, 140);
                    AddTextBox(addForm, "Код курса", 20, 180);
                    break;
            }

            // Кнопка "Добавить" на форме
            Button confirmButton = new Button();
            confirmButton.Text = "Добавить";
            confirmButton.Font = new Font("Arial", 12);
            confirmButton.Location = new Point(20, 220);
            confirmButton.Size = new Size(100, 40);
            confirmButton.Click += (s, e) => AddRecord(tableName, addForm.Controls, grid);
            addForm.Controls.Add(confirmButton);

            addForm.ShowDialog();
        }

        // Метод для добавления TextBox на форму
        private void AddTextBox(Form form, string labelText, int x, int y)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Location = new Point(x, y);
            label.AutoSize = true;
            form.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Name = labelText;
            textBox.Location = new Point(x + 150, y - 5);
            textBox.Size = new Size(200, 20);
            form.Controls.Add(textBox);
        }

        // Метод для добавления записи в БД
        private void AddRecord(string tableName, Control.ControlCollection controls, DataGridView grid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "";
                    SqlCommand cmd;

                    switch (tableName)
                    {
                        case "Платежи":
                            query = "INSERT INTO платежи (ученик, курс, посещенно, оплачено) VALUES (@ученик, @курс, @посещенно, @оплачено)";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ученик", GetTextBoxValue(controls, "Код ученика"));
                            cmd.Parameters.AddWithValue("@курс", GetTextBoxValue(controls, "Код курса"));
                            cmd.Parameters.AddWithValue("@посещенно", GetTextBoxValue(controls, "Посещено"));
                            cmd.Parameters.AddWithValue("@оплачено", GetTextBoxValue(controls, "Оплачено"));
                            break;
                        case "Тренеры":
                            query = "INSERT INTO тренеры (ФИО, почта, телефон, специалиация, адрес) VALUES (@ФИО, @почта, @телефон, @специалиация, @адрес)";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ФИО", GetTextBoxValue(controls, "ФИО"));
                            cmd.Parameters.AddWithValue("@почта", GetTextBoxValue(controls, "Почта"));
                            cmd.Parameters.AddWithValue("@телефон", GetTextBoxValue(controls, "Телефон"));
                            cmd.Parameters.AddWithValue("@специалиация", GetTextBoxValue(controls, "Специализация"));
                            cmd.Parameters.AddWithValue("@адрес", GetTextBoxValue(controls, "Адрес"));
                            break;
                        case "Курсы":
                            query = "INSERT INTO курсы (название, описание, возраст, цена, длительность) VALUES (@название, @описание, @возраст, @цена, @длительность)";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@название", GetTextBoxValue(controls, "Название"));
                            cmd.Parameters.AddWithValue("@описание", GetTextBoxValue(controls, "Описание"));
                            cmd.Parameters.AddWithValue("@возраст", GetTextBoxValue(controls, "Возраст"));
                            cmd.Parameters.AddWithValue("@цена", GetTextBoxValue(controls, "Цена"));
                            cmd.Parameters.AddWithValue("@длительность", GetTextBoxValue(controls, "Длительность"));
                            break;
                        case "Занятия":
                            query = "INSERT INTO занятия (курс, тренер, дата, начало, конец) VALUES (@курс, @тренер, @дата, @начало, @конец)";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@курс", GetTextBoxValue(controls, "Код курса"));
                            cmd.Parameters.AddWithValue("@тренер", GetTextBoxValue(controls, "Код тренера"));
                            cmd.Parameters.AddWithValue("@дата", GetTextBoxValue(controls, "Дата (ГГГГ-ММ-ДД)"));
                            cmd.Parameters.AddWithValue("@начало", GetTextBoxValue(controls, "Начало (ЧЧ:ММ)"));
                            cmd.Parameters.AddWithValue("@конец", GetTextBoxValue(controls, "Конец (ЧЧ:ММ)"));
                            break;
                        case "Ученики":
                            query = "INSERT INTO ученики (ФИО, телефон, возраст, регистрация, курс) VALUES (@ФИО, @телефон, @возраст, @регистрация, @курс)";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ФИО", GetTextBoxValue(controls, "ФИО"));
                            cmd.Parameters.AddWithValue("@телефон", GetTextBoxValue(controls, "Телефон"));
                            cmd.Parameters.AddWithValue("@возраст", GetTextBoxValue(controls, "Возраст"));
                            cmd.Parameters.AddWithValue("@регистрация", GetTextBoxValue(controls, "Дата регистрации (ГГГГ-ММ-ДД)"));
                            cmd.Parameters.AddWithValue("@курс", GetTextBoxValue(controls, "Код курса"));
                            break;
                        default:
                            return;
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Запись добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid(tableName, grid); // Обновляем таблицу после добавления
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для получения значения из TextBox с проверкой
        private string GetTextBoxValue(Control.ControlCollection controls, string name)
        {
            var textBox = controls.Find(name, true).FirstOrDefault() as TextBox;
            if (textBox == null || string.IsNullOrWhiteSpace(textBox.Text))
            {
                throw new Exception($"Поле '{name}' не заполнено.");
            }
            return textBox.Text;
        }

        // Метод для загрузки данных из БД в DataGridView с русскими названиями полей
        private void LoadData(string tableName, DataGridView grid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "";
                    switch (tableName)
                    {
                        case "Платежи":
                            query = "SELECT код AS [Код платежа], ученик AS [Код ученика], курс AS [Код курса], " +
                                    "посещенно AS [Посещено], оплачено AS [Оплачено] FROM платежи";
                            break;
                        case "Тренеры":
                            query = "SELECT код AS [Код тренера], ФИО AS [ФИО], почта AS [Почта], " +
                                    "телефон AS [Телефон], специалиация AS [Специализация], адрес AS [Адрес] FROM тренеры";
                            break;
                        case "Курсы":
                            query = "SELECT код AS [Код курса], название AS [Название], описание AS [Описание], " +
                                    "возраст AS [Возраст], цена AS [Цена], длительность AS [Длительность] FROM курсы";
                            break;
                        case "Занятия":
                            query = "SELECT код AS [Код Занятия], курс AS [Код курса], тренер AS [Код Тренера], " +
                                    "дата AS [Дата], начало AS [Начало], конец AS [Конец] FROM занятия";
                            break;
                        case "Ученики":
                            query = "SELECT код AS [Код ученика], ФИО AS [ФИО], телефон AS [Телефон], " +
                                    "возраст AS [Возраст], регистрация AS [Дата регистрации], курс AS [Код курса] FROM ученики";
                            break;
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    grid.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из таблицы '{tableName}': {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid(string tableName, DataGridView grid)
        {
            LoadData(tableName, grid); // Повторная загрузка данных из БД
        }

        private void DeleteFromGrid(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            foreach (DataGridViewRow row in grid.SelectedRows)
                            {
                                int kod = Convert.ToInt32(row.Cells[0].Value); // Первый столбец — код
                                string tableName = tabControl.SelectedTab.Text;
                                string query = $"DELETE FROM {tableName} WHERE код = @kod";
                                SqlCommand cmd = new SqlCommand(query, conn);
                                cmd.Parameters.AddWithValue("@kod", kod);
                                cmd.ExecuteNonQuery();
                                grid.Rows.Remove(row);
                            }
                        }
                        MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении записи: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы точно хотите выйти?", "Подтверждение выхода",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Не завершаем приложение полностью, чтобы вернуться в User
        }
    }
}