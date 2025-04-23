using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    public class TourConfiguration : BaseConfiguration<Tour>
    {
        public override void Configure(EntityTypeBuilder<Tour> builder)
        {
            base.Configure(builder);

            builder.HasOne(t => t.Tournament)
                .WithMany(t => t.Tours)
                .HasForeignKey(t => t.TournamentId);

            builder.Property(t => t.ParticipantsCount)
                .HasColumnType("integer");

            builder.Property(t => t.StartTime)
                .HasColumnType("timestamptz");
            builder.Property(t => t.EndTime)
                .HasColumnType("timestamptz");

            builder.Ignore(t => t.Participants);
        }
    }
}
