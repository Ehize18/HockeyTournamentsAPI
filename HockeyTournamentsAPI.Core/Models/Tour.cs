namespace HockeyTournamentsAPI.Core.Models
{
    public class Tour : BaseModel
    {
        public Tournament Tournament { get; set; }
        public Guid TournamentId { get; set; }

        public int ParticipantsCount { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Match> Matches { get; set; }
        public List<TournamentParticipant> Participants { get; set; }
    }
}
