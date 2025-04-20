using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentsRepository _tournamentsRepository;

        public TournamentService(ITournamentsRepository tournamentsRepository)
        {
            _tournamentsRepository = tournamentsRepository;
        }

        public async Task<Tournament> CreateAsync(TournamentRequest request)
        {
            var tournament = new Tournament()
            {
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime
            };

            var entity = await _tournamentsRepository.CreateAsync(tournament);

            return entity;
        }

        public async Task<IList<Tournament>> GetAllAsync()
        {
            var tournaments = await _tournamentsRepository.GetAllAsync();

            return tournaments;
        }

        public async Task<Tournament?> GetById(Guid id)
        {
            var tournament = await _tournamentsRepository.GetByIdAsync(id);

            return tournament;
        }
    }
}
