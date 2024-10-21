using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.DataTransferObject
{
    public class DashboardItemDto
    {       
        public string MostSoldProduct { get; set; }
        public string MostExpensive { get; set; } 
    }
}
