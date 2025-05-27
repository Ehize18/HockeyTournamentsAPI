namespace HockeyTournamentsAPI.Application.Contracts.Participants
{
    public record TournamentParticipantResponse(
        Guid Id,
        Guid UserId,
        string FirstName, string? MiddleName, string LastName,
        bool isAccepted,
        Guid TournamentId,
        string TournamentTitle,
        int RatingOnTournament);
}
