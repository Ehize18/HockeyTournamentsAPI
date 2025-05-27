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
using HockeyTournamentsAPI.BackgroudWorkers;

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
        /// <param name="config">Конфигурация.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddPostgreSQLDb(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["POSTGRES_CONNECTION_STRING"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = config.GetConnectionString("PostgreSQL")!;
            }

            services.AddDbContext<HockeyTournamentsDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            }, ServiceLifetime.Transient);
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
            services.AddScoped<ITourService, TourServiceDeep>();
            services.AddScoped<IMatchService, MatchServiceV2>();
            services.AddScoped<MatchMaker>();
            services.AddTransient<IRatingService, RatingService>();

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

        public static IServiceCollection AddBackGroundServices(this IServiceCollection services)
        {
            services.AddHostedService<TourWorker>();
            services.AddHostedService<TournamentWorker>();

            return services;
        }
    }
}
