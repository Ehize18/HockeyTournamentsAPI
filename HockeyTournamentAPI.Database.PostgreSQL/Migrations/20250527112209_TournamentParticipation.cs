using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyTournamentsAPI.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class TournamentParticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanParticipate",
                table: "Tournaments",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanParticipate",
                table: "Tournaments");
        }
    }
}
