namespace BellManager
{
    /// <summary>
    /// ����� ��� ������� ����
    /// </summary>
    public partial class Form1 : Form
    { 
        private readonly TelegramBot _telegramBot;

        /// <summary>
        /// ������ ���� ����� ������������ �����
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _telegramBot = new TelegramBot("7528193408:AAFyeCTvOYFoiGs5-h-PbdW_qqsRW6B3xYc"); 
        }

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _telegramBot.Stop(); 
        }
        /// <summary>
        /// ������ ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Load_1(object sender, EventArgs e)
        {
            await _telegramBot.StartAsync();
            this.Hide();

        }
    }
}
