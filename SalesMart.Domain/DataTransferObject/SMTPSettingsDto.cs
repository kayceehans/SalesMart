using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.DataTransferObject
{
    public class SMTPSettingsDto
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromMailAddress { get; set; }
        public string Password { get; set; }
    }
}
