using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface ITournamentParticipantsRepository : IBaseRepository<TournamentParticipant>
    {
        Task<List<TournamentParticipant>> GetTournamentParticipantsAsync(Guid tournamentId);
        Task<TournamentParticipant?> GetWithUserAsync(Guid participantId);
    }
}
