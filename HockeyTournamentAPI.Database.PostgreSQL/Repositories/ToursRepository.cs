using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class ToursRepository : BaseRepository<Tour>, IToursRepository
    {
        public ToursRepository(HockeyTournamentsDbContext context) : base(context)
        {
        }

        public async Task<List<Tour>> GetByTorunamentIdWithParticipants(Guid tournamentId)
        {
            var tours = await _context.Tours
                .Include(t => t.Matches)
                .ThenInclude(m => m.Teams)
                .ThenInclude(t => t.Members)
                .AsSplitQuery()
                .Where(t => t.TournamentId == tournamentId)
                .ToListAsync();

            foreach (var tour  in tours)
            {
                LoadParticipants(tour);
            }

            return tours;
        }

        public async Task<Tour?> GetByIdWithParticipants(Guid id)
        {
            var tour = await _context.Tours
                .Include(t => t.Matches)
                .ThenInclude(m => m.Teams)
                .ThenInclude(t => t.Members)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                return null;
            }

            LoadParticipants(tour);

            return tour;
        }

        private Tour LoadParticipants(Tour tour)
        {
            var participants = tour.Matches
                    .SelectMany(m => m.Teams)
                    .SelectMany(t => t.Members)
                    .DistinctBy(m => m.ParticipantId)
                    .Select(m => new TournamentParticipant() { Id = m.ParticipantId })
                    .ToList();

            tour.Participants = participants;

            return tour;
        }
    }
}
