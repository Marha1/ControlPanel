namespace BellManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        // Объявление компонентов
        private DataGridView lessonsGridView;
        private DataGridView breaksGridView;
        private Button btnAddLesson;
        private Button btnDeleteLesson;
        private Button btnDeleteBreak;
        private DateTimePicker startTimePicker;
        private DateTimePicker endTimePicker;
        private TextBox lessonNumberTextBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnFireAlarm;

        public MainForm()
        {
            InitializeComponent();
            CustomizeUI();
        }

        private void CustomizeUI()
        {
            // Убираем стандартную рамку и задаём тёмный фон
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Менеджер звонков";

            // Стилизация DataGridView для уроков
            lessonsGridView.BorderStyle = BorderStyle.None;
            lessonsGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            lessonsGridView.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            lessonsGridView.DefaultCellStyle.ForeColor = Color.White;
            lessonsGridView.EnableHeadersVisualStyles = false;
            lessonsGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 70, 70);
            lessonsGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            lessonsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Стилизация DataGridView для перемен
            breaksGridView.BorderStyle = BorderStyle.None;
            breaksGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            breaksGridView.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            breaksGridView.DefaultCellStyle.ForeColor = Color.White;
            breaksGridView.EnableHeadersVisualStyles = false;
            breaksGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 70, 70);
            breaksGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            breaksGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Стилизация кнопок
            StyleButton(btnAddLesson, Color.FromArgb(0, 122, 204));
            StyleButton(btnDeleteLesson, Color.FromArgb(204, 0, 0));
            StyleButton(btnDeleteBreak, Color.FromArgb(204, 0, 0));
            StyleButton(btnFireAlarm, Color.FromArgb(255, 140, 0));
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
        }

        private void InitializeComponent()
        {
            lessonsGridView = new DataGridView();
            breaksGridView = new DataGridView();
            btnAddLesson = new Button();
            btnDeleteLesson = new Button();
            btnDeleteBreak = new Button();
            startTimePicker = new DateTimePicker();
            endTimePicker = new DateTimePicker();
            lessonNumberTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnFireAlarm = new Button();
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).BeginInit();
            SuspendLayout();
            // 
            // lessonsGridView
            // 
            lessonsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lessonsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            lessonsGridView.Location = new Point(45, 55);
            lessonsGridView.Name = "lessonsGridView";
            lessonsGridView.ReadOnly = true;
            lessonsGridView.RowHeadersWidth = 51;
            lessonsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lessonsGridView.Size = new Size(707, 231);
            lessonsGridView.TabIndex = 0;
            // 
            // breaksGridView
            // 
            breaksGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            breaksGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            breaksGridView.Location = new Point(45, 505);
            breaksGridView.Name = "breaksGridView";
            breaksGridView.ReadOnly = true;
            breaksGridView.RowHeadersWidth = 51;
            breaksGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            breaksGridView.Size = new Size(707, 231);
            breaksGridView.TabIndex = 1;
            // 
            // btnAddLesson
            // 
            btnAddLesson.Location = new Point(45, 405);
            btnAddLesson.Name = "btnAddLesson";
            btnAddLesson.Size = new Size(100, 35);
            btnAddLesson.TabIndex = 2;
            btnAddLesson.Text = "Добавить";
            btnAddLesson.UseVisualStyleBackColor = true;
            btnAddLesson.Click += btnAddLesson_Click;
            // 
            // btnDeleteLesson
            // 
            btnDeleteLesson.Location = new Point(265, 405);
            btnDeleteLesson.Name = "btnDeleteLesson";
            btnDeleteLesson.Size = new Size(100, 35);
            btnDeleteLesson.TabIndex = 3;
            btnDeleteLesson.Text = "Удалить";
            btnDeleteLesson.UseVisualStyleBackColor = true;
            btnDeleteLesson.Click += btnDeleteLesson_Click;
            // 
            // btnDeleteBreak
            // 
            btnDeleteBreak.Location = new Point(45, 745);
            btnDeleteBreak.Name = "btnDeleteBreak";
            btnDeleteBreak.Size = new Size(100, 35);
            btnDeleteBreak.TabIndex = 5;
            btnDeleteBreak.Text = "Удалить";
            btnDeleteBreak.UseVisualStyleBackColor = true;
            btnDeleteBreak.Click += btnDeleteBreak_Click;
            // 
            // startTimePicker
            // 
            startTimePicker.CustomFormat = "HH:mm";
            startTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.Location = new Point(141, 365);
            startTimePicker.Name = "startTimePicker";
            startTimePicker.Size = new Size(107, 27);
            startTimePicker.TabIndex = 12;
            // 
            // endTimePicker
            // 
            endTimePicker.CustomFormat = "HH:mm";
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.Location = new Point(257, 365);
            endTimePicker.Name = "endTimePicker";
            endTimePicker.Size = new Size(107, 27);
            endTimePicker.TabIndex = 13;
            // 
            // lessonNumberTextBox
            // 
            lessonNumberTextBox.Location = new Point(45, 365);
            lessonNumberTextBox.Name = "lessonNumberTextBox";
            lessonNumberTextBox.Size = new Size(69, 27);
            lessonNumberTextBox.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 337);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 15;
            label1.Text = "Номер урока";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(141, 337);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 16;
            label2.Text = "Начало";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(257, 337);
            label3.Name = "label3";
            label3.Size = new Size(53, 20);
            label3.TabIndex = 17;
            label3.Text = "Конец";
            // 
            // btnFireAlarm
            // 
            btnFireAlarm.Location = new Point(862, 321);
            btnFireAlarm.Name = "btnFireAlarm";
            btnFireAlarm.Size = new Size(181, 115);
            btnFireAlarm.TabIndex = 18;
            btnFireAlarm.Text = "Пожар";
            btnFireAlarm.UseVisualStyleBackColor = true;
            btnFireAlarm.Click += btnFireAlarm_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 886);
            Controls.Add(btnFireAlarm);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lessonNumberTextBox);
            Controls.Add(endTimePicker);
            Controls.Add(startTimePicker);
            Controls.Add(btnDeleteBreak);
            Controls.Add(btnDeleteLesson);
            Controls.Add(btnAddLesson);
            Controls.Add(breaksGridView);
            Controls.Add(lessonsGridView);
            Name = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }


    #endregion

}
