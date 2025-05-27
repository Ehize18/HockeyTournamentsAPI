using HockeyTournamentsAPI.Application.Contracts.Matches;
using HockeyTournamentsAPI.Application.Contracts.Teams;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class MatchMapper
    {
        public static MatchResponse ToResponse(this Match match)
        {
            var teamsResponses = new List<TeamResponse>();

            foreach (var team in match.Teams)
            {
                teamsResponses.Add(team.ToResponse());
            }

            return new MatchResponse(match.Id, teamsResponses, match.StartTime, match.EndTime);
        }
    }
}
