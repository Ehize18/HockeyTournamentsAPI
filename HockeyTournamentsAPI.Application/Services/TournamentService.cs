using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentsRepository _tournamentsRepository;
        private readonly ITournamentParticipantsRepository _participantsRepository;
        private readonly IUsersRepository _usersRepository;

        public TournamentService(ITournamentsRepository tournamentsRepository,
            ITournamentParticipantsRepository participantsRepository,
            IUsersRepository usersRepository)
        {
            _tournamentsRepository = tournamentsRepository;
            _participantsRepository = participantsRepository;
            _usersRepository = usersRepository;
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

        public async Task<TournamentParticipant?> AddParticipantAsync(Tournament tournament, User user)
        {
            var participant = new TournamentParticipant()
            {
                User = user,
                Tournament = tournament,
                IsAccepted = true
            };

            var entity = await _participantsRepository.CreateAsync(participant);

            return entity;
        }

        public async Task<TournamentParticipant?> GetParticipantById(Guid participantId)
        {
            var participant = await _participantsRepository.GetWithUserAsync(participantId);

            return participant;
        }

        public async Task<List<TournamentParticipant>> GetParticipantsAsync(Guid tournamentId)
        {
            var participants = await _participantsRepository
                .GetTournamentParticipantsAsync(tournamentId);

            return participants;
        }

        public async Task<TournamentParticipant> ChangeParticipantStatus(TournamentParticipant participant, bool isAccepted)
        {
            participant.IsAccepted = isAccepted;

            await _participantsRepository.SaveChangesAsync();

            return participant;
        }

        public async Task<Tournament?> GetTournamentWithParticipants(Guid tournamentId)
        {
            return await _tournamentsRepository.GetTournamentWithParticipants(tournamentId);
        }
    }
}
