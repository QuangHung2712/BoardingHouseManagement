using QLNhaTro.Service.LandlordService;
using Quartz;

namespace QLNhaTro.API.Extensions.Job
{
    public class JobSendMonthlyReports : IJob
    {
        private readonly ILandlordService _landlordService;

        public JobSendMonthlyReports(ILandlordService landlordService)
        {
            _landlordService = landlordService;
        }

        async Task IJob.Execute(IJobExecutionContext context)
        {
            // Code thực thi hàm MonthlyReport
            await _landlordService.MonthlyReport();
        }
    }
}
