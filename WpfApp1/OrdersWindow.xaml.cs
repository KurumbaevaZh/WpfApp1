using Npgsql;
using System;
using System.Collections.Generic;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        static string connectionString = "Host=localhost; Port=5432; Username=postgres; Password=postgres; Database=nadezhdaa";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        private NadezhdaaDbContext bd;
        public OrdersWindow()
        {
            InitializeComponent();
            bd = new NadezhdaaDbContext();
            LoadOrders();
            LoadClients();
            LoadData();
        }
        public class Client
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string Name { get; set; } // добавьте это, если его нет
                                                 // другие свойства
        }
        public class Order
        {
            public int Id { get; set; }
            public DateTime OrderDate { get; set; } // Это должно быть определено
            public int ClientId { get; set; }
            public Client Client { get; set; }
            // другие свойства
        }


        private void LoadData()
        {
           
        }
        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is Order selectedOrder && sender is ComboBox cb)
            {
                selectedOrder.ClientId = (int)cb.SelectedValue;
                bd.SaveChanges();
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        

        private void LoadOrders()
        {
            
        }


        private void LoadClients()
        {
          
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления заказа
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления заказа
        }
    }
}
