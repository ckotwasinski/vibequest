//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VibeQuest.Utility.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VibeQuest.DataAccess.Infrastructure;

namespace VibeQuest.DataAccess.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProviderService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<VibeQuestDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<IUnitOfWork<VibeQuestDbContext>, UnitOfWork<VibeQuestDbContext>>();

            return services;
        }
    }
}
