using Microsoft.EntityFrameworkCore.Migrations;

namespace KeeTalk.Data.Migrations
{
    public partial class ChangeImageToImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ChatRoom",
                newName: "ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "ChatRoom",
                newName: "Image");
        }
    }
}
