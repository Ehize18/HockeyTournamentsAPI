namespace HockeyTournamentsAPI.Application.Contracts.Tours
{
    public record TourRequest(int TourCount, int MembersInTeams, Guid RefereeId);
}
