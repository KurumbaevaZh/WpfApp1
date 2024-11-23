using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WpfApp1.MainWindow;
using System.IO;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window

    {
        static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=nadezhdaa";
        

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Создание и открытие нового окна MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }

        private int failedLoginAttempts = 0;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            var roleId = AuthenticateUser(username, password);

            if (roleId == "admin")
            {
                MessageBox.Show("Вход успешен. Вы администратор");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else if (roleId == "user")
            {
                MessageBox.Show("Вход успешен. Вы клиент");
                ClientsWindow clientWindow = new ClientsWindow();
                clientWindow.Show();
                this.Close();
            }
            else
            {
                failedLoginAttempts++;

                if (failedLoginAttempts >= 3)//если больше 3х неправильных попыток авторизации
                {
                    MessageBox.Show("Показываем капчу");
                    ShowCaptcha(); // Вызываем окно капчи
                }

                MessageBox.Show("Неправильный логин или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ShowCaptcha()
        {
            var captchaWindow = new CaptchaWindow();
            captchaWindow.Owner = this; 
            captchaWindow.ShowDialog(); 
        }
        private bool IsValidUser(string username, string password)
        {
            return !string.IsNullOrEmpty(AuthenticateUser(username, password)); // Если роль пользователя не пустая, значит, вход успешен
        }
        private string AuthenticateUser(string username, string password)
        {
            string roleId = "";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Запрос для получения хеша пароля, соли и роли пользователя
                    string query = "SELECT \"hashed_password\", \"salt\", \"role\" FROM public.users WHERE \"username\" = @username";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader.GetString(0); // Хранимый хеш пароля
                                string salt = reader.GetString(1);       // Хранимая соль
                                roleId = reader.GetString(2);            // Роль пользователя

                                // Проверяем введенный пароль с помощью VerifyPassword
                                if (!PasswordHasher.VerifyPassword(password, storedHash, salt))
                                {
                                    roleId = ""; // Если пароли не совпадают, сбрасываем роль
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return roleId;
        }



    }

}
