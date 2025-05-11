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
        private Panel titleBar;
        private Button btnClose;
        private Button btnMinimize;
        private Label titleLabel;
        private Panel panel1;
        private Panel panel2;

        public MainForm()
        {
            InitializeComponent();
            CustomizeUI();
        }

        private void CustomizeUI()
        {
            // Основные настройки формы
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(15, 15, 20);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Менеджер звонков";
            this.Padding = new Padding(1);
            this.DoubleBuffered = true;

            // Настройка заголовка
            titleBar.BackColor = Color.FromArgb(25, 25, 35);
            titleLabel.ForeColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);

            // Стилизация кнопок заголовка
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 50, 50);
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 90);

            // Стилизация DataGridView для уроков
            StyleDataGridView(lessonsGridView, Color.FromArgb(30, 30, 40), Color.FromArgb(50, 50, 60),
                            Color.FromArgb(70, 70, 80), new Font("Segoe UI", 9.5F));

            // Стилизация DataGridView для перемен
            StyleDataGridView(breaksGridView, Color.FromArgb(30, 30, 40), Color.FromArgb(50, 50, 60),
                            Color.FromArgb(70, 70, 80), new Font("Segoe UI", 9.5F));

            // Стилизация кнопок
            StyleButton(btnAddLesson, Color.FromArgb(0, 150, 255), 12, 30);
            StyleButton(btnDeleteLesson, Color.FromArgb(220, 80, 80), 12, 30);
            StyleButton(btnDeleteBreak, Color.FromArgb(220, 80, 80), 12, 30);
            StyleButton(btnFireAlarm, Color.FromArgb(255, 160, 0), 14, 40, true);

            // Стилизация панелей
            panel1.BackColor = Color.FromArgb(25, 25, 35);
            panel2.BackColor = Color.FromArgb(25, 25, 35);

            // Стилизация DateTimePicker
            StyleDateTimePicker(startTimePicker);
            StyleDateTimePicker(endTimePicker);

            // Стилизация TextBox
            StyleTextBox(lessonNumberTextBox);

            // Стилизация Label
            StyleLabel(label1);
            StyleLabel(label2);
            StyleLabel(label3);
        }

        private void StyleDataGridView(DataGridView dgv, Color bgColor, Color cellColor, Color headerColor, Font font)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = bgColor;
            dgv.DefaultCellStyle.BackColor = cellColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = font;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv.ColumnHeadersHeight = 35;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.GridColor = Color.FromArgb(60, 60, 70);
        }

        private void StyleButton(Button btn, Color bgColor, float fontSize = 12, int borderRadius = 20, bool isBig = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bgColor, 0.2f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);

            if (isBig)
            {
                btn.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                btn.TextAlign = ContentAlignment.MiddleCenter;
            }

            // Создаем скругленные углы
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
            path.AddArc(btn.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
            path.AddArc(btn.Width - borderRadius, btn.Height - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(0, btn.Height - borderRadius, borderRadius, borderRadius, 90, 90);
            btn.Region = new Region(path);
        }

        private void StyleDateTimePicker(DateTimePicker picker)
        {
            picker.CalendarMonthBackground = Color.FromArgb(50, 50, 60);
            picker.CalendarTitleBackColor = Color.FromArgb(70, 70, 80);
            picker.CalendarTitleForeColor = Color.White;
            picker.CalendarTrailingForeColor = Color.FromArgb(150, 150, 150);
            picker.CalendarForeColor = Color.White;
            picker.BackColor = Color.FromArgb(50, 50, 60);
            picker.ForeColor = Color.White;
            picker.Font = new Font("Segoe UI", 10);
            picker.Format = DateTimePickerFormat.Custom;
            picker.CustomFormat = "HH:mm";
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(50, 50, 60);
            textBox.ForeColor = Color.White;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Segoe UI", 10);
        }

        private void StyleLabel(Label label)
        {
            label.ForeColor = Color.FromArgb(180, 180, 190);
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            titleBar = new Panel();
            btnClose = new Button();
            btnMinimize = new Button();
            titleLabel = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).BeginInit();
            titleBar.SuspendLayout();
            SuspendLayout();
            // 
            // lessonsGridView
            // 
            lessonsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lessonsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            lessonsGridView.Location = new Point(45, 85);
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
            breaksGridView.Location = new Point(45, 535);
            breaksGridView.Name = "breaksGridView";
            breaksGridView.ReadOnly = true;
            breaksGridView.RowHeadersWidth = 51;
            breaksGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            breaksGridView.Size = new Size(707, 231);
            breaksGridView.TabIndex = 1;
            // 
            // btnAddLesson
            // 
            btnAddLesson.Location = new Point(45, 435);
            btnAddLesson.Name = "btnAddLesson";
            btnAddLesson.Size = new Size(120, 40);
            btnAddLesson.TabIndex = 2;
            btnAddLesson.Text = "Добавить";
            btnAddLesson.UseVisualStyleBackColor = true;
            btnAddLesson.Click += btnAddLesson_Click;
            // 
            // btnDeleteLesson
            // 
            btnDeleteLesson.Location = new Point(265, 435);
            btnDeleteLesson.Name = "btnDeleteLesson";
            btnDeleteLesson.Size = new Size(120, 40);
            btnDeleteLesson.TabIndex = 3;
            btnDeleteLesson.Text = "Удалить";
            btnDeleteLesson.UseVisualStyleBackColor = true;
            btnDeleteLesson.Click += btnDeleteLesson_Click;
            // 
            // btnDeleteBreak
            // 
            btnDeleteBreak.Location = new Point(45, 782);
            btnDeleteBreak.Name = "btnDeleteBreak";
            btnDeleteBreak.Size = new Size(120, 40);
            btnDeleteBreak.TabIndex = 5;
            btnDeleteBreak.Text = "Удалить";
            btnDeleteBreak.UseVisualStyleBackColor = true;
            btnDeleteBreak.Click += btnDeleteBreak_Click;
            // 
            // startTimePicker
            // 
            startTimePicker.CustomFormat = "HH:mm";
            startTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.Location = new Point(141, 395);
            startTimePicker.Name = "startTimePicker";
            startTimePicker.Size = new Size(107, 27);
            startTimePicker.TabIndex = 12;
            // 
            // endTimePicker
            // 
            endTimePicker.CustomFormat = "HH:mm";
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.Location = new Point(257, 395);
            endTimePicker.Name = "endTimePicker";
            endTimePicker.Size = new Size(107, 27);
            endTimePicker.TabIndex = 13;
            // 
            // lessonNumberTextBox
            // 
            lessonNumberTextBox.Location = new Point(45, 395);
            lessonNumberTextBox.Name = "lessonNumberTextBox";
            lessonNumberTextBox.Size = new Size(69, 27);
            lessonNumberTextBox.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 367);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 15;
            label1.Text = "Номер урока";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(141, 367);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 16;
            label2.Text = "Начало";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(257, 367);
            label3.Name = "label3";
            label3.Size = new Size(53, 20);
            label3.TabIndex = 17;
            label3.Text = "Конец";
            // 
            // btnFireAlarm
            // 
            btnFireAlarm.Location = new Point(862, 351);
            btnFireAlarm.Name = "btnFireAlarm";
            btnFireAlarm.Size = new Size(181, 115);
            btnFireAlarm.TabIndex = 18;
            btnFireAlarm.Text = "Пожар";
            btnFireAlarm.UseVisualStyleBackColor = true;
            btnFireAlarm.Click += btnFireAlarm_Click;
            // 
            // titleBar
            // 
            titleBar.BackColor = Color.FromArgb(25, 25, 35);
            titleBar.Controls.Add(btnClose);
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(titleLabel);
            titleBar.Dock = DockStyle.Top;
            titleBar.Location = new Point(0, 0);
            titleBar.Name = "titleBar";
            titleBar.Size = new Size(1208, 40);
            titleBar.TabIndex = 19;
            titleBar.MouseDown += TitleBar_MouseDown;
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1128, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(40, 40);
            btnClose.TabIndex = 2;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnMinimize
            // 
            btnMinimize.Dock = DockStyle.Right;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(1168, 0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(40, 40);
            btnMinimize.TabIndex = 1;
            btnMinimize.Text = "_";
            btnMinimize.UseVisualStyleBackColor = true;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(12, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(204, 28);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Менеджер звонков";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(25, 25, 35);
            panel1.Location = new Point(35, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(727, 251);
            panel1.TabIndex = 20;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(25, 25, 35);
            panel2.Location = new Point(35, 525);
            panel2.Name = "panel2";
            panel2.Size = new Size(727, 251);
            panel2.TabIndex = 21;
            // 
            // MainForm
            // 
            this.FormBorderStyle = FormBorderStyle.None;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 886);
            Controls.Add(titleBar);
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
            Controls.Add(panel1);
            Controls.Add(panel2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)lessonsGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)breaksGridView).EndInit();
            titleBar.ResumeLayout(false);
            titleBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        internal static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            internal static extern bool ReleaseCapture();

            internal const int WM_NCLBUTTONDOWN = 0xA1;
            internal const int HT_CAPTION = 0x2;
        }

        #endregion
    }
}