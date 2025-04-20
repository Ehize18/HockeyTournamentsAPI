namespace HockeyTournamentsAPI.Application.Contracts.Tournaments
{
    public record TournamentRequest(string Title, string? Description, DateTime StartTime);
}
