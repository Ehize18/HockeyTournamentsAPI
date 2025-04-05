using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    /// <summary>
    /// Базовая конфигурация.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt).HasColumnType("timestamptz");
            builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        }
    }
}
