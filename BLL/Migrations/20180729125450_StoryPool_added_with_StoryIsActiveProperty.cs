using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BLL.Migrations
{
    public partial class StoryPool_added_with_StoryIsActiveProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Engine_machineId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Engine");

            migrationBuilder.DropIndex(
                name: "IX_Users_machineId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "machineId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "SavedState",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionCommand",
                table: "Stations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoryPools",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StarterStationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPools_Stations_StarterStationId",
                        column: x => x.StarterStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoryPools_StarterStationId",
                table: "StoryPools",
                column: "StarterStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoryPools");

            migrationBuilder.DropColumn(
                name: "SavedState",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "ConnectionCommand",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "machineId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Engine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_machineId",
                table: "Users",
                column: "machineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Engine_machineId",
                table: "Users",
                column: "machineId",
                principalTable: "Engine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
