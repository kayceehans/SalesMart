using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.SignalR
{
    public class MessageHub: Hub<IMessageHubClient>
    {
        public async Task SendSalesUpdate(string message)
        {
            await Clients.All.SendSalesUpdate(message);
        }
    }
}
