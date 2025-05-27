using HockeyTournamentsAPI.Application.Contracts.Teams;

namespace HockeyTournamentsAPI.Application.Contracts.Matches
{
    public record MatchResultRequest(TeamGoalsRequest LeftTeam, TeamGoalsRequest RightTeam);
}
