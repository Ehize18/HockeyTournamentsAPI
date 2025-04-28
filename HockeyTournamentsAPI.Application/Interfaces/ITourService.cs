using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface ITourService
    {
        Task<List<Tour>> CreateTours(Tournament tournament, User referee, int toursCount, int teamMemberCount);
        Task<List<Tour>> GetToursByTournamentId(Guid tournamentId);
        Task<Tour?> GetTourById(Guid tourId);
    }
}