using System.Runtime.CompilerServices;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchesRepository _matchesRepository;

        public MatchService(IMatchesRepository matchesRepository)
        {
            _matchesRepository = matchesRepository;
        }

        public async Task<Match?> GetMatchById(Guid matchId)
        {
            var match = await _matchesRepository.GetMatchByIdWithTeams(matchId);

            return match;
        }

        public async Task<List<Match>> GetMatchesByTourId(Guid tourId)
        {
            var matches = await _matchesRepository.GetMatchesByTourIdWithTeams(tourId);

            return matches;
        }
    }
}
