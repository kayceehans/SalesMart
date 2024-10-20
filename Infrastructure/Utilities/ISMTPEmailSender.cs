using SalesMart.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Infrastructure.Utilities
{
    public interface ISMTPEmailSender
    {
        Task<Tuple<string, bool>> Email(EmailSenderDto request);
    }
}
