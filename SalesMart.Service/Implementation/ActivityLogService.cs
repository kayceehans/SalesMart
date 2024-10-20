using SalesMart.Data.Repositories;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.Entities;
using SalesMart.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Implementation
{
    public class ActivityLogService: IActivityLogService
    {
        private readonly IGenericRepo<ActivityLogs> _activityLogsRepo;
        public ActivityLogService(IGenericRepo<ActivityLogs> activityLogsRepo)
        {
            _activityLogsRepo = activityLogsRepo;
        }
        public async Task<Result<List<ActivityLogs>>> GetActivityLogs()
        {
            try
            {
                var response = new Result<List<ActivityLogs>>();
                var logs = await _activityLogsRepo.GetAllAsync();
                if (logs != null)
                {
                    response.Content = logs;
                    response.IsSuccess = true;
                    response.Message = $"List of all available activitiies. Count:{logs.Count}";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get products";
                }
                return response;
            }
            catch (Exception ex)
            {
                var response = new Result<List<ActivityLogs>>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task AddActivityLog(ActivityLogs logs)
        {
            try
            {
                var response = new Result<ActivityLogs>();
                _activityLogsRepo.Add(logs);
                var isSaved = await _activityLogsRepo.SaveAsync();
                
            }
            catch (Exception)
            {
                
            }
        }
    }
}
