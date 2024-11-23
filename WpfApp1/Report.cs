using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Report
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }
}
