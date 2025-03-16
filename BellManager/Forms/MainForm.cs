using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BellManager.Models;
using BellManager.Service;

namespace BellManager
{
    public partial class MainForm : Form
    {
        private readonly LessonService _lessonService;
        private readonly BreakService _breakService;
        private readonly BellManagerService _bellManager;
        public MainForm(BellManagerService bellManagerService)
        {
            _lessonService = new LessonService();
            _breakService = new BreakService();
            InitializeComponent();
            _bellManager = bellManagerService;
            AddChangeMusicButtonColumn();
            breaksGridView.CellContentClick += breaksGridView_CellContentClick;
            LoadLessons();
            LoadBreaks();
        }
        private void breaksGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажатие произошло на столбце с кнопкой
            if (e.ColumnIndex == breaksGridView.Columns["ChangeMusicColumn"].Index && e.RowIndex >= 0)
            {
                // Получаем выбранную перемену
                var selectedBreak = (Break)breaksGridView.Rows[e.RowIndex].DataBoundItem;

                // Открываем диалоговое окно для выбора файла
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Audio Files|*.mp3;*.wav;*.ogg",
                    Title = "Выберите музыкальный файл"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Обновляем путь к музыкальному файлу
                    selectedBreak.MusicFile = openFileDialog.FileName;

                    // Сохраняем изменения в базе данных
                    _breakService.Update(selectedBreak);
                    LoadBreaks();
                }
            }
        }

        private async void btnDeleteLesson_Click(object sender, EventArgs e)
        {
            if (lessonsGridView.SelectedRows.Count > 0)
            {
                var selectedLesson = (Lesson)lessonsGridView.SelectedRows[0].DataBoundItem;
                await _lessonService.Delete(selectedLesson.Id);
                LoadLessons();
            }
            else
            {
                MessageBox.Show("Выберите урок для удаления.");
            }
        }
        private void AddChangeMusicButtonColumn()
        {
            // Создаем столбец с кнопкой
            DataGridViewButtonColumn changeMusicButtonColumn = new DataGridViewButtonColumn
            {
                Name = "ChangeMusicColumn",
                HeaderText = "Изменить музыку",
                Text = "Изменить",
                UseColumnTextForButtonValue = true // Отображаем текст на кнопке
            };

            // Добавляем столбец в DataGridView
            breaksGridView.Columns.Add(changeMusicButtonColumn);
        }
        private async void LoadLessons()
        {
                var lessons = await _lessonService.GetLessons();
                lessonsGridView.DataSource = lessons;
          
        }
        private async void LoadBreaks()
        {
            var breaks = await _breakService.GetBreaks();
                breaksGridView.DataSource = breaks;

        }
        private async void btnDeleteBreak_Click(object sender, EventArgs e)
        {
            if (breaksGridView.SelectedRows.Count > 0)
            {
                var selectedBreak = (Break)breaksGridView.SelectedRows[0].DataBoundItem;
                await _breakService.Delete(selectedBreak.Id);
                LoadBreaks();
            }
            else
            {
                MessageBox.Show("Выберите перемену для удаления.");
            }
        }
       

        private async void btnAddLesson_Click(object sender, EventArgs e)
        {
            try
            {


                // Проверка на корректность ввода данных
                if (string.IsNullOrWhiteSpace(lessonNumberTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите номер урока.");
                    return;
                }

                if (startTimePicker.Value >= endTimePicker.Value)
                {
                    MessageBox.Show("Время начала урока не может быть позже времени конца.");
                    return;
                }

                // Получаем данные из элементов управления
                var lessonNumber = int.Parse(lessonNumberTextBox.Text);

                // Проверка: номер урока должен быть больше нуля
                if (lessonNumber <= 0)
                {
                    MessageBox.Show("Номер урока должен быть больше нуля.");
                    return;
                }

                // Извлекаем только часы и минуты, игнорируя секунды и миллисекунды
                var startTime = new TimeSpan(startTimePicker.Value.Hour, startTimePicker.Value.Minute, 0);
                var endTime = new TimeSpan(endTimePicker.Value.Hour, endTimePicker.Value.Minute, 0);

                // Создаем новый урок с указанным номером
                var newLesson = new Lesson
                {
                    Id = lessonNumber, // Присваиваем номер урока как ID
                    Name = $"Урок {lessonNumber}",
                    StartTime = startTime,
                    EndTime = endTime,
                    IsActive = true
                };

                try
                {
                  await  _lessonService.AddLesson(newLesson);
                    var lessons = await _lessonService.GetLessons();
                    
                  await  _breakService.AddBreaksBetweenLessons(lessons,string.Empty);

                    LoadLessons();
                    LoadBreaks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SafeClose()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SafeClose));
            }
            else
            {
                this.Hide();
                this.Dispose();
            }
        }
        private void btnFireAlarm_Click(object sender, EventArgs e)
        {
             _bellManager.ToggleAlarm();
        }

        private void btnSecurityThreat_Click(object sender, EventArgs e)
        {

            ///  _bellManager.ToggleAlarm("Угроза безопасности", (Button)sender);
        }

        private void btnEvacuationAlarm_Click(object sender, EventArgs e)
        {

            /// _bellManager.ToggleAlarm("Эвакуация", (Button)sender);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}