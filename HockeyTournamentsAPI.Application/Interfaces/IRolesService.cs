using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IRolesService
    {
        Task<Role?> GetRoleByNameAsync(string name);
        Task<Role> CreateRoleAsync(Role role);
    }
}