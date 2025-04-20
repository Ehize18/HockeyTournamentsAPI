using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetSupervisorAsync();
    }
}
