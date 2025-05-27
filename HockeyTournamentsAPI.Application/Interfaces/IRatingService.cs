using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IRatingService
    {
        Task RecalculateRating(List<Match> matches);
    }
}