using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BellManager.Models;
using BellManager.Service;
using BellManager.Service.BellManager;

namespace BellManager
{
    public partial class MainForm : Form
    {
        private readonly LessonService _lessonService;
        private readonly BreakService _breakService;
        private readonly BellManagerService bellManager;
        private readonly BellManagerService _bellManager;
        public MainForm()
        {
            _lessonService = new LessonService();
            _breakService = new BreakService();
            InitializeComponent();
            _bellManager = new BellManagerService();

            LoadLessons();
            LoadBreaks();
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
                    // Добавление в базу данных через LessonService
                  await  _lessonService.AddLesson(newLesson);
                    // Загружаем все уроки, чтобы пересчитать перемены
                    var lessons = await _lessonService.GetLessons();
                  await  _breakService.AddBreaksBetweenLessons(lessons); // Создание перемен

                    // Обновляем отображение данных
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
            /// _bellManager.ToggleAlarm("Пожар", (Button)sender);
        }

        private void btnSecurityThreat_Click(object sender, EventArgs e)
        {

            ///  _bellManager.ToggleAlarm("Угроза безопасности", (Button)sender);
        }

        private void btnEvacuationAlarm_Click(object sender, EventArgs e)
        {

            /// _bellManager.ToggleAlarm("Эвакуация", (Button)sender);
        }

        private async void btnAddBreak_Click(object sender, EventArgs e)
        {
            var breakItem = new Break
            {
                Name = txtBreakName.Text,
                StartTime = TimeSpan.Parse(txtBreakStart.Text),
                EndTime = TimeSpan.Parse(txtBreakEnd.Text),
                MusicFile = txtMusicFile.Text
            };

            await _breakService.AddBreak(breakItem);
            LoadBreaks();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}