using System.Runtime.CompilerServices;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class TourService : ITourService
    {
        private readonly IToursRepository _toursRepository;
        private readonly IMatchesRepository _matchesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly ITournamentsRepository _turnamentsRepository;

        public TourService(IToursRepository toursRepository,
            IMatchesRepository matchesRepository,
            ITeamsRepository teamsRepository,
            ITournamentsRepository tournamentsRepository)
        {
            _toursRepository = toursRepository;
            _matchesRepository = matchesRepository;
            _teamsRepository = teamsRepository;
            _turnamentsRepository = tournamentsRepository;
        }

        public async Task<List<Tour>> CreateTours(Tournament tournament, User referee, int toursCount, int teamMemberCount)
        {
            var tourParticipants = tournament.Participants.Where(p => !p.IsKicked && p.IsAccepted).ToList();

            if (tournament.Tours == null)
            {
                tournament.Tours = new List<Tour>();
            }

            for (var i = 0; i < toursCount; i++)
            {
                var tour = CreateTour(tournament, tourParticipants, teamMemberCount);

                if (tour == null)
                {
                    return new List<Tour>();
                }

                foreach (var match in tour.Matches)
                {
                    match.Referee = referee;
                }

                tournament.Tours.Add(tour);
                tournament = await _turnamentsRepository.UpdateAsync(tournament);
            }

            return tournament.Tours;
        }

        public async Task<List<Tour>> GetToursByTournamentId(Guid tournamentId)
        {
            var tours = await _toursRepository.GetByTorunamentIdWithParticipants(tournamentId);

            return tours;
        }

        public async Task<Tour?> GetTourById(Guid tourId)
        {
            var tour = await _toursRepository.GetByIdWithParticipants(tourId);

            return tour;
        }

        private void PrepareParticipantsForTour(List<TournamentParticipant> participants)
        {
            foreach (var participant in participants)
            {
                participant.CanPlay = true;
                participant.GamesInRow = 0;
            }
        }

        private List<TournamentParticipant> FindOpponents(List<TournamentParticipant> participants)
        {
            participants.Sort((p1, p2) => p1.RatingOnTournament - p2.RatingOnTournament);

            for (int i = 0; i < participants.Count; i++)
            {
                if (i == 0)
                {
                    participants[i].Opponent = participants[i + 1];
                    continue;
                }
                if (i == participants.Count - 1)
                {
                    participants[i].Opponent = participants[i - 1];
                    continue;
                }
                var previous = participants[i - 1];
                var next = participants[i + 1];

                if ((previous.RatingOnTournament - participants[i].RatingOnTournament) >
                    participants[i].RatingOnTournament - next.RatingOnTournament)
                {
                    participants[i].Opponent = next;
                }
                else
                {
                    participants[i].Opponent = previous;
                }
            }

            return participants;
        }

        private Tour? CreateTour(Tournament tournament, List<TournamentParticipant> participants, int teamMemberCount)
        {
            PrepareParticipantsForTour(participants);

            participants = FindOpponents(participants);

            var tour = new Tour();

            var startTime = tournament.StartTime;

            if (tournament.Tours.Count > 0)
            {
                startTime = tournament.Tours.Last().EndTime.AddMinutes(30);
            }

            tour.StartTime = startTime;

            tour.ParticipantsCount = participants.Count;

            tour.Participants = participants;

            tour = CreateMatches(tour, teamMemberCount);

            if (tour == null)
            {
                return null;
            }

            tour.EndTime = tour.Matches.Last().EndTime;

            return tour;
        }

        private Tour? CreateMatches(Tour tour, int teamMemberCount)
        {
            var participantsWithNotPlayed = LoadNotPlayed(tour.Participants);

            if (tour.Matches == null)
            {
                tour.Matches = new List<Match>();
            }

            while (participantsWithNotPlayed.Count > 0)
            {
                var pivotPlayer = GetPivotPlayer(tour.Participants);
                var match = CreateMatch(pivotPlayer, teamMemberCount, tour.Participants);

                if (match == null)
                {
                    return null;
                }

                var startTime = tour.StartTime;

                if (tour.Matches.Count > 0)
                {
                    startTime = tour.Matches.Last().EndTime.AddSeconds(30);
                }

                match.StartTime = startTime;
                match.EndTime = startTime.AddMinutes(1);

                tour.Matches.Add(match!);
                participantsWithNotPlayed = UpdateNotPlayed(participantsWithNotPlayed);

                if (tour.Matches.Count > 30)
                {
                    var test = 1;
                }

                UpdateGamesInRow(tour);
            }
            return tour;
        }

        private List<TournamentParticipant> UpdateNotPlayed(List<TournamentParticipant> participants)
        {
            var newList = new List<TournamentParticipant>();

            foreach (var participant in participants)
            {
                if (participant.NotPlayedParticipants.Count > 0)
                {
                    newList.Add(participant);
                }
            }
            return newList;
        }

        private void UpdateGamesInRow(Tour tour)
        {
            if (tour.Matches.Count < 2)
            {
                return;
            }
            var lastTwoMathces = tour.Matches.Skip(tour.Matches.Count - 2).Take(2);

            foreach (var participant in tour.Participants)
            {
                if (lastTwoMathces.Any(m => m.Teams.Any(t => t.Members.Any(m => m.Participant.Id == participant.Id))))
                {
                    continue;
                }
                participant.GamesInRow = 0;
                participant.CanPlay = true;
            }
        }

        private Match? CreateMatch(TournamentParticipant pivotPlayer, int teamMemberCount, List<TournamentParticipant> allParticipants)
        {
            var match = new Match()
            {
                Teams = new List<Team>
                {
                    new Team()
                    {
                        Members = new List<TeamMember>()
                    },
                    new Team()
                    {
                        Members = new List<TeamMember>()
                    }
                }
            };

            match.Teams[0].Members.Add(new TeamMember
            {
                Participant = pivotPlayer
            });
            match.Teams[1].Members.Add(new TeamMember
            {
                Participant = pivotPlayer.Opponent
            });

            pivotPlayer.CanPlay = false;
            pivotPlayer.Opponent.CanPlay = false;

            pivotPlayer.NotPlayedParticipants.Remove(pivotPlayer.Opponent);
            pivotPlayer.Opponent.NotPlayedParticipants.Remove(pivotPlayer);

            for (var i = 1; i < teamMemberCount; i++)
            {
                var opponent = GetOpponent(pivotPlayer, allParticipants);
                if (opponent == null)
                {
                    return null;
                }
                match.Teams[1].Members.Add(new TeamMember
                {
                    Participant = opponent
                });
                match.Teams[0].Members.Add(new TeamMember
                {
                    Participant = opponent.Opponent
                });
                opponent.CanPlay = false;
                opponent.Opponent.CanPlay = false;

                for (var j = 0; j < match.Teams[0].Members.Count; j++)
                {
                    match.Teams[0].Members[j].Participant.NotPlayedParticipants.Remove(opponent);
                    match.Teams[1].Members[j].Participant.NotPlayedParticipants.Remove(opponent.Opponent);
                    opponent.NotPlayedParticipants.Remove(match.Teams[0].Members[j].Participant);
                    opponent.Opponent.NotPlayedParticipants.Remove(match.Teams[1].Members[j].Participant);
                }
            }

            SetCanPlay(match);

            UpdateGamesInRow(match);

            return match;
        }

        private void SetCanPlay(Match match)
        {
            foreach (var team in match.Teams)
            {
                foreach (var member in team.Members)
                {
                    member.Participant.CanPlay = true;
                }
            }
        }

        private void UpdateGamesInRow(Match match)
        {
            foreach (var team in match.Teams)
            {
                foreach (var member in team.Members)
                {
                    member.Participant.GamesInRow++;
                    if (member.Participant.GamesInRow > 2)
                    {
                        member.Participant.CanPlay = false;
                    }
                }
            }
        }

        private TournamentParticipant? GetOpponent(TournamentParticipant pivotPlayer, List<TournamentParticipant> participants)
        {
            foreach (var opponent in pivotPlayer.NotPlayedParticipants)
            {
                //Хранить двух опонентов
                if (opponent.CanPlay && opponent.Opponent.CanPlay)
                {
                    return opponent;
                }
            }

            foreach (var participant in participants)
            {
                if (participant.CanPlay && participant.Opponent.CanPlay)
                {
                    return participant;
                }
            }
            return null;
        }

        private List<TournamentParticipant> LoadNotPlayed(List<TournamentParticipant> participants)
        {
            foreach (var participant in participants)
            {
                participant.NotPlayedParticipants = new List<TournamentParticipant>();
                participant.NotPlayedParticipants
                    .AddRange(participants.Where(p => p.Id != participant.Id).ToList());
            }
            var result = new List<TournamentParticipant>(participants);

            return participants;
        }

        private TournamentParticipant GetPivotPlayer(List<TournamentParticipant> participants)
        {
            var pivot = participants.OrderByDescending(p => p.NotPlayedParticipants.Count).FirstOrDefault(p => p.CanPlay && p.Opponent.CanPlay);

            return pivot;
        }
    }
}