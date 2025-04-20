using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class TournamentMapper
    {
        public static TournamentResponse ToResponse(this Tournament tournament)
        {
            var response = new TournamentResponse(
                tournament.Id,
                tournament.Title, tournament.Description,
                tournament.StartTime, tournament.EndTime);

            return response;
        }

        public static List<TournamentResponse> ToResponse(this IList<Tournament> tournaments)
        {
            var response = new List<TournamentResponse>();

            foreach (var tournament in tournaments)
            {
                response.Add(tournament.ToResponse());
            }

            return response;
        }
    }
}