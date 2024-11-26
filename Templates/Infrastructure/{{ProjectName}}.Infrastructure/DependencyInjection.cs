using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using __ProjectName__.Application.Common.Interfaces;
using __ProjectName__.Infrastructure.Services;

namespace __ProjectName__.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
