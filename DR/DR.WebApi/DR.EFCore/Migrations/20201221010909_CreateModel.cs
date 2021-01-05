using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.EFCore.DbMigrations.Migrations
{
    public partial class CreateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Disable = table.Column<bool>(nullable: false),
                    ConfigKey = table.Column<string>(maxLength: 50, nullable: false),
                    ConfigValues = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Disable = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Xxx = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Disable = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    LoginType = table.Column<int>(nullable: false),
                    AuthRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PictureInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Disable = table.Column<bool>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    PictureContent = table.Column<string>(nullable: false),
                    PictureTitle = table.Column<string>(nullable: false),
                    PictureExplain = table.Column<string>(nullable: false),
                    RecommendIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PictureInfo_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Disable = table.Column<bool>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    HtmlContent = table.Column<string>(nullable: false),
                    HtmlTitle = table.Column<string>(nullable: false),
                    HtmlExplain = table.Column<string>(nullable: false),
                    ArticleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordInfo_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PictureInfo_UserID",
                table: "PictureInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WordInfo_UserID",
                table: "WordInfo",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PictureInfo");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "WordInfo");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
