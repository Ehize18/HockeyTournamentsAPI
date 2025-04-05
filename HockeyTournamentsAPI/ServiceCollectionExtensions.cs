using System.Text;
using HockeyTournamentsAPI.Database.PostgreSQL;
using HockeyTournamentsAPI.Infrastructure.Jwt;
using HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HockeyTournamentsAPI
{
    /// <summary>
    /// Методы рассширения для IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет PostgreSQL базу данных.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="connectionString">Строка подключения.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddPostgreSQLDb(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HockeyTournamentsDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration jwtConfig)
        {
            var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("SecretKey")!));

            services.Configure<JwtOptions>(jwtConfig);
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey
                    };
                });

            return services;
        }
    }
}
