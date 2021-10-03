using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using VibeQuest.Service;
using VibeQuest.Utility.Extensions;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VibeQuest.Api.Extentions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VibeQuest.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = Constants.AppLocalization.JwtBearerDescription,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                        {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                        }
                       },
                       new string[] { }
                     }
                    });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeQuest.Api V1");
            });

            return app;
        }

        public static IServiceCollection ConfigurePermissionOptions(this IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                var definitionProviders = new List<Type>();
                definitionProviders.Add(typeof(IPermissionDefinitionProvider));

                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });

            return services;
        }

        public static async Task SetDefaultPermissions(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var permissionManagement = serviceScope.ServiceProvider.GetService<IPermissionService>();
                await permissionManagement.AddUpdatePermissions();
            }
        }
    }
}
