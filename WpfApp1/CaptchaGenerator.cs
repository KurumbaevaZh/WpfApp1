using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; // Для работы с классами рисования, включая Bitmap
using System.Drawing.Imaging; // Для доступа к ImageFormat


namespace WpfApp1
{
    internal class CaptchaGenerator
    {
        private Random random = new Random();
        private string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public Bitmap GenerateCaptcha(out string captchaText)
        {
            int length = random.Next(4, 8); // Длина текста от 4 до 7 символов
            captchaText = new string(Enumerable.Range(0, length)
                .Select(_ => characters[random.Next(characters.Length)]).ToArray());

            Bitmap bitmap = new Bitmap(200, 70);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Создание контрастного фона с шумом
                graphics.Clear(Color.FromArgb(230, 230, 230));
                // Добавление шума
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(0, bitmap.Width);
                    int y = random.Next(0, bitmap.Height);
                    graphics.FillRectangle(Brushes.Gray, x, y, 1, 1);
                }

                // Настройка шрифта
                Font font = new Font("Arial", random.Next(20, 30), FontStyle.Bold);
                Brush textBrush = new SolidBrush(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));

                for (int i = 0; i < captchaText.Length; i++)
                {
                    float angle = (float)(random.NextDouble() * 30 - 15); // Угол от -15 до 15 градусов
                    graphics.TranslateTransform(random.Next(5, 150), random.Next(5, 50));
                    graphics.RotateTransform(angle);
                    graphics.DrawString(captchaText[i].ToString(), font, textBrush, 0, 0);
                    graphics.RotateTransform(-angle);
                    graphics.TranslateTransform(-random.Next(5, 150), -random.Next(5, 50));
                }

                // Добавление линий
                Pen pen = new Pen(Color.Gray, 2);
                for (int i = 0; i < 5; i++)
                {
                    graphics.DrawLine(pen, random.Next(0, bitmap.Width), random.Next(0, bitmap.Height),
                                      random.Next(0, bitmap.Width), random.Next(0, bitmap.Height));
                }
            }

            return bitmap;
        }

        public void SaveCaptchaImage(Bitmap captchaImage, string filePath)
        {
            captchaImage.Save(filePath, ImageFormat.Png);
        }

    }
}
