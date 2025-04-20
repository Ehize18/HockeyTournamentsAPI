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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
