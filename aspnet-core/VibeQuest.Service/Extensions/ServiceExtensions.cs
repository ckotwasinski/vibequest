using VibeQuest.Service.MapperConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VibeQuest.Utility.DependencyInjection;
using VibeQuest.DataAccess.Extensions;

namespace VibeQuest.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton(AutoMapperConfiguration.InitializeAutoMapper().CreateMapper());
            services.AddProviderService(Configuration);
            return services;
        }
    }
}
