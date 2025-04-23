using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    public class MatchConfiguration : BaseConfiguration<Match>
    {
        public override void Configure(EntityTypeBuilder<Match> builder)
        {
            base.Configure(builder);

            builder.HasOne(m => m.Tour)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TourId);

            builder.HasOne(m => m.Referee)
                .WithMany()
                .HasForeignKey(m => m.RefereeId);

            builder.Property(t => t.StartTime)
                .HasColumnType("timestamptz");
            builder.Property(t => t.EndTime)
                .HasColumnType("timestamptz");
        }
    }
}
