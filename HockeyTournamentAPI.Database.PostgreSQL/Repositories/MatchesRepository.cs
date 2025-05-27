using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class MatchesRepository : BaseRepository<Match>, IMatchesRepository
    {
        public MatchesRepository(HockeyTournamentsDbContext context) : base(context)
        {
        }

        public async Task<Match?> GetMatchByIdWithTeams(Guid matchId)
        {
            var match = await _context.Matches
                .Include(m => m.Teams)
                .ThenInclude(t => t.Members)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            return match;
        }

        public async Task<List<Match>> GetMatchesByTourIdWithTeams(Guid tourId)
        {
            var matches = await _context.Matches
                .Include(m => m.Teams)
                .ThenInclude(t => t.Members)
                .ThenInclude(m => m.Participant)
                .Include(m => m.Referee)
                .Where(m => m.TourId == tourId)
                .ToListAsync();

            return matches;
        }

        public async Task<List<Match>> GetMatchesEndsInTimes(DateTime from, DateTime to)
        {
            var matches = await _context.Matches
                .Where(m => m.EndTime > from && m.EndTime <= to && m.IsLastMatchInTour)
                .ToListAsync();

            return matches;
        }
    }
}
