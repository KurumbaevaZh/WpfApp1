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
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Window
    {
        private NadezhdaaDbContext bd;
        public ServicesWindow()
        {
            InitializeComponent();
            bd = new NadezhdaaDbContext();
            LoadServices();
        }
        private void LoadServices()
        {
            dgServices.ItemsSource = bd.Services.ToList();
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления услуги
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления услуги
        }
    }
}
