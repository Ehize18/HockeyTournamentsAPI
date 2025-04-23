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
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            return tournament;
        }
    }
}
