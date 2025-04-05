using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    /// <summary>
    /// Конфигурация ролей.
    /// </summary>
    public class RoleConfiguration : BaseConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .HasColumnType("varchar(30)");
            builder.Property(x => x.Description)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(x => x.Permissions)
                .HasColumnType("integer")
                .HasConversion(
                    x => (int)x,
                    x => (RolePermissions)x);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
