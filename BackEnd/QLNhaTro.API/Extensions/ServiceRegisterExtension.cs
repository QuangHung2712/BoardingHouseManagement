using QLNhaTro.Service;
using QLNhaTro.Service.EmailService;

namespace QLNhaTro.API.Extensions
{
    public static class ServiceRegisterExtension
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
