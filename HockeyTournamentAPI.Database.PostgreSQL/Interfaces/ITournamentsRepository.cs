using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface ITournamentsRepository : IBaseRepository<Tournament>
    {
        Task<Tournament?> GetTournamentWithParticipants(Guid tournamentId);
        Task<List<Tournament>> GetTournamentsStartInDates(DateTime from, DateTime to);
    }
}
