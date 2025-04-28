using HockeyTournamentsAPI.Application.Contracts.Teams;

namespace HockeyTournamentsAPI.Application.Contracts.Matches
{
    public record MatchResponse(Guid id, List<TeamResponse> Teams);
}
