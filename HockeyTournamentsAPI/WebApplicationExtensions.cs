using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Infrastructure.Hash;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> CheckDefaultUsers(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();

            var defaultUsers = webApplication.Configuration.GetSection("DefaultUsers").Get<List<Dictionary<string, string>>>();

            foreach (var user in defaultUsers)
            {
                if (user["Name"] == "Supervisor")
                {
                    var supervisor = await userRepository.GetSupervisorAsync();

                    if (supervisor == null)
                    {
                        await userRepository.CreateAsync(new User()
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Supervisor",
                            LastName = "Supervisor",
                            BirthDate = new DateOnly(2000, 1, 1),
                            IsMale = true,
                            Email = "changeme@supervisor.com",
                            Phone = "11111111111",
                            SportLevel = "",
                            Role = Role.Supervisor,
                            PasswordHash = Hash.SHA256Hash(user["Password"]),
                        });
                    }
                }
            }

            return webApplication;
        }

        public static WebApplication MigrateDb(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<HockeyTournamentsDbContext>();
            context.Database.Migrate();

            return webApplication;
        }

        public static WebApplication UseDevPolicy(this WebApplication webApplication)
        {
            webApplication.UseCors("DevPolicy");

            return webApplication;
        }
    }
}
