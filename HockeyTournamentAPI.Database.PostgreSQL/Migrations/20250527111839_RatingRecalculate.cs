using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyTournamentsAPI.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RatingRecalculate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "TournamentParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ToursPlayed",
                table: "TournamentParticipants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLastMatchInTour",
                table: "Matches",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToursPlayed",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "IsLastMatchInTour",
                table: "Matches");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "TournamentParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
