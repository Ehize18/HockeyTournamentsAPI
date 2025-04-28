using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> GetMatchById(Guid matchId);
        Task<List<Match>> GetMatchesByTourId(Guid tourId);
    }
}