namespace HockeyTournamentsAPI.Application.Contracts.Teams
{
    public record TeamGoalsRequest(Guid TeamId, int Goals);
}
