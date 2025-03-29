using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;
using System.Runtime.InteropServices;


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
        private string connectionString;
        private DataSet dataSet;
        private SqlDataAdapter paymentsAdapter, coachesAdapter, coursesAdapter, lessonsAdapter, studentsAdapter;

        public admin()
        {
            // Устанавливаем |DataDirectory| в корневую папку проекта
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
            this.Size = new Size(1480, 772); // Новый размер формы
            InitializeDataSet();
            SetupAdminControls();
        }

        // Инициализация DataSet и адаптеров
        private void InitializeDataSet()
        {
            try
            {
                dataSet = new DataSet();

                // Создаём соединение
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Адаптер для таблицы "Платежи"
                    paymentsAdapter = new SqlDataAdapter("SELECT код AS [Код платежа], ученик AS [Код ученика], курс AS [Код курса], посещенно AS [Посещено], оплачено AS [Оплачено] FROM платежи", conn);
                    paymentsAdapter.SelectCommand.Connection = new SqlConnection(connectionString); // Явно задаём новое соединение
                    paymentsAdapter.Fill(dataSet, "Платежи");
                    new SqlCommandBuilder(paymentsAdapter); // Теперь команды будут сгенерированы корректно

                    // Адаптер для таблицы "Тренеры"
                    coachesAdapter = new SqlDataAdapter("SELECT код AS [Код тренера], ФИО AS [ФИО], почта AS [Почта], телефон AS [Телефон], специалиация AS [Специализация], адрес AS [Адрес] FROM тренеры", conn);
                    coachesAdapter.SelectCommand.Connection = new SqlConnection(connectionString);
                    coachesAdapter.Fill(dataSet, "Тренеры");
                    new SqlCommandBuilder(coachesAdapter);

                    // Адаптер для таблицы "Курсы"
                    coursesAdapter = new SqlDataAdapter("SELECT код AS [Код курса], название AS [Название], описание AS [Описание], возраст AS [Возраст], цена AS [Цена], длительность AS [Длительность] FROM курсы", conn);
                    coursesAdapter.SelectCommand.Connection = new SqlConnection(connectionString);
                    coursesAdapter.Fill(dataSet, "Курсы");
                    new SqlCommandBuilder(coursesAdapter);

                    // Адаптер для таблицы "Занятия"
                    lessonsAdapter = new SqlDataAdapter("SELECT код AS [Код Занятия], курс AS [Код курса], тренер AS [Код Тренера], дата AS [Дата], начало AS [Начало], конец AS [Конец] FROM занятия", conn);
                    lessonsAdapter.SelectCommand.Connection = new SqlConnection(connectionString);
                    lessonsAdapter.Fill(dataSet, "Занятия");
                    new SqlCommandBuilder(lessonsAdapter);

                    // Адаптер для таблицы "Ученики"
                    studentsAdapter = new SqlDataAdapter("SELECT код AS [Код ученика], ФИО AS [ФИО], телефон AS [Телефон], возраст AS [Возраст], регистрация AS [Дата регистрации], курс AS [Код курса] FROM ученики", conn);
                    studentsAdapter.SelectCommand.Connection = new SqlConnection(connectionString);
                    studentsAdapter.Fill(dataSet, "Ученики");
                    new SqlCommandBuilder(studentsAdapter);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private SqlConnection connection; // Предполагается, что у вас есть подключение к базе данных

        private void SetupAdapters()
        {
            // Подключение к базе данных (замените строку подключения на вашу)
            connection = new SqlConnection("Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True");

            // Настройка адаптера для таблицы Платежи
            string selectQueryPayments = "SELECT Код, Код_ученика, Код_курса, После_уч, Опл_уч FROM Платежи";
            paymentsAdapter = new SqlDataAdapter(selectQueryPayments, connection);

            string insertQueryPayments = "INSERT INTO Платежи (Код_ученика, Код_курса, После_уч, Опл_уч) " +
                                         "VALUES (@Код_ученика, @Код_курса, @После_уч, @Опл_уч); " +
                                         "SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommandPayments = new SqlCommand(insertQueryPayments, connection);
            insertCommandPayments.Parameters.Add("@Код_ученика", SqlDbType.Int, 0, "Код_ученика");
            insertCommandPayments.Parameters.Add("@Код_курса", SqlDbType.Int, 0, "Код_курса");
            insertCommandPayments.Parameters.Add("@После_уч", SqlDbType.DateTime, 0, "После_уч");
            insertCommandPayments.Parameters.Add("@Опл_уч", SqlDbType.Decimal, 0, "Опл_уч");
            paymentsAdapter.InsertCommand = insertCommandPayments;

            paymentsAdapter.Fill(dataSet, "Платежи");
            dataSet.Tables["Платежи"].Columns["Код"].AutoIncrement = true;
            dataSet.Tables["Платежи"].Columns["Код"].AutoIncrementSeed = 1;
            dataSet.Tables["Платежи"].Columns["Код"].AutoIncrementStep = 1;
            dataSet.Tables["Платежи"].Columns["Код"].ReadOnly = true;

            // Настройка адаптера для таблицы Тренеры
            string selectQueryCoaches = "SELECT Код, ФИО, Почта, Телефон, Специализация, Адрес FROM Тренеры";
            coachesAdapter = new SqlDataAdapter(selectQueryCoaches, connection);

            string insertQueryCoaches = "INSERT INTO Тренеры (ФИО, Почта, Телефон, Специализация, Адрес) " +
                                        "VALUES (@ФИО, @Почта, @Телефон, @Специализация, @Адрес); " +
                                        "SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommandCoaches = new SqlCommand(insertQueryCoaches, connection);
            insertCommandCoaches.Parameters.Add("@ФИО", SqlDbType.NVarChar, 100, "ФИО");
            insertCommandCoaches.Parameters.Add("@Почта", SqlDbType.NVarChar, 100, "Почта");
            insertCommandCoaches.Parameters.Add("@Телефон", SqlDbType.NVarChar, 20, "Телефон");
            insertCommandCoaches.Parameters.Add("@Специализация", SqlDbType.NVarChar, 50, "Специализация");
            insertCommandCoaches.Parameters.Add("@Адрес", SqlDbType.NVarChar, 200, "Адрес");
            coachesAdapter.InsertCommand = insertCommandCoaches;

            coachesAdapter.Fill(dataSet, "Тренеры");
            dataSet.Tables["Тренеры"].Columns["Код"].AutoIncrement = true;
            dataSet.Tables["Тренеры"].Columns["Код"].AutoIncrementSeed = 1;
            dataSet.Tables["Тренеры"].Columns["Код"].AutoIncrementStep = 1;
            dataSet.Tables["Тренеры"].Columns["Код"].ReadOnly = true;

            // Настройка адаптера для таблицы Курсы
            string selectQueryCourses = "SELECT Код, Название, Описание, Стоимость FROM Курсы";
            coursesAdapter = new SqlDataAdapter(selectQueryCourses, connection);

            string insertQueryCourses = "INSERT INTO Курсы (Название, Описание, Стоимость) " +
                                       "VALUES (@Название, @Описание, @Стоимость); " +
                                       "SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommandCourses = new SqlCommand(insertQueryCourses, connection);
            insertCommandCourses.Parameters.Add("@Название", SqlDbType.NVarChar, 100, "Название");
            insertCommandCourses.Parameters.Add("@Описание", SqlDbType.NVarChar, 500, "Описание");
            insertCommandCourses.Parameters.Add("@Стоимость", SqlDbType.Decimal, 0, "Стоимость");
            coursesAdapter.InsertCommand = insertCommandCourses;

            coursesAdapter.Fill(dataSet, "Курсы");
            dataSet.Tables["Курсы"].Columns["Код"].AutoIncrement = true;
            dataSet.Tables["Курсы"].Columns["Код"].AutoIncrementSeed = 1;
            dataSet.Tables["Курсы"].Columns["Код"].AutoIncrementStep = 1;
            dataSet.Tables["Курсы"].Columns["Код"].ReadOnly = true;

            // Настройка адаптера для таблицы Занятия
            string selectQueryLessons = "SELECT Код, Код_тренера, Код_курса, Дата_время, Длительность FROM Занятия";
            lessonsAdapter = new SqlDataAdapter(selectQueryLessons, connection);

            string insertQueryLessons = "INSERT INTO Занятия (Код_тренера, Код_курса, Дата_время, Длительность) " +
                                       "VALUES (@Код_тренера, @Код_курса, @Дата_время, @Длительность); " +
                                       "SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommandLessons = new SqlCommand(insertQueryLessons, connection);
            insertCommandLessons.Parameters.Add("@Код_тренера", SqlDbType.Int, 0, "Код_тренера");
            insertCommandLessons.Parameters.Add("@Код_курса", SqlDbType.Int, 0, "Код_курса");
            insertCommandLessons.Parameters.Add("@Дата_время", SqlDbType.DateTime, 0, "Дата_время");
            insertCommandLessons.Parameters.Add("@Длительность", SqlDbType.Int, 0, "Длительность");
            lessonsAdapter.InsertCommand = insertCommandLessons;

            lessonsAdapter.Fill(dataSet, "Занятия");
            dataSet.Tables["Занятия"].Columns["Код"].AutoIncrement = true;
            dataSet.Tables["Занятия"].Columns["Код"].AutoIncrementSeed = 1;
            dataSet.Tables["Занятия"].Columns["Код"].AutoIncrementStep = 1;
            dataSet.Tables["Занятия"].Columns["Код"].ReadOnly = true;

            // Настройка адаптера для таблицы Ученики
            string selectQueryStudents = "SELECT Код, ФИО, Телефон, Код_тренера FROM Ученики";
            studentsAdapter = new SqlDataAdapter(selectQueryStudents, connection);

            string insertQueryStudents = "INSERT INTO Ученики (ФИО, Телефон, Код_тренера) " +
                                        "VALUES (@ФИО, @Телефон, @Код_тренера); " +
                                        "SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommandStudents = new SqlCommand(insertQueryStudents, connection);
            insertCommandStudents.Parameters.Add("@ФИО", SqlDbType.NVarChar, 100, "ФИО");
            insertCommandStudents.Parameters.Add("@Телефон", SqlDbType.NVarChar, 20, "Телефон");
            insertCommandStudents.Parameters.Add("@Код_тренера", SqlDbType.Int, 0, "Код_тренера");
            studentsAdapter.InsertCommand = insertCommandStudents;

            studentsAdapter.Fill(dataSet, "Ученики");
            dataSet.Tables["Ученики"].Columns["Код"].AutoIncrement = true;
            dataSet.Tables["Ученики"].Columns["Код"].AutoIncrementSeed = 1;
            dataSet.Tables["Ученики"].Columns["Код"].AutoIncrementStep = 1;
            dataSet.Tables["Ученики"].Columns["Код"].ReadOnly = true;
        }

        private void SetupAdminControls()
        {
            // Центрируем элементы: ширина формы 1480, отступы по краям
            int formWidth = 1480;
            int tabControlWidth = 1400;
            int leftMargin = (formWidth - tabControlWidth) / 2;

            // Заголовок
            Label titleLabel = new Label();
            titleLabel.Text = "Панель администрирования";
            titleLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            titleLabel.Location = new Point(leftMargin, 20);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            // Создание TabControl
            tabControl = new TabControl();
            tabControl.Location = new Point(leftMargin, 60);
            tabControl.Size = new Size(tabControlWidth, 650);
            tabControl.Font = new Font("Arial", 14, FontStyle.Bold);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(150, 40);
            this.Controls.Add(tabControl);

            // Вкладка 1: Платежи
            TabPage paymentsTab = new TabPage("Платежи");
            paymentsGrid = new DataGridView();
            paymentsGrid.Dock = DockStyle.Top;
            paymentsGrid.Height = 450;
            paymentsGrid.DataSource = dataSet.Tables["Платежи"];
            paymentsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            paymentsGrid.ColumnHeadersHeight = 50;
            paymentsTab.Controls.Add(paymentsGrid);

            Button paymentsRefreshButton = new Button();
            paymentsRefreshButton.Text = "Обновить";
            paymentsRefreshButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsRefreshButton.Location = new Point(10, 470);
            paymentsRefreshButton.Size = new Size(120, 50);
            paymentsRefreshButton.Click += (s, e) => RefreshGrid("Платежи", paymentsGrid, paymentsAdapter);
            paymentsTab.Controls.Add(paymentsRefreshButton);

            Button paymentsDeleteButton = new Button();
            paymentsDeleteButton.Text = "Удалить";
            paymentsDeleteButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsDeleteButton.Location = new Point(140, 470);
            paymentsDeleteButton.Size = new Size(120, 50);
            paymentsDeleteButton.Click += (s, e) => DeleteFromGrid(paymentsGrid, paymentsAdapter);
            paymentsTab.Controls.Add(paymentsDeleteButton);

            Button paymentsAddButton = new Button();
            paymentsAddButton.Text = "Добавить";
            paymentsAddButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsAddButton.Location = new Point(270, 470);
            paymentsAddButton.Size = new Size(120, 50);
            paymentsAddButton.Click += (s, e) => OpenAddForm("Платежи", paymentsGrid, paymentsAdapter);
            paymentsTab.Controls.Add(paymentsAddButton);

            Button paymentsSaveButton = new Button();
            paymentsSaveButton.Text = "Сохранить";
            paymentsSaveButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsSaveButton.Location = new Point(400, 470); // Добавляем кнопку "Сохранить"
            paymentsSaveButton.Size = new Size(120, 50);
            paymentsSaveButton.Click += (s, e) => SaveChanges(paymentsGrid, paymentsAdapter);
            paymentsTab.Controls.Add(paymentsSaveButton);

            Button paymentsExportButton = new Button();
            paymentsExportButton.Text = "Отчет";
            paymentsExportButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsExportButton.Location = new Point(tabControlWidth - 130, 470);
            paymentsExportButton.Size = new Size(120, 50);
            paymentsExportButton.BackColor = Color.LightGreen;
            paymentsExportButton.Click += exportbutton_Click;
            paymentsTab.Controls.Add(paymentsExportButton);

            Button paymentsSortButton = new Button();
            paymentsSortButton.Text = "Сорт.";
            paymentsSortButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsSortButton.Location = new Point(10, 530);
            paymentsSortButton.Size = new Size(120, 50);
            paymentsSortButton.Click += (s, e) => SortGrid(paymentsGrid, "Платежи");
            paymentsTab.Controls.Add(paymentsSortButton);

            Button paymentsFilterButton = new Button();
            paymentsFilterButton.Text = "Фильтр";
            paymentsFilterButton.Font = new Font("Arial", 12, FontStyle.Bold);
            paymentsFilterButton.Location = new Point(140, 530);
            paymentsFilterButton.Size = new Size(120, 50);
            paymentsFilterButton.Click += (s, e) => FilterGrid(paymentsGrid, "Платежи");
            paymentsTab.Controls.Add(paymentsFilterButton);

            tabControl.TabPages.Add(paymentsTab);

            // Вкладка 2: Тренеры
            TabPage coachesTab = new TabPage("Тренеры");
            coachesGrid = new DataGridView();
            coachesGrid.Dock = DockStyle.Top;
            coachesGrid.Height = 450;
            coachesGrid.DataSource = dataSet.Tables["Тренеры"];
            coachesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            coachesGrid.ColumnHeadersHeight = 50;
            coachesGrid.DataBindingComplete += (s, e) =>
            {
                coachesGrid.Columns["ФИО"].Width = 300;
                coachesGrid.Columns["Почта"].Width = 200;
                coachesGrid.Columns["Специализация"].Width = 200;
                coachesGrid.Columns["Адрес"].Width = 300;
            };
            coachesTab.Controls.Add(coachesGrid);

            Button coachesRefreshButton = new Button();
            coachesRefreshButton.Text = "Обновить";
            coachesRefreshButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesRefreshButton.Location = new Point(10, 470);
            coachesRefreshButton.Size = new Size(120, 50);
            coachesRefreshButton.Click += (s, e) => RefreshGrid("Тренеры", coachesGrid, coachesAdapter);
            coachesTab.Controls.Add(coachesRefreshButton);

            Button coachesDeleteButton = new Button();
            coachesDeleteButton.Text = "Удалить";
            coachesDeleteButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesDeleteButton.Location = new Point(140, 470);
            coachesDeleteButton.Size = new Size(120, 50);
            coachesDeleteButton.Click += (s, e) => DeleteFromGrid(coachesGrid, coachesAdapter);
            coachesTab.Controls.Add(coachesDeleteButton);

            Button coachesAddButton = new Button();
            coachesAddButton.Text = "Добавить";
            coachesAddButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesAddButton.Location = new Point(270, 470);
            coachesAddButton.Size = new Size(120, 50);
            coachesAddButton.Click += (s, e) => OpenAddForm("Тренеры", coachesGrid, coachesAdapter);
            coachesTab.Controls.Add(coachesAddButton);

            Button coachesSaveButton = new Button();
            coachesSaveButton.Text = "Сохранить";
            coachesSaveButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesSaveButton.Location = new Point(400, 470);
            coachesSaveButton.Size = new Size(120, 50);
            coachesSaveButton.Click += (s, e) => SaveChanges(coachesGrid, coachesAdapter);
            coachesTab.Controls.Add(coachesSaveButton);

            Button coachesExportButton = new Button();
            coachesExportButton.Text = "Отчет";
            coachesExportButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesExportButton.Location = new Point(tabControlWidth - 130, 470);
            coachesExportButton.Size = new Size(120, 50);
            coachesExportButton.BackColor = Color.LightGreen;
            coachesExportButton.Click += exportbutton_Click;
            coachesTab.Controls.Add(coachesExportButton);

            Button coachesSortButton = new Button();
            coachesSortButton.Text = "Сорт.";
            coachesSortButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesSortButton.Location = new Point(10, 530);
            coachesSortButton.Size = new Size(120, 50);
            coachesSortButton.Click += (s, e) => SortGrid(coachesGrid, "Тренеры");
            coachesTab.Controls.Add(coachesSortButton);

            Button coachesFilterButton = new Button();
            coachesFilterButton.Text = "Фильтр";
            coachesFilterButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coachesFilterButton.Location = new Point(140, 530);
            coachesFilterButton.Size = new Size(120, 50);
            coachesFilterButton.Click += (s, e) => FilterGrid(coachesGrid, "Тренеры");
            coachesTab.Controls.Add(coachesFilterButton);

            tabControl.TabPages.Add(coachesTab);

            // Вкладка 3: Курсы
            TabPage coursesTab = new TabPage("Курсы");
            coursesGrid = new DataGridView();
            coursesGrid.Dock = DockStyle.Top;
            coursesGrid.Height = 450;
            coursesGrid.DataSource = dataSet.Tables["Курсы"];
            coursesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            coursesGrid.ColumnHeadersHeight = 50;
            coursesGrid.DataBindingComplete += (s, e) =>
            {
                coursesGrid.Columns["Название"].Width = 200;
                coursesGrid.Columns["Описание"].Width = 400;
            };
            coursesTab.Controls.Add(coursesGrid);

            Button coursesRefreshButton = new Button();
            coursesRefreshButton.Text = "Обновить";
            coursesRefreshButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesRefreshButton.Location = new Point(10, 470);
            coursesRefreshButton.Size = new Size(120, 50);
            coursesRefreshButton.Click += (s, e) => RefreshGrid("Курсы", coursesGrid, coursesAdapter);
            coursesTab.Controls.Add(coursesRefreshButton);

            Button coursesDeleteButton = new Button();
            coursesDeleteButton.Text = "Удалить";
            coursesDeleteButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesDeleteButton.Location = new Point(140, 470);
            coursesDeleteButton.Size = new Size(120, 50);
            coursesDeleteButton.Click += (s, e) => DeleteFromGrid(coursesGrid, coursesAdapter);
            coursesTab.Controls.Add(coursesDeleteButton);

            Button coursesAddButton = new Button();
            coursesAddButton.Text = "Добавить";
            coursesAddButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesAddButton.Location = new Point(270, 470);
            coursesAddButton.Size = new Size(120, 50);
            coursesAddButton.Click += (s, e) => OpenAddForm("Курсы", coursesGrid, coursesAdapter);
            coursesTab.Controls.Add(coursesAddButton);

            Button coursesSaveButton = new Button();
            coursesSaveButton.Text = "Сохранить";
            coursesSaveButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesSaveButton.Location = new Point(400, 470);
            coursesSaveButton.Size = new Size(120, 50);
            coursesSaveButton.Click += (s, e) => SaveChanges(coursesGrid, coursesAdapter);
            coursesTab.Controls.Add(coursesSaveButton);

            Button coursesExportButton = new Button();
            coursesExportButton.Text = "Отчет";
            coursesExportButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesExportButton.Location = new Point(tabControlWidth - 130, 470);
            coursesExportButton.Size = new Size(120, 50);
            coursesExportButton.BackColor = Color.LightGreen;
            coursesExportButton.Click += exportbutton_Click;
            coursesTab.Controls.Add(coursesExportButton);

            Button coursesSortButton = new Button();
            coursesSortButton.Text = "Сорт.";
            coursesSortButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesSortButton.Location = new Point(10, 530);
            coursesSortButton.Size = new Size(120, 50);
            coursesSortButton.Click += (s, e) => SortGrid(coursesGrid, "Курсы");
            coursesTab.Controls.Add(coursesSortButton);

            Button coursesFilterButton = new Button();
            coursesFilterButton.Text = "Фильтр";
            coursesFilterButton.Font = new Font("Arial", 12, FontStyle.Bold);
            coursesFilterButton.Location = new Point(140, 530);
            coursesFilterButton.Size = new Size(120, 50);
            coursesFilterButton.Click += (s, e) => FilterGrid(coursesGrid, "Курсы");
            coursesTab.Controls.Add(coursesFilterButton);

            tabControl.TabPages.Add(coursesTab);

            // Вкладка 4: Занятия
            TabPage lessonsTab = new TabPage("Занятия");
            lessonsGrid = new DataGridView();
            lessonsGrid.Dock = DockStyle.Top;
            lessonsGrid.Height = 450;
            lessonsGrid.DataSource = dataSet.Tables["Занятия"];
            lessonsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lessonsGrid.ColumnHeadersHeight = 50;
            lessonsTab.Controls.Add(lessonsGrid);

            Button lessonsRefreshButton = new Button();
            lessonsRefreshButton.Text = "Обновить";
            lessonsRefreshButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsRefreshButton.Location = new Point(10, 470);
            lessonsRefreshButton.Size = new Size(120, 50);
            lessonsRefreshButton.Click += (s, e) => RefreshGrid("Занятия", lessonsGrid, lessonsAdapter);
            lessonsTab.Controls.Add(lessonsRefreshButton);

            Button lessonsDeleteButton = new Button();
            lessonsDeleteButton.Text = "Удалить";
            lessonsDeleteButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsDeleteButton.Location = new Point(140, 470);
            lessonsDeleteButton.Size = new Size(120, 50);
            lessonsDeleteButton.Click += (s, e) => DeleteFromGrid(lessonsGrid, lessonsAdapter);
            lessonsTab.Controls.Add(lessonsDeleteButton);

            Button lessonsAddButton = new Button();
            lessonsAddButton.Text = "Добавить";
            lessonsAddButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsAddButton.Location = new Point(270, 470);
            lessonsAddButton.Size = new Size(120, 50);
            lessonsAddButton.Click += (s, e) => OpenAddForm("Занятия", lessonsGrid, lessonsAdapter);
            lessonsTab.Controls.Add(lessonsAddButton);

            Button lessonsSaveButton = new Button();
            lessonsSaveButton.Text = "Сохранить";
            lessonsSaveButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsSaveButton.Location = new Point(400, 470);
            lessonsSaveButton.Size = new Size(120, 50);
            lessonsSaveButton.Click += (s, e) => SaveChanges(lessonsGrid, lessonsAdapter);
            lessonsTab.Controls.Add(lessonsSaveButton);

            Button lessonsExportButton = new Button();
            lessonsExportButton.Text = "Отчет";
            lessonsExportButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsExportButton.Location = new Point(tabControlWidth - 130, 470);
            lessonsExportButton.Size = new Size(120, 50);
            lessonsExportButton.BackColor = Color.LightGreen;
            lessonsExportButton.Click += exportbutton_Click;
            lessonsTab.Controls.Add(lessonsExportButton);

            Button lessonsSortButton = new Button();
            lessonsSortButton.Text = "Сорт.";
            lessonsSortButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsSortButton.Location = new Point(10, 530);
            lessonsSortButton.Size = new Size(120, 50);
            lessonsSortButton.Click += (s, e) => SortGrid(lessonsGrid, "Занятия");
            lessonsTab.Controls.Add(lessonsSortButton);

            Button lessonsFilterButton = new Button();
            lessonsFilterButton.Text = "Фильтр";
            lessonsFilterButton.Font = new Font("Arial", 12, FontStyle.Bold);
            lessonsFilterButton.Location = new Point(140, 530);
            lessonsFilterButton.Size = new Size(120, 50);
            lessonsFilterButton.Click += (s, e) => FilterGrid(lessonsGrid, "Занятия");
            lessonsTab.Controls.Add(lessonsFilterButton);

            tabControl.TabPages.Add(lessonsTab);

            // Вкладка 5: Ученики
            TabPage studentsTab = new TabPage("Ученики");
            studentsGrid = new DataGridView();
            studentsGrid.Dock = DockStyle.Top;
            studentsGrid.Height = 450;
            studentsGrid.DataSource = dataSet.Tables["Ученики"];
            studentsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            studentsGrid.ColumnHeadersHeight = 50;
            studentsGrid.DataBindingComplete += (s, e) =>
            {
                studentsGrid.Columns["ФИО"].Width = 300;
                studentsGrid.Columns["Телефон"].Width = 200;
            };
            studentsTab.Controls.Add(studentsGrid);

            Button studentsRefreshButton = new Button();
            studentsRefreshButton.Text = "Обновить";
            studentsRefreshButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsRefreshButton.Location = new Point(10, 470);
            studentsRefreshButton.Size = new Size(120, 50);
            studentsRefreshButton.Click += (s, e) => RefreshGrid("Ученики", studentsGrid, studentsAdapter);
            studentsTab.Controls.Add(studentsRefreshButton);

            Button studentsDeleteButton = new Button();
            studentsDeleteButton.Text = "Удалить";
            studentsDeleteButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsDeleteButton.Location = new Point(140, 470);
            studentsDeleteButton.Size = new Size(120, 50);
            studentsDeleteButton.Click += (s, e) => DeleteFromGrid(studentsGrid, studentsAdapter);
            studentsTab.Controls.Add(studentsDeleteButton);

            Button studentsAddButton = new Button();
            studentsAddButton.Text = "Добавить";
            studentsAddButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsAddButton.Location = new Point(270, 470);
            studentsAddButton.Size = new Size(120, 50);
            studentsAddButton.Click += (s, e) => OpenAddForm("Ученики", studentsGrid, studentsAdapter);
            studentsTab.Controls.Add(studentsAddButton);

            Button studentsSaveButton = new Button();
            studentsSaveButton.Text = "Сохранить";
            studentsSaveButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsSaveButton.Location = new Point(400, 470);
            studentsSaveButton.Size = new Size(120, 50);
            studentsSaveButton.Click += (s, e) => SaveChanges(studentsGrid, studentsAdapter);
            studentsTab.Controls.Add(studentsSaveButton);

            Button studentsExportButton = new Button();
            studentsExportButton.Text = "Отчет";
            studentsExportButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsExportButton.Location = new Point(tabControlWidth - 130, 470);
            studentsExportButton.Size = new Size(120, 50);
            studentsExportButton.BackColor = Color.LightGreen;
            studentsExportButton.Click += exportbutton_Click;
            studentsTab.Controls.Add(studentsExportButton);

            Button studentsSortButton = new Button();
            studentsSortButton.Text = "Сорт.";
            studentsSortButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsSortButton.Location = new Point(10, 530);
            studentsSortButton.Size = new Size(120, 50);
            studentsSortButton.Click += (s, e) => SortGrid(studentsGrid, "Ученики");
            studentsTab.Controls.Add(studentsSortButton);

            Button studentsFilterButton = new Button();
            studentsFilterButton.Text = "Фильтр";
            studentsFilterButton.Font = new Font("Arial", 12, FontStyle.Bold);
            studentsFilterButton.Location = new Point(140, 530);
            studentsFilterButton.Size = new Size(120, 50);
            studentsFilterButton.Click += (s, e) => FilterGrid(studentsGrid, "Ученики");
            studentsTab.Controls.Add(studentsFilterButton);

            tabControl.TabPages.Add(studentsTab);

            // Устанавливаем "Платежи" активной вкладкой по умолчанию
            tabControl.SelectedTab = paymentsTab;

            // Кнопка "Выход из системы"
            Button exitButton = new Button();
            exitButton.Text = "Выход из системы";
            exitButton.Font = new Font("Arial", 12, FontStyle.Bold);
            exitButton.Location = new Point(formWidth - 230, 15);
            exitButton.Size = new Size(190, 40);
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }
        // Метод для сортировки данных в DataGridView
        private void SortGrid(DataGridView grid, string tableName)
        {
            try
            {
                Form sortForm = new Form();
                sortForm.Text = $"Сортировка таблицы {tableName}";
                sortForm.Size = new Size(300, 200);
                sortForm.StartPosition = FormStartPosition.CenterParent;

                Label columnLabel = new Label();
                columnLabel.Text = "Выберите столбец:";
                columnLabel.Location = new Point(10, 20);
                columnLabel.AutoSize = true;
                sortForm.Controls.Add(columnLabel);

                ComboBox columnComboBox = new ComboBox();
                columnComboBox.Location = new Point(10, 40);
                columnComboBox.Size = new Size(260, 20);
                foreach (DataColumn column in dataSet.Tables[tableName].Columns)
                {
                    columnComboBox.Items.Add(column.ColumnName);
                }
                columnComboBox.SelectedIndex = 0; // По умолчанию первый столбец
                sortForm.Controls.Add(columnComboBox);

                Label directionLabel = new Label();
                directionLabel.Text = "Направление сортировки:";
                directionLabel.Location = new Point(10, 70);
                directionLabel.AutoSize = true;
                sortForm.Controls.Add(directionLabel);

                ComboBox directionComboBox = new ComboBox();
                directionComboBox.Location = new Point(10, 90);
                directionComboBox.Size = new Size(260, 20);
                directionComboBox.Items.AddRange(new string[] { "По возрастанию (ASC)", "По убыванию (DESC)" });
                directionComboBox.SelectedIndex = 0; // По умолчанию по возрастанию
                sortForm.Controls.Add(directionComboBox);

                Button confirmButton = new Button();
                confirmButton.Text = "Применить";
                confirmButton.Font = new Font("Arial", 12);
                confirmButton.Location = new Point(10, 120);
                confirmButton.Size = new Size(100, 40);
                confirmButton.Click += (s, e) =>
                {
                    string selectedColumn = columnComboBox.SelectedItem.ToString();
                    string direction = directionComboBox.SelectedIndex == 0 ? "ASC" : "DESC";

                    DataView dataView = dataSet.Tables[tableName].DefaultView;
                    dataView.Sort = $"{selectedColumn} {direction}";
                    grid.DataSource = dataView;

                    sortForm.Close();
                };
                sortForm.Controls.Add(confirmButton);

                sortForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для фильтрации данных в DataGridView
        private void FilterGrid(DataGridView grid, string tableName)
        {
            try
            {
                Form filterForm = new Form();
                filterForm.Text = $"Фильтрация таблицы {tableName}";
                filterForm.Size = new Size(300, 200);
                filterForm.StartPosition = FormStartPosition.CenterParent;

                Label columnLabel = new Label();
                columnLabel.Text = "Выберите столбец:";
                columnLabel.Location = new Point(10, 20);
                columnLabel.AutoSize = true;
                filterForm.Controls.Add(columnLabel);

                ComboBox columnComboBox = new ComboBox();
                columnComboBox.Location = new Point(10, 40);
                columnComboBox.Size = new Size(260, 20);
                foreach (DataColumn column in dataSet.Tables[tableName].Columns)
                {
                    columnComboBox.Items.Add(column.ColumnName);
                }
                columnComboBox.SelectedIndex = 0; // По умолчанию первый столбец
                filterForm.Controls.Add(columnComboBox);

                Label valueLabel = new Label();
                valueLabel.Text = "Введите значение:";
                valueLabel.Location = new Point(10, 70);
                valueLabel.AutoSize = true;
                filterForm.Controls.Add(valueLabel);

                TextBox valueTextBox = new TextBox();
                valueTextBox.Location = new Point(10, 90);
                valueTextBox.Size = new Size(260, 20);
                filterForm.Controls.Add(valueTextBox);

                Button confirmButton = new Button();
                confirmButton.Text = "Применить";
                confirmButton.Font = new Font("Arial", 12);
                confirmButton.Location = new Point(10, 120);
                confirmButton.Size = new Size(100, 40);
                confirmButton.Click += (s, e) =>
                {
                    string selectedColumn = columnComboBox.SelectedItem.ToString();
                    string filterValue = valueTextBox.Text.Trim();

                    DataView dataView = dataSet.Tables[tableName].DefaultView;
                    if (string.IsNullOrEmpty(filterValue))
                    {
                        dataView.RowFilter = ""; // Сброс фильтра
                    }
                    else
                    {
                        // Проверяем тип данных столбца
                        if (dataSet.Tables[tableName].Columns[selectedColumn].DataType == typeof(string))
                        {
                            dataView.RowFilter = $"{selectedColumn} LIKE '%{filterValue}%'";
                        }
                        else
                        {
                            dataView.RowFilter = $"{selectedColumn} = '{filterValue}'";
                        }
                    }
                    grid.DataSource = dataView;

                    filterForm.Close();
                };
                filterForm.Controls.Add(confirmButton);

                filterForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveChanges(DataGridView grid, SqlDataAdapter adapter)
        {
            try
            {
                // Завершаем редактирование в DataGridView, чтобы все изменения применились к DataTable
                grid.EndEdit();

                // Получаем DataTable, связанную с DataGridView
                DataTable dataTable = (DataTable)grid.DataSource;

                // Проверяем, есть ли изменения в DataTable
                if (dataTable.GetChanges() != null)
                {
                    // Обновляем базу данных через адаптер
                    adapter.Update(dataTable);

                    // Принимаем изменения в DataTable, чтобы они не считались "изменёнными" повторно
                    dataTable.AcceptChanges();

                    MessageBox.Show("Изменения успешно сохранены в базе данных!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для открытия формы добавления
        private void OpenAddForm(string tableName, DataGridView grid, SqlDataAdapter adapter)
        {
            try
            {
                Form addForm = new Form();
                addForm.Text = $"Добавление записи в таблицу {tableName}";
                addForm.Size = new Size(400, 400);
                addForm.StartPosition = FormStartPosition.CenterParent;

                // Создаём элементы управления для каждого столбца таблицы, кроме IDENTITY
                DataTable table = dataSet.Tables[tableName];
                List<TextBox> textBoxes = new List<TextBox>();
                int yPosition = 20;

                foreach (DataColumn column in table.Columns)
                {
                    // Пропускаем поле IDENTITY (например, "Код")
                    if (column.AutoIncrement) // Проверяем, является ли столбец IDENTITY
                        continue;

                    Label label = new Label();
                    label.Text = column.ColumnName + ":";
                    label.Location = new Point(10, yPosition);
                    label.AutoSize = true;
                    addForm.Controls.Add(label);

                    TextBox textBox = new TextBox();
                    textBox.Location = new Point(150, yPosition);
                    textBox.Size = new Size(200, 20);
                    textBox.Name = column.ColumnName; // Для удобства обращения
                    addForm.Controls.Add(textBox);
                    textBoxes.Add(textBox);

                    yPosition += 40;
                }

                Button confirmButton = new Button();
                confirmButton.Text = "Добавить";
                confirmButton.Font = new Font("Arial", 12);
                confirmButton.Location = new Point(10, yPosition);
                confirmButton.Size = new Size(100, 40);
                confirmButton.Click += (s, e) =>
                {
                    try
                    {
                        // Создаём новую строку в DataTable
                        DataRow newRow = table.NewRow();

                        // Заполняем строку данными из формы
                        foreach (TextBox textBox in textBoxes)
                        {
                            newRow[textBox.Name] = textBox.Text;
                        }

                        // Добавляем строку в DataTable
                        table.Rows.Add(newRow);

                        // Обновляем базу данных через адаптер
                        adapter.Update(table);

                        // Обновляем DataGridView
                        grid.DataSource = table;

                        MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        addForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                addForm.Controls.Add(confirmButton);

                addForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы добавления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        // Метод для добавления записи в DataSet
        private void AddRecord(string tableName, Control.ControlCollection controls, DataGridView grid, SqlDataAdapter adapter)
        {
            try
            {
                DataRow newRow = dataSet.Tables[tableName].NewRow();

                switch (tableName)
                {
                    case "Платежи":
                        newRow["Код ученика"] = GetTextBoxValue(controls, "Код ученика");
                        newRow["Код курса"] = GetTextBoxValue(controls, "Код курса");
                        newRow["Посещено"] = GetTextBoxValue(controls, "Посещено");
                        newRow["Оплачено"] = GetTextBoxValue(controls, "Оплачено");
                        break;
                    case "Тренеры":
                        newRow["ФИО"] = GetTextBoxValue(controls, "ФИО");
                        newRow["Почта"] = GetTextBoxValue(controls, "Почта");
                        newRow["Телефон"] = GetTextBoxValue(controls, "Телефон");
                        newRow["Специализация"] = GetTextBoxValue(controls, "Специализация");
                        newRow["Адрес"] = GetTextBoxValue(controls, "Адрес");
                        break;
                    case "Курсы":
                        newRow["Название"] = GetTextBoxValue(controls, "Название");
                        newRow["Описание"] = GetTextBoxValue(controls, "Описание");
                        newRow["Возраст"] = GetTextBoxValue(controls, "Возраст");
                        newRow["Цена"] = GetTextBoxValue(controls, "Цена");
                        newRow["Длительность"] = GetTextBoxValue(controls, "Длительность");
                        break;
                    case "Занятия":
                        newRow["Код курса"] = GetTextBoxValue(controls, "Код курса");
                        newRow["Код Тренера"] = GetTextBoxValue(controls, "Код тренера");
                        newRow["Дата"] = GetTextBoxValue(controls, "Дата");
                        newRow["Начало"] = GetTextBoxValue(controls, "Начало (ЧЧ:ММ)");
                        newRow["Конец"] = GetTextBoxValue(controls, "Конец (ЧЧ:ММ)");
                        break;
                    case "Ученики":
                        newRow["ФИО"] = GetTextBoxValue(controls, "ФИО");
                        newRow["Телефон"] = GetTextBoxValue(controls, "Телефон");
                        newRow["Возраст"] = GetTextBoxValue(controls, "Возраст");
                        newRow["Дата регистрации"] = GetTextBoxValue(controls, "Дата регистрации");
                        newRow["Код курса"] = GetTextBoxValue(controls, "Код курса");
                        break;
                }

                dataSet.Tables[tableName].Rows.Add(newRow);
                adapter.Update(dataSet, tableName); // Сохраняем изменения в базе данных
                MessageBox.Show("Запись добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grid.DataSource = dataSet.Tables[tableName]; // Обновляем DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Метод для обновления данных в DataGridView
        private void RefreshGrid(string tableName, DataGridView grid, SqlDataAdapter adapter)
        {
            try
            {
                dataSet.Tables[tableName].Clear();
                adapter.Fill(dataSet, tableName);
                grid.DataSource = dataSet.Tables[tableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для удаления записи из DataGridView и базы данных
        private void DeleteFromGrid(DataGridView grid, SqlDataAdapter adapter)
        {
            if (grid.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        foreach (DataGridViewRow row in grid.SelectedRows)
                        {
                            grid.Rows.Remove(row); // Удаляем строку из DataGridView
                        }
                        adapter.Update(dataSet, tabControl.SelectedTab.Text); // Сохраняем изменения в базе данных
                        MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении записи: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportbutton_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workBook = null;

            try
            {
                // Создание нового Excel-приложения
                excelApp = new Excel.Application();
                excelApp.Visible = true;
                workBook = excelApp.Workbooks.Add();

                // Создаем лист "Дополнительно" как первый лист
                Excel.Worksheet workSheetAdditional = (Excel.Worksheet)workBook.Sheets[1];
                workSheetAdditional.Name = "Дополнительно";

                // Перебираем все таблицы в dataSet и добавляем их в Excel
                int sheetIndex = 2;
                foreach (DataTable table in dataSet.Tables)
                {
                    // Создаем новый лист с именем таблицы
                    Excel.Worksheet workSheet = (Excel.Worksheet)workBook.Sheets.Add();
                    workSheet.Name = table.TableName.Length > 31 ? table.TableName.Substring(0, 31) : table.TableName; // Ограничение длины имени листа в Excel

                    // Заголовки колонок
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        workSheet.Cells[1, i + 1] = table.Columns[i].ColumnName;
                    }

                    // Данные таблицы
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = table.Rows[i][j]?.ToString(); // Приведение к строке для избежания проблем с типами
                        }
                    }

                    // Автоматически растягиваем колонки и строки
                    workSheet.Columns.AutoFit();
                    workSheet.Rows.AutoFit();

                    sheetIndex++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
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
            // Сохраняем изменения перед закрытием формы
            try
            {
                paymentsAdapter.Update(dataSet, "Платежи");
                coachesAdapter.Update(dataSet, "Тренеры");
                coursesAdapter.Update(dataSet, "Курсы");
                lessonsAdapter.Update(dataSet, "Занятия");
                studentsAdapter.Update(dataSet, "Ученики");
                MessageBox.Show("Данные успешно сохранены перед закрытием.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}