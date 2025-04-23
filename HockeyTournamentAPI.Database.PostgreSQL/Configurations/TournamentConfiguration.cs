using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    public class TournamentConfiguration : BaseConfiguration<Tournament>
    {
        public override void Configure(EntityTypeBuilder<Tournament> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.Title)
                .HasColumnType("text");

            builder.Property(t => t.Description)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(t => t.StartTime)
                .HasColumnType("timestamptz");
            builder.Property(t => t.EndTime)
                .HasColumnType("timestamptz")
                .IsRequired(false);

            builder.HasMany(t => t.Participants)
                .WithOne(p => p.Tournament)
                .HasForeignKey(p => p.TournamentId);
        }
    }
}
