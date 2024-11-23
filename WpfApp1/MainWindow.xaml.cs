using CsvHelper;
using Microsoft.Win32;
using Npgsql;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using static WpfApp1.MainWindow;
using MaterialSkin;
using MaterialSkin.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connectionString = "Host=localhost; Port=5432; Username=postgres; Password=postgres; Database=nadezhdaa";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);

        public class Service
        {
            public int ID { get; set; }
            public string ServiceName { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
        }
        private List<Service> services = new List<Service>();
        private DataService dataService = new DataService();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void OpenReportsWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportsWindow reportsWindow = new ReportsWindow();
                reportsWindow.Show();
            }
            catch (Exception ex)
            {
              
            }
        }


        private void OpenOrdersWindow_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow();
            ordersWindow.Show();
        }

        private void OpenServicesWindow_Click(object sender, RoutedEventArgs e)
        {
            ServicesWindow servicesWindow = new ServicesWindow();
            servicesWindow.Show();
        }
        // Важно: не закрывать главное окно
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Убедитесь, что не вызывается this.Close()
        }

        private void LoadData()
        {
            Nadezhda.ItemsSource = services;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM public.services";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new Service
                            {
                                ID = reader.GetInt32(0),
                                ServiceName = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetInt32(3)
                            });
                        }
                    }
                }

                Nadezhda.ItemsSource = services; // Привязываем все данные к DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
            }

        }
        

        // Метод для фильтрации
        private void FilterData()
        {
            string filterText = FilterTextBox.Text.ToLower(); 
            string selectedTag = (FilterComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString(); 

            var filteredServices = services;

            if (!string.IsNullOrEmpty(filterText) && !string.IsNullOrEmpty(selectedTag))
            {
                switch (selectedTag)
                {
                    case "ID":
                        if (int.TryParse(filterText, out int idFilter))
                        {
                            filteredServices = services.Where(s => s.ID == idFilter).ToList();
                        }
                        break;

                    case "ServiceName":
                        filteredServices = services.Where(s => s.ServiceName.ToLower().Contains(filterText)).ToList();
                        break;

                    case "Price":
                        if (int.TryParse(filterText, out int priceFilter))
                        {
                            filteredServices = services.Where(s => s.Price == priceFilter).ToList();
                        }
                        break;
                }
            }
            Nadezhda.ItemsSource = filteredServices;
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterData(); // Применяем фильтрацию
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterData(); // Применяем фильтрацию
        }

        public void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterComboBox.SelectedIndex = -1; // Сбрасываем выбор в ComboBox
            FilterTextBox.Text = string.Empty; // Очищаем TextBox
            Nadezhda.ItemsSource = services; // Отображаем все записи
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос на добавление новой услуги
                    string query = "INSERT INTO public.Services (service_name, description, price) VALUES (@service_name, @description, @price)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("service_name", ServiceName.Text);
                        cmd.Parameters.AddWithValue("description", Description.Text);
                        cmd.Parameters.AddWithValue("price", int.Parse(Price.Text));

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Услуга добавлена успешно!");
                    LoadData(); // обновляем таблицу
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении услуги: " + ex.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Nadezhda.SelectedItem is Service selectedService)
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // SQL-запрос на удаление услуги по ID
                        string query = "DELETE FROM public.Services WHERE service_id = @service_id";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@service_id", selectedService.ID);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Услуга удалена успешно!");
                                LoadData(); // обновляем таблицу
                            }
                        }

                        MessageBox.Show("Услуга удалена успешно!");
                        LoadData(); // обновляем таблицу
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении услуги: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите услугу для удаления.");
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Nadezhda.SelectedItem is Service selectedService)
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // SQL-запрос на обновление данных услуги по ID
                        string query = "UPDATE public.Services SET service_name = @service_name, description = @description, price = @price WHERE service_id = @service_id";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@service_id", selectedService.ID);
                            cmd.Parameters.AddWithValue("service_name", ServiceName.Text);
                            cmd.Parameters.AddWithValue("description", Description.Text);
                            cmd.Parameters.AddWithValue("price", int.Parse(Price.Text));

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Услуга обновлена успешно!");
                        LoadData(); // обновляем таблицу
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении услуги: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите услугу для обновления.");
            }

        }

        public class PasswordHelper
        {
            // Метод для генерации соли
            public static byte[] GenerateSalt()
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] salt = new byte[16];
                    rng.GetBytes(salt);
                    return salt;
                }
            }
        }

            // Метод для хеширования пароля с солью
            public static string HashPassword(string password, byte[] salt)
            {
                using (var sha256 = SHA256.Create())
                {
                    // Конкатенируем пароль и соль
                    var saltedPassword = Encoding.UTF8.GetBytes(password);
                    var saltedPasswordWithSalt = new byte[saltedPassword.Length + salt.Length];
                    Array.Copy(saltedPassword, saltedPasswordWithSalt, saltedPassword.Length);
                    Array.Copy(salt, 0, saltedPasswordWithSalt, saltedPassword.Length, salt.Length);

                    // Вычисляем хеш
                    var hash = sha256.ComputeHash(saltedPasswordWithSalt);

                    // Конвертируем хеш в строку
                    return Convert.ToBase64String(hash);
                }
            }
        public void ExportToCsv(List<Service> services)
        {
            using (var writer = new StreamWriter("products.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(services);
            }
        }

        private void ExportToExcel()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Сохранить как Excel"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                dataService.ExportToExcel(saveFileDialog.FileName, services);
                MessageBox.Show("Экспорт в Excel выполнен успешно!");
            }
        }

        private void ImportFromExcel()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Открыть Excel файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                services = dataService.ImportFromExcel(openFileDialog.FileName);
                MessageBox.Show("Импорт из Excel выполнен успешно!");
            }
        }

        private void ExportToCsv()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Сохранить как CSV"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                dataService.ExportToCsv(saveFileDialog.FileName, services);
                MessageBox.Show("Экспорт в CSV выполнен успешно!");
            }
        }

        private void ImportFromCsv()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Открыть CSV файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                services = dataService.ImportFromCsv(openFileDialog.FileName);
                MessageBox.Show("Импорт из CSV выполнен успешно!");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();
                switch (selectedText)
                {
                    case "Экспорт в Excel":
                        ExportToExcel();
                        break;
                    case "Импорт из Excel":
                        ImportFromExcel();
                        break;
                    case "Экспорт в CSV":
                        ExportToCsv();
                        break;
                    case "Импорт из CSV":
                        ImportFromCsv();
                        break;
                }
            }

        }

        private void OpenImageWindow_Click(object sender, RoutedEventArgs e)
        {
            ImageWindow imageWindow = new ImageWindow();
            imageWindow.ShowDialog();
        }

        private void OpenReportsWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.ShowDialog();
        }
    }
}  