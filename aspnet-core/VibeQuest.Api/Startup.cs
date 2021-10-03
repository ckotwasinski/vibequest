using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Api.Extentions;
using VibeQuest.Api.Handler;
using VibeQuest.Api.Middleware;
using VibeQuest.Dto.Validators;
using VibeQuest.Service.Extensions;
using VibeQuest.Utility.CORS;
using VibeQuest.Utility.Extensions;
using VibeQuest.Utility.Permissions.Core;

namespace VibeQuest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultServices(Configuration);
            //services.AddControllers();
            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UsersDtoValidator>());
            services.AddServices(Configuration);
            services.AddSwaggerDocumentation();
            services.ConfigurePermissionOptions();
            services.AddAuthorization();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddMemoryCache();
            services.AddHttpClient();

            //services.TryAddTransient<DefaultAuthorizationPolicyProvider>();
            //services.AddSingleton<IPermissionValueProvider, RolePermissionValueProvider>();
            //services.Configure<PermissionOptions>(options =>
            //{
            //    options.ValueProviders.Add<RolePermissionValueProvider>();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SetDefaultPermissions();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseCors(CorsConfiguration.CorsPolicyName);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerDocumentation();
        }
    }
}
