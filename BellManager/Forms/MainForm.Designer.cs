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
          private System.Windows.Forms.DataGridView lessonsGridView;
        private System.Windows.Forms.DataGridView breaksGridView;
        private System.Windows.Forms.Button btnAddLesson;
        private System.Windows.Forms.Button btnDeleteLesson;
        private System.Windows.Forms.Button btnDeleteBreak;
        private System.Windows.Forms.MonthCalendar holidayCalendar;
        private System.Windows.Forms.Button btnAddHoliday;
        private System.Windows.Forms.Button btnDeleteHoliday;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.TextBox lessonNumberTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFireAlarm;
        private System.Windows.Forms.Button btnSecurityThreat;
        private System.Windows.Forms.Button btnEvacuationAlarm;
        private System.Windows.Forms.TextBox txtLessonName;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.TextBox txtBreakName;
        private System.Windows.Forms.TextBox txtBreakStart;
        private System.Windows.Forms.TextBox txtBreakEnd;
        private System.Windows.Forms.TextBox txtMusicFile;
        private System.Windows.Forms.DataGridView dataGridViewLessons;
        private System.Windows.Forms.DataGridView dataGridViewBreaks;
        private System.Windows.Forms.Button btnStartScheduler;
        private System.Windows.Forms.Button btnStopScheduler;
        private System.Windows.Forms.Label lblStatus;


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
            btnSecurityThreat = new Button();
            btnEvacuationAlarm = new Button();
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).BeginInit();
            SuspendLayout();
            // Настройка стиля формы
            this.Text = "Менеджер звонков";
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 9);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // 
            // lessonsGridView
            // 
            lessonsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            lessonsGridView.Location = new Point(45, 55);
            lessonsGridView.Margin = new Padding(4, 5, 4, 5);
            lessonsGridView.Name = "lessonsGridView";
            lessonsGridView.RowHeadersWidth = 51;
            lessonsGridView.Size = new Size(707, 231);
            lessonsGridView.TabIndex = 0;
            lessonsGridView.AutoGenerateColumns = true;
            lessonsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lessonsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lessonsGridView.ReadOnly = true;
            // 
            // breaksGridView
            // 
            breaksGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            breaksGridView.Location = new Point(45, 505);
            breaksGridView.Margin = new Padding(4, 5, 4, 5);
            breaksGridView.Name = "breaksGridView";
            breaksGridView.RowHeadersWidth = 51;
            breaksGridView.Size = new Size(707, 231);
            breaksGridView.TabIndex = 1;
            // Настройка DataGridView для перемен
            breaksGridView.AutoGenerateColumns = true;
            breaksGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            breaksGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            breaksGridView.ReadOnly = true;
            // 
            // btnAddLesson
            // 
            btnAddLesson.Location = new Point(45, 405);
            btnAddLesson.Margin = new Padding(4, 5, 4, 5);
            btnAddLesson.Name = "btnAddLesson";
            btnAddLesson.Size = new Size(100, 35);
            btnAddLesson.TabIndex = 2;
            btnAddLesson.Text = "Добавить";
            btnAddLesson.UseVisualStyleBackColor = true;
            btnAddLesson.Click += btnAddLesson_Click;
            btnAddLesson.BackColor = System.Drawing.Color.LightBlue;
            btnAddLesson.FlatStyle = FlatStyle.Flat;
            btnAddLesson.FlatAppearance.BorderSize = 0;
            // 
            // btnDeleteLesson
            // 
            btnDeleteLesson.Location = new Point(265, 405);
            btnDeleteLesson.Margin = new Padding(4, 5, 4, 5);
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
            btnDeleteBreak.Margin = new Padding(4, 5, 4, 5);
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
            startTimePicker.Margin = new Padding(4, 5, 4, 5);
            startTimePicker.Name = "startTimePicker";
            startTimePicker.Size = new Size(107, 27);
            startTimePicker.TabIndex = 12;
            // 
            // endTimePicker
            // 
            endTimePicker.CustomFormat = "HH:mm";
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.Location = new Point(257, 365);
            endTimePicker.Margin = new Padding(4, 5, 4, 5);
            endTimePicker.Name = "endTimePicker";
            endTimePicker.Size = new Size(107, 27);
            endTimePicker.TabIndex = 13;
            // 
            // lessonNumberTextBox
            // 
            lessonNumberTextBox.Location = new Point(45, 365);
            lessonNumberTextBox.Margin = new Padding(4, 5, 4, 5);
            lessonNumberTextBox.Name = "lessonNumberTextBox";
            lessonNumberTextBox.Size = new Size(69, 27);
            lessonNumberTextBox.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 337);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 15;
            label1.Text = "Номер урока";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(141, 337);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 16;
            label2.Text = "Начало";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(257, 337);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(53, 20);
            label3.TabIndex = 17;
            label3.Text = "Конец";
            // 
            // btnFireAlarm
            // 
            btnFireAlarm.Location = new Point(865, 55);
            btnFireAlarm.Margin = new Padding(4, 5, 4, 5);
            btnFireAlarm.Name = "btnFireAlarm";
            btnFireAlarm.Size = new Size(181, 115);
            btnFireAlarm.TabIndex = 18;
            btnFireAlarm.Text = "Пожар";
            btnFireAlarm.UseVisualStyleBackColor = true;
            btnFireAlarm.Click += btnFireAlarm_Click;
            // 
            // btnSecurityThreat
            // 
            btnSecurityThreat.Location = new Point(865, 289);
            btnSecurityThreat.Margin = new Padding(4, 5, 4, 5);
            btnSecurityThreat.Name = "btnSecurityThreat";
            btnSecurityThreat.Size = new Size(181, 115);
            btnSecurityThreat.TabIndex = 19;
            btnSecurityThreat.Text = "Угроза безопасности";
            btnSecurityThreat.UseVisualStyleBackColor = true;
            btnSecurityThreat.Click += btnSecurityThreat_Click;
            // 
            // btnEvacuationAlarm
            // 
            btnEvacuationAlarm.Location = new Point(865, 580);
            btnEvacuationAlarm.Margin = new Padding(4, 5, 4, 5);
            btnEvacuationAlarm.Name = "btnEvacuationAlarm";
            btnEvacuationAlarm.Size = new Size(181, 115);
            btnEvacuationAlarm.TabIndex = 20;
            btnEvacuationAlarm.Text = "Эвакуация";
            btnEvacuationAlarm.UseVisualStyleBackColor = true;
            btnEvacuationAlarm.Click += btnEvacuationAlarm_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 886);
            Controls.Add(btnEvacuationAlarm);
            Controls.Add(btnSecurityThreat);
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
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }


    #endregion

}
