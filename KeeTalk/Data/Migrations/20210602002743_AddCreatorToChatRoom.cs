using Microsoft.EntityFrameworkCore.Migrations;

namespace KeeTalk.Data.Migrations
{
    public partial class AddCreatorToChatRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ChatRoom",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ChatRoom");
        }
    }
}
