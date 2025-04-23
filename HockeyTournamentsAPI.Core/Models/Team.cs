namespace HockeyTournamentsAPI.Core.Models
{
    public class Team : BaseModel
    {
        public Match Match { get; set; }
        public Guid MatchId { get; set; }

        public int Goals { get; set; }

        public List<TeamMember> Members { get; set; }
    }
}
