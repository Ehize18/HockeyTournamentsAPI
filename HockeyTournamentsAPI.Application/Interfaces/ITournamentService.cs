using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface ITournamentService
    {
        Task<Tournament> CreateAsync(TournamentRequest request);
        Task<IList<Tournament>> GetAllAsync();
        Task<Tournament?> GetById(Guid id);
        Task<TournamentParticipant?> AddParticipantAsync(Tournament tournament, User user);
        Task<TournamentParticipant?> GetParticipantById(Guid participantId);
        Task<List<TournamentParticipant>> GetParticipantsAsync(Guid tournamentId);
        Task<TournamentParticipant> ChangeParticipantStatus(TournamentParticipant participant, bool status);
        Task<Tournament?> GetTournamentWithParticipants(Guid tournamentId);
    }
}