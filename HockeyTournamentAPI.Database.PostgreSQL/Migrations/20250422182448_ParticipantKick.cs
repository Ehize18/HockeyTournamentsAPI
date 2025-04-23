using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyTournamentsAPI.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantKick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsKicked",
                table: "TournamentParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsKicked",
                table: "TournamentParticipants");
        }
    }
}
