using Microsoft.EntityFrameworkCore.Migrations;

namespace KeeTalk.Data.Migrations
{
    public partial class ChangeThreadModelToThread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ThreadModel",
                table: "ThreadModel");

            migrationBuilder.RenameTable(
                name: "ThreadModel",
                newName: "Thread");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Thread",
                table: "Thread",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Thread",
                table: "Thread");

            migrationBuilder.RenameTable(
                name: "Thread",
                newName: "ThreadModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThreadModel",
                table: "ThreadModel",
                column: "Id");
        }
    }
}
