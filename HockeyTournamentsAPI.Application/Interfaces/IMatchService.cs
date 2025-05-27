using HockeyTournamentsAPI.Application.Contracts.Matches;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> GetMatchById(Guid matchId);
        Task<List<Match>> GetMatchesByTourId(Guid tourId);
        Task<Match> SetMatchResults(Match match, MatchResultRequest request);
    }
}