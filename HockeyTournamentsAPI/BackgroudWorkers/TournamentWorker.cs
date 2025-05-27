
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.BackgroudWorkers
{
    public class TournamentWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private const int HOUR_IN_MS = 3600000;

        public TournamentWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastCheckedTimeFrom = DateTime.UtcNow.AddHours(24);

            var random = new Random();

            while (!stoppingToken.IsCancellationRequested)
            {
                var lastCheckedTimeTo = DateTime.UtcNow.AddHours(25);

                try
                {
                    using(var scope = _serviceProvider.CreateScope())
                    {
                        var tournamentRepository = scope.ServiceProvider.GetRequiredService<ITournamentsRepository>();

                        var tournamentsToBlock = await tournamentRepository.GetTournamentsStartInDates(lastCheckedTimeFrom, lastCheckedTimeTo);

                        if (tournamentsToBlock.Count > 0)
                        {
                            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();
                            var usersRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();

                            foreach (var tournament in tournamentsToBlock)
                            {
                                tournament.CanParticipate = false;

                                var referees = await usersRepository.GetReferees();

                                var referee = referees.OrderBy(x => random.Next()).FirstOrDefault();

                                await tourService.CreateTours(tournament, referee!, 1, 2);

                                await tournamentRepository.UpdateAsync(tournament);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                lastCheckedTimeFrom = lastCheckedTimeTo;

                await Task.Delay(HOUR_IN_MS);
            }
        }
    }
}
