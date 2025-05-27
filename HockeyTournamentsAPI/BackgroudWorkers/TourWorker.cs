
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.BackgroudWorkers
{
    public class TourWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private const int MIN_IN_MS = 60000;

        public TourWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastCheckedTimeFrom = DateTime.UtcNow.AddMinutes(-1);

            while (!stoppingToken.IsCancellationRequested)
            {
                var lastCheckedTimeTo = DateTime.UtcNow;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var matchesRepository = scope.ServiceProvider.GetRequiredService<IMatchesRepository>();

                        var matches = await matchesRepository.GetMatchesEndsInTimes(lastCheckedTimeFrom, lastCheckedTimeTo);

                        if (matches.Count > 0)
                        {
                            var ratingService = scope.ServiceProvider.GetRequiredService<IRatingService>();
                            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();

                            var tourRepository = scope.ServiceProvider.GetRequiredService<IToursRepository>();
                            var tournamentRepository = scope.ServiceProvider.GetRequiredService<ITournamentsRepository>();

                            foreach (var match in matches)
                            {
                                var tourMatches = await matchesRepository.GetMatchesByTourIdWithTeams(match.TourId);

                                await ratingService.RecalculateRating(tourMatches);

                                var tour = await tourRepository.GetByIdAsync(match.TourId);
                                var tournament = await tournamentRepository.GetTournamentWithParticipants(tour!.TournamentId);

                                await tourService.CreateTours(tournament!, match.Referee, 1, tourMatches[0].Teams[0].Members.Count);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                lastCheckedTimeFrom = lastCheckedTimeTo;

                await Task.Delay(MIN_IN_MS);
            }
        }
    }
}
