using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    public class TournamentParticipantConfiguration: BaseConfiguration<TournamentParticipant>
    {
        public override void Configure(EntityTypeBuilder<TournamentParticipant> builder)
        {
            base.Configure(builder);

            builder.HasIndex(p => p.UserId)
                .IsUnique();

            builder.Property(p => p.IsAccepted)
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Property(p => p.RatingOnTournament)
                .HasColumnType("integer");

            builder.HasOne(p => p.Tournament)
                .WithMany(t => t.Participants)
                .HasForeignKey(p => p.TournamentId);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            builder.Property(p => p.IsKicked)
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Ignore(p => p.Opponent);
            builder.Ignore(p => p.GamesInRow);
            builder.Ignore(p => p.CanPlay);
            builder.Ignore(p => p.NotPlayedParticipants);
        }
    }
}
