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
        private Button btnSecurityThreat;
        private Button btnEvacuationAlarm;

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
            StyleButton(btnSecurityThreat, Color.FromArgb(255, 215, 0));
            StyleButton(btnEvacuationAlarm, Color.FromArgb(0, 204, 102));
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
            // Инициализация компонентов
            this.lessonsGridView = new DataGridView();
            this.breaksGridView = new DataGridView();
            this.btnAddLesson = new Button();
            this.btnDeleteLesson = new Button();
            this.btnDeleteBreak = new Button();
            this.startTimePicker = new DateTimePicker();
            this.endTimePicker = new DateTimePicker();
            this.lessonNumberTextBox = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.btnFireAlarm = new Button();
            this.btnSecurityThreat = new Button();
            this.btnEvacuationAlarm = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.lessonsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.breaksGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // lessonsGridView
            // 
            this.lessonsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lessonsGridView.Location = new Point(45, 55);
            this.lessonsGridView.Name = "lessonsGridView";
            this.lessonsGridView.Size = new Size(707, 231);
            this.lessonsGridView.TabIndex = 0;
            this.lessonsGridView.AutoGenerateColumns = true;
            this.lessonsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.lessonsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lessonsGridView.ReadOnly = true;
            // 
            // breaksGridView
            // 
            this.breaksGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.breaksGridView.Location = new Point(45, 505);
            this.breaksGridView.Name = "breaksGridView";
            this.breaksGridView.Size = new Size(707, 231);
            this.breaksGridView.TabIndex = 1;
            this.breaksGridView.AutoGenerateColumns = true;
            this.breaksGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.breaksGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.breaksGridView.ReadOnly = true;
            // 
            // btnAddLesson
            // 
            this.btnAddLesson.Location = new Point(45, 405);
            this.btnAddLesson.Name = "btnAddLesson";
            this.btnAddLesson.Size = new Size(100, 35);
            this.btnAddLesson.TabIndex = 2;
            this.btnAddLesson.Text = "Добавить";
            this.btnAddLesson.UseVisualStyleBackColor = true;
            this.btnAddLesson.Click += new EventHandler(this.btnAddLesson_Click);
            // 
            // btnDeleteLesson
            // 
            this.btnDeleteLesson.Location = new Point(265, 405);
            this.btnDeleteLesson.Name = "btnDeleteLesson";
            this.btnDeleteLesson.Size = new Size(100, 35);
            this.btnDeleteLesson.TabIndex = 3;
            this.btnDeleteLesson.Text = "Удалить";
            this.btnDeleteLesson.UseVisualStyleBackColor = true;
            this.btnDeleteLesson.Click += new EventHandler(this.btnDeleteLesson_Click);
            // 
            // btnDeleteBreak
            // 
            this.btnDeleteBreak.Location = new Point(45, 745);
            this.btnDeleteBreak.Name = "btnDeleteBreak";
            this.btnDeleteBreak.Size = new Size(100, 35);
            this.btnDeleteBreak.TabIndex = 5;
            this.btnDeleteBreak.Text = "Удалить";
            this.btnDeleteBreak.UseVisualStyleBackColor = true;
            this.btnDeleteBreak.Click += new EventHandler(this.btnDeleteBreak_Click);
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "HH:mm";
            this.startTimePicker.Format = DateTimePickerFormat.Custom;
            this.startTimePicker.Location = new Point(141, 365);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new Size(107, 27);
            this.startTimePicker.TabIndex = 12;
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "HH:mm";
            this.endTimePicker.Format = DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new Point(257, 365);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Size = new Size(107, 27);
            this.endTimePicker.TabIndex = 13;
            // 
            // lessonNumberTextBox
            // 
            this.lessonNumberTextBox.Location = new Point(45, 365);
            this.lessonNumberTextBox.Name = "lessonNumberTextBox";
            this.lessonNumberTextBox.Size = new Size(69, 27);
            this.lessonNumberTextBox.TabIndex = 14;
            // 
            // label1 (Номер урока)
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(36, 337);
            this.label1.Name = "label1";
            this.label1.Size = new Size(101, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Номер урока";
            // 
            // label2 (Начало)
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(141, 337);
            this.label2.Name = "label2";
            this.label2.Size = new Size(61, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Начало";
            // 
            // label3 (Конец)
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(257, 337);
            this.label3.Name = "label3";
            this.label3.Size = new Size(53, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Конец";
            // 
            // btnFireAlarm
            // 
            this.btnFireAlarm.Location = new Point(865, 55);
            this.btnFireAlarm.Name = "btnFireAlarm";
            this.btnFireAlarm.Size = new Size(181, 115);
            this.btnFireAlarm.TabIndex = 18;
            this.btnFireAlarm.Text = "Пожар";
            this.btnFireAlarm.UseVisualStyleBackColor = true;
            this.btnFireAlarm.Click += new EventHandler(this.btnFireAlarm_Click);
            // 
            // btnSecurityThreat
            // 
            this.btnSecurityThreat.Location = new Point(865, 289);
            this.btnSecurityThreat.Name = "btnSecurityThreat";
            this.btnSecurityThreat.Size = new Size(181, 115);
            this.btnSecurityThreat.TabIndex = 19;
            this.btnSecurityThreat.Text = "Угроза безопасности";
            this.btnSecurityThreat.UseVisualStyleBackColor = true;
            this.btnSecurityThreat.Click += new EventHandler(this.btnSecurityThreat_Click);
            // 
            // btnEvacuationAlarm
            // 
            this.btnEvacuationAlarm.Location = new Point(865, 580);
            this.btnEvacuationAlarm.Name = "btnEvacuationAlarm";
            this.btnEvacuationAlarm.Size = new Size(181, 115);
            this.btnEvacuationAlarm.TabIndex = 20;
            this.btnEvacuationAlarm.Text = "Эвакуация";
            this.btnEvacuationAlarm.UseVisualStyleBackColor = true;
            this.btnEvacuationAlarm.Click += new EventHandler(this.btnEvacuationAlarm_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1208, 886);
            this.Controls.Add(this.btnEvacuationAlarm);
            this.Controls.Add(this.btnSecurityThreat);
            this.Controls.Add(this.btnFireAlarm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lessonNumberTextBox);
            this.Controls.Add(this.endTimePicker);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.btnDeleteBreak);
            this.Controls.Add(this.btnDeleteLesson);
            this.Controls.Add(this.btnAddLesson);
            this.Controls.Add(this.breaksGridView);
            this.Controls.Add(this.lessonsGridView);
            this.Name = "MainForm";
            this.Load += new EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lessonsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.breaksGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }


    #endregion

}
