namespace HockeyTournamentsAPI.Application.Contracts.Members
{
    public record MemberResponse(Guid MemberId, Guid UserId,
        string LastName, string FirstName, string? MiddleName,
        int RatingOnTournament);
}
