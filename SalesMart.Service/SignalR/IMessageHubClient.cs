using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.SignalR
{
    public interface IMessageHubClient
    {
        Task SendSalesUpdate(string message);
    }
}
