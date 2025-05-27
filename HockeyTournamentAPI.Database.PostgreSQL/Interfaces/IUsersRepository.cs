using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetSupervisorAsync();
        Task<List<User>> GetUsersWithFiltrationAsync(DateOnly birthdayFrom, DateOnly birthdayTo, int page, int pageSize, bool? gender,
            string orderBy, bool isAscending);
        Task<List<User>> GetReferees();
    }
}
