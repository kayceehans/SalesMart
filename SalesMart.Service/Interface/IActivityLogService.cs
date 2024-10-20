using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Interface
{
    public interface IActivityLogService
    {
        Task<Result<List<ActivityLogs>>> GetActivityLogs();
        Task AddActivityLog(ActivityLogs logs);
        
    }
}
