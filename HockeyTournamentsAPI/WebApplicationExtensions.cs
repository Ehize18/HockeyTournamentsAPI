using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> CheckDefaultRoles(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            var rolesService = scope.ServiceProvider.GetRequiredService<IRolesService>();

            var defaultRoles = webApplication.Configuration.GetSection("DefaultRoles").Get<List<Dictionary<string, string>>>();

            var adminRole = await rolesService.GetRoleByNameAsync("Администратор");
            if (adminRole == null)
            {
                await rolesService.CreateRoleAsync(new()
                {
                    Name = "Администратор",
                    Permissions = (RolePermissions)Enum.GetValues<RolePermissions>().Cast<int>().Sum()
                });
            }
            var userRole = await rolesService.GetRoleByNameAsync("Пользователь");
            if (userRole == null)
            {
                await rolesService.CreateRoleAsync(new()
                {
                    Name = "Пользователь",
                    Permissions = 0
                });
            }

            return webApplication;
        }
    }
}
