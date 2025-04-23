using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    public class TeamMemberConfiguration : BaseConfiguration<TeamMember>
    {
        public override void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            base.Configure(builder);

            builder.HasOne(m => m.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(m => m.TeamId);

            builder.HasOne(m => m.Participant)
                .WithMany()
                .HasForeignKey(m => m.ParticipantId);
        }
    }
}
