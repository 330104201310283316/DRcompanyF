using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.EFCore.DbMigrations.Migrations
{
    public partial class insertAttachedPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachedPath",
                table: "WordInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachedPath",
                table: "WordInfo");
        }
    }
}
