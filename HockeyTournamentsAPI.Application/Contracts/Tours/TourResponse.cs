namespace HockeyTournamentsAPI.Application.Contracts.Tours
{
    public record TourResponse(Guid Id, int MatchesCount, int MembersCount, DateTime StartTime, DateTime EndTime);
}
