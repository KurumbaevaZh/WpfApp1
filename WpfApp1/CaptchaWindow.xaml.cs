using SkiaSharp;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;



namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CaptchaWindow.xaml
    /// </summary>
    public partial class CaptchaWindow : Window
    {  
        // Переменная для хранения текста капчи
        private string generatedCaptchaText = "ABC123";
        private string captchaText; // Переменная для хранения текста капчи
        public CaptchaWindow(string captchaText = null)
        {
            InitializeComponent();
            this.captchaText = captchaText ?? GenerateCaptchaText();

            // Генерация и установка изображения капчи
            var captchaImage = GenerateCaptchaImage(this.captchaText);
            CaptchaImage.Source = captchaImage;
        }
        private string GenerateCaptchaText()
        {
            // Генерация строки капчи, например, случайной последовательности из 5-7 символов
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }     
        private BitmapSource GenerateCaptchaImage(string captchaText)
        {
            int width = 150;
            int height = 50;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(System.Drawing.Color.White);
            Font font = new Font("Arial", 20, System.Drawing.FontStyle.Bold);
            System.Drawing.Brush brush = System.Drawing.Brushes.Black;
            graphics.DrawString(captchaText, font, brush, new PointF(10, 10));
            AddNoise(graphics, width, height);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private void AddNoise(Graphics graphics, int width, int height)
        {
            Random rand = new Random();
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            // Добавляем линии для усложнения капчи
            for (int i = 0; i < 10; i++)
            {
                int x1 = rand.Next(width);
                int y1 = rand.Next(height);
                int x2 = rand.Next(width);
                int y2 = rand.Next(height);
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }
        private void DrawCaptcha()
        {
            System.Windows.Point point = new System.Windows.Point(50, 50);
        }
        // Метод для проверки капчи
        private bool ValidateCaptcha(string userInput)
        {
            return userInput == generatedCaptchaText; // Сравнивает введенный текст с сгенерированным текстом
        }
        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCaptcha(EnteredCaptchaTextBox.Text))
            {
                MessageBox.Show("Капча введена верно!");
                this.DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Неверная капча, попробуйте снова.");
            }
        }


    }

}
