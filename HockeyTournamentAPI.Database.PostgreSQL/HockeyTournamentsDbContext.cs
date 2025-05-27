using HockeyTournamentsAPI.Database.PostgreSQL.Configurations;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    /// <param name="options">Настройки контекста.</param>
    public class HockeyTournamentsDbContext(DbContextOptions<HockeyTournamentsDbContext> options)
        : DbContext(options)
    {
        /// <summary>
        /// Пользователи.
        /// </summary>
        public DbSet<User> Users { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new TourConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMemberConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseModel entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.UpdatedAt = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
