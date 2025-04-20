using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface ITournamentService
    {
        Task<Tournament> CreateAsync(TournamentRequest request);
        Task<IList<Tournament>> GetAllAsync();
        Task<Tournament?> GetById(Guid id);
    }
}