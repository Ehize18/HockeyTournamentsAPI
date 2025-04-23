namespace HockeyTournamentsAPI.Core.Models
{
    public class TeamMember : BaseModel
    {
        public Team Team { get; set; }
        public Guid TeamId { get; set; }

        public TournamentParticipant Participant { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
