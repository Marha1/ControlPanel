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
            // 
            // btnAddLesson
            // 
            btnAddLesson.Location = new Point(45, 405);
            btnAddLesson.Margin = new Padding(4, 5, 4, 5);
            btnAddLesson.Name = "btnAddLesson";
            btnAddLesson.Size = new Size(100, 35);
            btnAddLesson.TabIndex = 2;
            btnAddLesson.Text = "Добавить урок";
            btnAddLesson.UseVisualStyleBackColor = true;
            btnAddLesson.Click += btnAddLesson_Click;
            // 
            // btnDeleteLesson
            // 
            btnDeleteLesson.Location = new Point(265, 405);
            btnDeleteLesson.Margin = new Padding(4, 5, 4, 5);
            btnDeleteLesson.Name = "btnDeleteLesson";
            btnDeleteLesson.Size = new Size(100, 35);
            btnDeleteLesson.TabIndex = 3;
            btnDeleteLesson.Text = "Удалить урок";
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
            btnDeleteBreak.Text = "Удалить перемену";
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
            Text = "Менеджер звонков";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
            ApplyModernStyle();
        }

        private void ApplyModernStyle()
        {
            this.Text = "Менеджер звонков";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создаем тень вокруг формы
            this.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            };

            // Улучшаем DataGridView
            CustomizeGridView(lessonsGridView);
            CustomizeGridView(breaksGridView);

            // Кастомизация кнопок
            StyleButton(btnAddLesson);
            StyleButton(btnDeleteLesson);
            StyleButton(btnDeleteBreak);
            StyleButton(btnFireAlarm, Color.Crimson);
            StyleButton(btnSecurityThreat, Color.DarkOrange);
            StyleButton(btnEvacuationAlarm, Color.DarkBlue);
        }

        private void CustomizeGridView(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.BackgroundColor = Color.WhiteSmoke;
            grid.GridColor = Color.LightGray;
            grid.BorderStyle = BorderStyle.None;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void StyleButton(Button btn, Color? color = null)
        {
            btn.BackColor = color ?? Color.FromArgb(52, 152, 219);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Height = 40;
            btn.Cursor = Cursors.Hand;
            btn.Region = new Region(new Rectangle(0, 0, btn.Width, btn.Height));

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(btn.BackColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = color ?? Color.FromArgb(52, 152, 219);
        }
    }
}