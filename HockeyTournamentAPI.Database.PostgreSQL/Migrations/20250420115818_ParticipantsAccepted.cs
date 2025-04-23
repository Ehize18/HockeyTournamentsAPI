using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyTournamentsAPI.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantsAccepted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "TournamentParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "TournamentParticipants");
        }
    }
}
