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
    /// Логика взаимодействия для OrderItemsWindow.xaml
    /// </summary>
    public partial class OrderItemsWindow : Window
    {
        private NadezhdaaDbContext bd;
        public OrderItemsWindow()
        {
            InitializeComponent();
            LoadOrderItems();
            LoadServices();
        }
        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int ServiceId { get; set; }
            public string Name { get; set; }

            // Добавьте свойство Quantity, если оно отсутствует
            public int Quantity { get; set; }

        }
        private void LoadOrderItems()
        {
            
        }

        private void LoadServices()
        {
        }

        private void AddOrderItem_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления элемента заказа
        }

        private void DeleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления элемента заказа
        }
    }
}
