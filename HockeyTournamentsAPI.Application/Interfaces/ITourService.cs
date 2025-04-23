using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface ITourService
    {
        List<Tour> CreateTours(Tournament tournament, int toursCount, int teamMemberCount);
    }
}