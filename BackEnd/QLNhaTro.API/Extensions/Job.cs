using QLNhaTro.Service.BillService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace QLNhaTro.API.Extensions
{
    public class Job : IJob
    {
        private readonly IBillService _billService;

        public Job(IBillService billService)
        {
            _billService = billService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Code thực thi hàm SubmitRequesInformation
            await _billService.SubmitRequesInformation();
        }

    }
}
