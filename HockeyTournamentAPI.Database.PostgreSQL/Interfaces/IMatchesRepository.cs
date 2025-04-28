using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IMatchesRepository : IBaseRepository<Match>
    {
        Task<Match?> GetMatchByIdWithTeams(Guid matchId);
        Task<List<Match>> GetMatchesByTourIdWithTeams(Guid tourId);
    }
}
