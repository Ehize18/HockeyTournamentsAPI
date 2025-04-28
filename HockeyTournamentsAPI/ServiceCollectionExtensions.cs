using System.Text;
using HockeyTournamentsAPI.Database.PostgreSQL;
using HockeyTournamentsAPI.Infrastructure.Jwt;
using HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Services;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Database.PostgreSQL.Repositories;

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
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["HockeyToken"];
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddDbRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ITournamentsRepository, TournamentsRepository>();
            services.AddScoped<ITournamentParticipantsRepository, TournamentParticipantsRepository>();
            services.AddScoped<IToursRepository, ToursRepository>();
            services.AddScoped<IMatchesRepository, MatchesRepository>();
            services.AddScoped<ITeamsRepository, TeamsRepository>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITourService, TourService>();
            services.AddScoped<IMatchService, MatchService>();

            return services;
        }

        public static IServiceCollection AddApplicationCors(this IServiceCollection services)
        {
            services.AddCors(
                o => o.AddPolicy("DevPolicy", 
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));
            return services;
        }
    }
}
