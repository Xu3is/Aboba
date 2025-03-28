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
            paymentsGrid.Location = new Point(10, 10);
            paymentsGrid.Size = new Size(1130, 500);
            paymentsGrid.Columns.Add("PaymentID", "ID Платежа");
            paymentsGrid.Columns.Add("StudentID", "ID Ученика");
            paymentsGrid.Columns.Add("Amount", "Сумма");
            paymentsGrid.Columns.Add("PaymentDate", "Дата платежа");
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

            tabControl.TabPages.Add(paymentsTab);

            // Вкладка 2: Тренеры
            TabPage coachesTab = new TabPage("Тренеры");
            coachesGrid = new DataGridView();
            coachesGrid.Location = new Point(10, 10);
            coachesGrid.Size = new Size(1130, 500);
            coachesGrid.Columns.Add("CoachID", "ID Тренера");
            coachesGrid.Columns.Add("CoachName", "Имя тренера");
            coachesGrid.Columns.Add("Specialization", "Специализация");
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

            tabControl.TabPages.Add(coachesTab);

            // Вкладка 3: Курсы
            TabPage coursesTab = new TabPage("Курсы");
            coursesGrid = new DataGridView();
            coursesGrid.Location = new Point(10, 10);
            coursesGrid.Size = new Size(1130, 500);
            coursesGrid.Columns.Add("CourseID", "ID Курса");
            coursesGrid.Columns.Add("CourseName", "Название курса");
            coursesGrid.Columns.Add("CoachID", "ID Тренера");
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

            tabControl.TabPages.Add(coursesTab);

            // Вкладка 4: Занятия
            TabPage lessonsTab = new TabPage("Занятия");
            lessonsGrid = new DataGridView();
            lessonsGrid.Location = new Point(10, 10);
            lessonsGrid.Size = new Size(1130, 500);
            lessonsGrid.Columns.Add("LessonID", "ID Занятия");
            lessonsGrid.Columns.Add("CourseID", "ID Курса");
            lessonsGrid.Columns.Add("LessonDate", "Дата занятия");
            lessonsGrid.Columns.Add("Time", "Время");
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

            tabControl.TabPages.Add(lessonsTab);

            // Вкладка 5: Ученики
            TabPage studentsTab = new TabPage("Ученики");
            studentsGrid = new DataGridView();
            studentsGrid.Location = new Point(10, 10);
            studentsGrid.Size = new Size(1130, 500);
            studentsGrid.Columns.Add("StudentID", "ID Ученика");
            studentsGrid.Columns.Add("StudentName", "Имя ученика");
            studentsGrid.Columns.Add("CourseID", "ID Курса");
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

            tabControl.TabPages.Add(studentsTab);

            // Кнопка "Выход" (перенесена в правый верхний угол)
            Button exitButton = new Button();
            exitButton.Text = "Выход из системы";
            exitButton.Font = new Font("Arial", 14);
            exitButton.Location = new Point(980,15); // Правый верхний угол: 1200 - 100 - 50 (отступ) = 1050
            exitButton.Size = new Size(190, 40);
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }

        private void RefreshGrid(string tableName, DataGridView grid)
        {
            MessageBox.Show($"Данные таблицы '{tableName}' будут обновлены после подключения к базе данных.",
                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteFromGrid(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in grid.SelectedRows)
                    {
                        grid.Rows.Remove(row);
                    }
                    MessageBox.Show("Запись удалена (пока только из таблицы на экране).",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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