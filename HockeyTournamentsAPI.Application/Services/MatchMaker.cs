using HockeyTournamentsAPI.Core.Models;
using System.Numerics;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Database.PostgreSQL.Repositories;

namespace HockeyTournamentsAPI.Application.Services
{
    public class MatchMaker
    {
        private readonly IToursRepository _toursRepository;
        private readonly IMatchesRepository _matchesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly ITournamentsRepository _tournamentsRepository;

        private Random _random = new Random();

        public MatchMaker(IToursRepository toursRepository,
            IMatchesRepository matchesRepository,
            ITeamsRepository teamsRepository,
            ITournamentsRepository tournamentsRepository)
        {
            _toursRepository = toursRepository;
            _matchesRepository = matchesRepository;
            _teamsRepository = teamsRepository;
            _tournamentsRepository = tournamentsRepository;
        }

        public List<Match> CreateRoundMatches(List<TournamentParticipant> allPlayers, int playersPerTeam, DateTime firstMatchStartTime)
        {
            var matches = new List<Match>();
            var availablePlayers = new List<TournamentParticipant>(allPlayers);
            var usedPlayers = new List<TournamentParticipant>();

            // Перемешиваем игроков для случайного выбора
            Shuffle(availablePlayers);

            while (availablePlayers.Count > 0)
            {
                var startTime = firstMatchStartTime;

                var lastMatch = matches.LastOrDefault();

                if (lastMatch is not null)
                {
                    startTime = lastMatch.EndTime.AddSeconds(30);
                }

                var matchPlayers = new List<TournamentParticipant>();

                // Берем случайных игроков для матча
                int newPlayersToTake = Math.Min(playersPerTeam * 2, availablePlayers.Count);
                var randomPlayers = GetRandomPlayers(availablePlayers, newPlayersToTake);
                matchPlayers.AddRange(randomPlayers);

                // Удаляем выбранных игроков из доступных
                availablePlayers.RemoveAll(p => randomPlayers.Contains(p));

                // Если не хватает игроков, добираем случайных из уже игравших
                if (matchPlayers.Count < playersPerTeam * 2)
                {
                    int needed = playersPerTeam * 2 - matchPlayers.Count;
                    var additionalPlayers = usedPlayers
                        .Where(p => !matchPlayers.Contains(p))
                        .OrderBy(x => _random.Next())
                        .Take(needed)
                        .ToList();

                    matchPlayers.AddRange(additionalPlayers);
                }

                // Создаем сбалансированный матч (сортировка по рейтингу перед распределением)
                var match = CreateBalancedMatch(matchPlayers, playersPerTeam);
                match.StartTime = startTime;
                match.EndTime = startTime.AddMinutes(1);
                matches.Add(match);

                // Запоминаем использованных игроков (кроме дополнительных)
                usedPlayers.AddRange(randomPlayers);
            }
            return matches;
        }

        private List<TournamentParticipant> GetRandomPlayers(List<TournamentParticipant> players, int count)
        {
            return players.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        private void Shuffle(List<TournamentParticipant> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                TournamentParticipant value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private Match CreateBalancedMatch(List<TournamentParticipant> players, int playersPerTeam)
        {
            // Сортируем игроков по рейтингу перед распределением по командам
            var sortedPlayers = players.OrderBy(p => p.RatingOnTournament).ToList();
            var team1Members = new List<TeamMember>();
            var team2Members = new List<TeamMember>();

            for (int i = 0; i < playersPerTeam; i++)
            {
                team1Members.Add(new TeamMember { Participant = sortedPlayers[i * 2] });
                team2Members.Add(new TeamMember { Participant = sortedPlayers[i * 2 + 1] });
            }

            return new Match
            {
                Teams = new List<Team>
            {
                new Team { Members = team1Members },
                new Team { Members = team2Members }
            }
            };
        }
    }
}
