using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IRolesRepository : IBaseRepository<Role>
    {
        Task<Role> GetRoleByName(string name);
    }
}
