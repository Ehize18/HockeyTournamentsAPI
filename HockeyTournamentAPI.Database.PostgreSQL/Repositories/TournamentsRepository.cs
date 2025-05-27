using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class TournamentsRepository : BaseRepository<Tournament>, ITournamentsRepository
    {
        public TournamentsRepository(HockeyTournamentsDbContext context)
            : base(context) { }

        public async Task<Tournament?> GetTournamentWithParticipants(Guid tournamentId)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .ThenInclude(p => p.User)
                .Include(t => t.Tours)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            return tournament;
        }

        public async Task<List<Tournament>> GetTournamentsStartInDates(DateTime from, DateTime to)
        {
            var tournaments = await _context.Tournaments
                .Include(t => t.Participants)
                .ThenInclude(p => p.User)
                .Include(t => t.Tours)
                .Where(t => t.StartTime > from && t.StartTime <= to)
                .ToListAsync();

            return tournaments;
        }
    }
}
