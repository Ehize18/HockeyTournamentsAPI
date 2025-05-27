using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class TourServiceDeep : ITourService
    {
        private readonly MatchMaker _matchMaker;
        private readonly IToursRepository _toursRepository;
        private readonly ITournamentsRepository _tournamentsRepository;

        public TourServiceDeep(MatchMaker matchMaker, IToursRepository toursRepository, ITournamentsRepository tournamentsRepository)
        {
            _matchMaker = matchMaker;
            _toursRepository = toursRepository;
            _tournamentsRepository = tournamentsRepository;
        }

        public async Task<List<Tour>> CreateTours(Tournament tournament, User referee, int toursCount, int teamMemberCount)
        {
            var tours = new List<Tour>();

            var tourParticipants = tournament.Participants.Where(p => !p.IsKicked && p.IsAccepted && p.ToursPlayed <= 16).ToList();

            var today = DateOnly.FromDateTime(DateTime.Today);

            tournament.Participants.ForEach(p =>
            {
                var months = today.Month - p.User.BirthDate.Month;
                var years = today.Year - p.User.BirthDate.Year;

                if (today.Day < p.User.BirthDate.Day)
                {
                    months--;
                }

                if (months < 0)
                {
                    years--;
                }

                p.Age = years;
            });

            for (int i = 0; i < toursCount; i++)
            {
                var lastTour = tournament.Tours.OrderByDescending(x => x.EndTime).FirstOrDefault();

                var startTime = tournament.StartTime;

                if (lastTour is not null)
                {
                    startTime = lastTour.EndTime.AddMinutes(1);
                }

                var matches = _matchMaker.CreateRoundMatches(tourParticipants, teamMemberCount, startTime);

                var lastMatch = matches.OrderByDescending(m => m.EndTime).FirstOrDefault();

                if (lastMatch is null)
                {
                    throw new Exception();
                }

                lastMatch.IsLastMatchInTour = true;

                foreach (var match in matches)
                {
                    match.Referee = referee;
                }

                var tour = new Tour
                {
                    StartTime = startTime,
                    Matches = matches,
                    TournamentId = tournament.Id,
                    EndTime = lastMatch.EndTime
                };

                //var savedTour = await _toursRepository.CreateAsync(tour);
                
                tour.Participants = tourParticipants;
                tournament.Tours.Add(tour);
                await _tournamentsRepository.UpdateAsync(tournament);
                tours.Add(tour);
            }

            return tours;
        }

        public async Task<Tour?> GetTourById(Guid tourId)
        {
            return await _toursRepository.GetByIdWithParticipants(tourId);
        }

        public async Task<List<Tour>> GetToursByTournamentId(Guid tournamentId)
        {
            return await _toursRepository.GetByTorunamentIdWithParticipants(tournamentId);
        }
    }
}
