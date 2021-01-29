using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.EFCore.DbMigrations.Migrations
{
    public partial class countname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "User");
        }
    }
}
