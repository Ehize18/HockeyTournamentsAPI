using HockeyTournamentsAPI.Application.Contracts.Matches;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class MatchServiceV2 : IMatchService
    {
        private readonly IMatchesRepository _matchesRepository;
        private readonly ITeamsRepository _teamsRepository;

        public MatchServiceV2(IMatchesRepository matchesRepository, ITeamsRepository teamsRepository)
        {
            _matchesRepository = matchesRepository;
            _teamsRepository = teamsRepository;
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

        public async Task<Match> SetMatchResults(Match match, MatchResultRequest request)
        {
            var leftTeam = match.Teams.FirstOrDefault(t => t.Id == request.LeftTeam.TeamId)!;
            leftTeam.Goals = request.LeftTeam.Goals;

            var rightTeam = match.Teams.FirstOrDefault(t => t.Id == request.RightTeam.TeamId)!;
            rightTeam.Goals = request.RightTeam.Goals;

            await _teamsRepository.UpdateRangeAsync(match.Teams);

            return match;
        }
    }
}
