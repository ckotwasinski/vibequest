using VibeQuest.Utility.CORS;
using VibeQuest.Utility.DependencyInjection;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace VibeQuest.Utility.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            if (services == null) 
                throw new ArgumentNullException(nameof(services));

            services
                .ConfigureCors(configuration)
                .ConfigureIIS()
                .ConfigureJWT(configuration)
                .RegisterAllDependencies();

            return services;
        }

        public static IServiceCollection ConfigureIIS(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsConfiguration.CorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration[CorsConfiguration.CorsSectionName]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtTokenConfig = configuration.GetSection(JwtConfigurations.JwtSectionName).Get<JwtTokenConfig>();
            if (jwtTokenConfig.RefreshTokenExpiration == default(int))
                jwtTokenConfig.RefreshTokenExpiration = JwtConfigurations.DefaultRefreshTokenExpiration;
            if (jwtTokenConfig.AccessTokenExpiration == default(int))
                jwtTokenConfig.AccessTokenExpiration = JwtConfigurations.DefaultAccessTokenExpiration;

            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtTokenConfig.Secret.GetBytes()),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            services.AddHostedService<JwtRefreshTokenCache>();

            return services;
        }

        public static IServiceCollection RegisterAllDependencies(this IServiceCollection services)
        {
            return services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();
        }
    }
}
