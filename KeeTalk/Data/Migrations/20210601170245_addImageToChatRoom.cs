using Microsoft.EntityFrameworkCore.Migrations;

namespace KeeTalk.Data.Migrations
{
    public partial class addImageToChatRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ChatRoom",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ChatRoom");
        }
    }
}
