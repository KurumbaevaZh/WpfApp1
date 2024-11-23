using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using static WpfApp1.models;



namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Reports.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        static string connectionString = "Host=localhost; Port=5432; Username=postgres; Password=postgres; Database=nadezhdaa";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        private NadezhdaaDbContext _context;

        public ReportsWindow()
        {
            InitializeComponent();
            _context = new NadezhdaaDbContext();
            LoadReportData();
        }
        public class YourEntity
        {
            public YourEntity() { } // Конструктор по умолчанию обязателен

            public YourEntity(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void LoadReportData(string sortBy = "client")
        {
            try
            {
               

                var query = from order in _context.Orders
                            join client in _context.Orders on order.ClientId equals client.ClientId
                            select new
                            {
                                OrderId = order.OrderId,         // ID заказа
                                ClientName = client.Name,        // Имя клиента
                                OrderDate = order.OrderDate,     // Дата заказа
                                Status = order.Status            // Статус заказа
                            };

              
                // Применяем сортировку
                switch (sortBy)
                {
                    case "client":
                        query = query.OrderBy(o => o.ClientName);
                        break;
                    case "order":
                        query = query.OrderBy(o => o.OrderId);
                        break;
                    case "date":
                        query = query.OrderBy(o => o.OrderDate);
                        break;
                }

                // Привязываем результат к DataGrid
                OrdersDataGrid.ItemsSource = query.ToList();

               
            }
            catch (Exception ex)
            {
               
            }
        }


        // Обработчик изменения выбора в ComboBox
        private void SortComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem selectedItem)
            {
                string sortBy = selectedItem.Tag.ToString(); // Получаем критерий сортировки
                LoadReportData(sortBy); // Загружаем данные с новой сортировкой
            }
        }

    }
}
