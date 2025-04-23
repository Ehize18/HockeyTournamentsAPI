using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Configurations
{
    /// <summary>
    /// Конфигурация пользователей.
    /// </summary>
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FirstName)
                .HasColumnType("varchar(30)");
            builder.Property(x => x.MiddleName)
                .HasColumnType("varchar(30)");
            builder.Property(x => x.LastName)
                .HasColumnType("varchar(30)");

            builder.Property(x => x.BirthDate)
                .HasColumnType("date");

            builder.Property(x => x.IsMale)
                .HasColumnType("boolean")
                .HasColumnName("Gender");

            builder.Property(x => x.Email)
                .HasColumnType("varchar(100)");
            builder.Property(x => x.Phone)
                .HasColumnType("varchar(11)");
            builder.Property(x => x.PasswordHash)
                .HasColumnType("text");

            builder.Property(x => x.SportLevel)
                .HasColumnType("text");
            builder.Property(x => x.Rating)
                .HasColumnType("integer");

            builder.Property(x => x.TelegramId)
                .HasColumnType("integer");

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.TrainerId)
                .IsRequired(false);

            builder.Ignore(u => u.Tournaments);
        }
    }
}
