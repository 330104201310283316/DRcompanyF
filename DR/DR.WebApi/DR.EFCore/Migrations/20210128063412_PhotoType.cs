using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.EFCore.DbMigrations.Migrations
{
    public partial class PhotoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoType",
                table: "PictureInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoType",
                table: "PictureInfo");
        }
    }
}
