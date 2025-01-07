using QLNhaTro.API.Extensions.Job;
using QLNhaTro.Service;
using QLNhaTro.Service.BillService;
using QLNhaTro.Service.ContractService;
using QLNhaTro.Service.CustomerService;
using QLNhaTro.Service.EmailService;
using QLNhaTro.Service.IncurService;
using QLNhaTro.Service.LandlordService;
using QLNhaTro.Service.RoomService;
using QLNhaTro.Service.Service;
using QLNhaTro.Service.TowerService;
using Quartz;

namespace QLNhaTro.API.Extensions
{
    public static class ServiceRegisterExtension
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IService,ServiceRepo>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IIncurService, IncurService>();
            services.AddScoped<ILandlordService, LandlordService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ITowerService,TowerService>();
            services.AddMemoryCache();
            // Cấu hình dịch vụ Hosted Quartz (Quartz sẽ chạy job ngay cả khi không có người truy cập)
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = JobKey.Create(nameof(JobInvoiceRequest));
                q.AddJob<JobInvoiceRequest>(jobKey)
                .AddTrigger(trigger => trigger
                                        .ForJob(jobKey)
                                        .WithCronSchedule("0 53 16 * * ?"));
                var SendMonthlyReports = JobKey.Create(nameof(JobSendMonthlyReports));
                q.AddJob<JobSendMonthlyReports>(SendMonthlyReports)
                .AddTrigger(trigger => trigger
                                       .ForJob(SendMonthlyReports)
                                       .WithCronSchedule("0 5 17 * * ?"));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
