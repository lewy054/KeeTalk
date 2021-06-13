using Microsoft.EntityFrameworkCore.Migrations;

namespace KeeTalk.Data.Migrations
{
    public partial class AddClosedToThread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Thread",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Thread");
        }
    }
}
