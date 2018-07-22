using Microsoft.EntityFrameworkCore.Migrations;

namespace BLL.Migrations
{
    public partial class Unsignedint64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Facebook_id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Facebook_id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
