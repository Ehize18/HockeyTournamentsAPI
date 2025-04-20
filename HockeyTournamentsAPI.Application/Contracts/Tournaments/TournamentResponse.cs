namespace HockeyTournamentsAPI.Application.Contracts.Tournaments
{
    public record TournamentResponse(
        Guid Id, string Title, string? Description,
        DateTime StartTime, DateTime? EndTime);
}
