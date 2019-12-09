using Microsoft.EntityFrameworkCore.Migrations;

namespace Watcher.DAL.Migrations
{
    public partial class Remove_NotifyMyAndroid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyMyAndroidKey",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotifyMyAndroidKey",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
