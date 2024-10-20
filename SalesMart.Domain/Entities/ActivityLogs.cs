using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.Entities
{
    public class ActivityLogs
    {
        public int Id { get; set; }
        public string Activity { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}
