using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp1.MainWindow;

namespace WpfApp1
{
    public class DataService
    {
        public void ExportToCsv(string filePath, List<Service> services)
        {
            using (var writer = new StreamWriter(filePath))
            {
                // Запись заголовков
                writer.WriteLine("ID,ServiceName,Description,Price");

                // Запись данных
                foreach (var service in services)
                {
                    writer.WriteLine($"{service.ID},{service.ServiceName},{service.Description},{service.Price}");
                }
            }
        }

        // Импорт данных из CSV
        public List<Service> ImportFromCsv(string filePath)
        {
            var importedServices = new List<Service>();

            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Пропуск строки заголовков
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var service = new Service
                    {
                        ID = int.Parse(values[0]),
                        ServiceName = values[1],
                        Description = values[2],
                        Price = int.Parse(values[3])
                    };

                    importedServices.Add(service);
                }
            }

            return importedServices;
        }

        // Экспорт данных в Excel
        public void ExportToExcel(string filePath, List<Service> services)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Services");

                // Запись заголовков
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "ServiceName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Price";

                // Запись данных
                for (int i = 0; i < services.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = services[i].ID;
                    worksheet.Cell(i + 2, 2).Value = services[i].ServiceName;
                    worksheet.Cell(i + 2, 3).Value = services[i].Description;
                    worksheet.Cell(i + 2, 4).Value = services[i].Price;
                }

                workbook.SaveAs(filePath);
            }
        }

        // Импорт данных из Excel
        public List<Service> ImportFromExcel(string filePath)
        {
            var importedServices = new List<Service>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet("Services");
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Пропуск заголовка
                {
                    var service = new Service
                    {
                        ID = (int)row.Cell(1).GetValue<int>(),
                        ServiceName = row.Cell(2).GetValue<string>(),
                        Description = row.Cell(3).GetValue<string>(),
                        Price = (int)row.Cell(4).GetValue<int>()
                    };

                    importedServices.Add(service);
                }
            }

            return importedServices;
        }
    }
}
