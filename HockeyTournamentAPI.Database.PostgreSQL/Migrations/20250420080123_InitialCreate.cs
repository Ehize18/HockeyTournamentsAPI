using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HockeyTournamentsAPI.Database.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(30)", nullable: false),
                    MiddleName = table.Column<string>(type: "varchar(30)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(30)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(11)", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    SportLevel = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    TelegramId = table.Column<int>(type: "integer", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    TrainerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TrainerId",
                table: "Users",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
