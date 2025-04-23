using System.Runtime.CompilerServices;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class TournamentParticipantsRepository : BaseRepository<TournamentParticipant>, ITournamentParticipantsRepository
    {
        public TournamentParticipantsRepository(HockeyTournamentsDbContext context)
            : base(context) { }

        public async Task<List<TournamentParticipant>> GetTournamentParticipantsAsync(Guid tournamentId)
        {
            var participants = await _context.TournamentParticipants
                .Include(p => p.User)
                .Where(p => p.TournamentId == tournamentId)
                .ToListAsync();

            return participants;
        }

        public async Task<TournamentParticipant?> GetWithUserAsync(Guid participantId)
        {
            var participant = await _context.TournamentParticipants
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == participantId);

            return participant;
        }
    }
}
