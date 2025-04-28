using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IToursRepository : IBaseRepository<Tour>
    {
        Task<List<Tour>> GetByTorunamentIdWithParticipants(Guid tournamentId);
        Task<Tour?> GetByIdWithParticipants(Guid id);
    }
}
