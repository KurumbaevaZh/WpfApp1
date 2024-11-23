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
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        private NadezhdaaDbContext bd;
        public ClientsWindow()
        {
            InitializeComponent();
            bd = new NadezhdaaDbContext();
            LoadClients();
        }
        private void LoadClients()
        {
            dgClients.ItemsSource = bd.Clients.ToList();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления клиента
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления клиента
        }

    }
}
